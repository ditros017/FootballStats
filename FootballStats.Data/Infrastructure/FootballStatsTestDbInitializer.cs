using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballStats.Domain;

namespace FootballStats.Data.Infrastructure
{
    public class FootballStatsTestDbInitializer : DropCreateDatabaseIfModelChanges<FootballStatsDbContext>
    {
        protected override void Seed(FootballStatsDbContext context)
        {
            var tournamentIds = AddTournaments(context);
            var teamIds = AddTeams(context);
            var teamsPlayerIds = AddPlayers(context, teamIds);
            var referees = AddReferees(context);

            AddFootballMatches(context, tournamentIds, teamsPlayerIds, referees);
        }

        private static Dictionary<int, IEnumerable<int>> AddPlayers(FootballStatsDbContext context, IEnumerable<int> teamIds)
        {
            var teamsPlayerIds = new Dictionary<int, IEnumerable<int>>();

            foreach (var teamId in teamIds)
            {
                var playerIds = new List<int>();

                for (var i = 1; i <= 17; i++)
                {
                    var player = new Player
                    {
                        TeamId = teamId,
                        CreatedAt = DateTime.UtcNow
                    };
                    InitializePerson(player, "Player", i.ToString());

                    context.Players.Add(player);
                    context.SaveChanges();

                    playerIds.Add(player.Id);
                }

                teamsPlayerIds.Add(teamId, playerIds);
            }

            return teamsPlayerIds;
        }

        private static IEnumerable<int> AddTournaments(FootballStatsDbContext context)
        {
            var tournamentIds = new List<int>();

            for (var i = 1; i <= 10; i++)
            {
                var tournament = new Tournament
                {
                    Name = $"Tournament {i}",
                    CreatedAt = DateTime.UtcNow
                };

                context.Tournaments.Add(tournament);
                context.SaveChanges();

                tournamentIds.Add(tournament.Id);
            }

            return tournamentIds;
        }

        private static IEnumerable<int> AddTeams(FootballStatsDbContext context)
        {
            var teamIds = new List<int>();

            for (var i = 1; i <= 10; i++)
            {
                var team = new Team
                {
                    Name = $"Team {i}",
                    Country = "Russian Federation",
                    City = "Tomsk",
                    CoachId = AddCoach(context, i),
                    CreatedAt = DateTime.UtcNow
                };

                context.Teams.Add(team);
                context.SaveChanges();

                teamIds.Add(team.Id);
            }

            return teamIds;
        }

        private static int AddCoach(FootballStatsDbContext context, int index)
        {
            var coach = new Coach
            {
                CreatedAt = DateTime.UtcNow
            };
            InitializePerson(coach, "Coach", index.ToString());

            context.Coaches.Add(coach);
            context.SaveChanges();

            return coach.Id;
        }

        private static void InitializePerson(PersonBase person, string prefix, string postfix)
        {
            person.FirstName = $"{prefix} FirstName {postfix}";
            person.LastName = $"{prefix} LastName {postfix}";
            person.MiddleName = $"{prefix} MiddleName {postfix}";
            person.DateOfBirth = DateTime.UtcNow;
        }

        private static IEnumerable<Referee> AddReferees(FootballStatsDbContext context)
        {
            var referees = new List<Referee>();

            for (var i = 1; i <= 10; i++)
            {
                var referee = new Referee
                {
                    Type = i%2 == 0 ? RefereeType.Linesman : RefereeType.Main,
                    CreatedAt = DateTime.UtcNow
                };
                InitializePerson(referee, "Referee", i.ToString());

                context.Referees.Add(referee);
                context.SaveChanges();

                referees.Add(referee);
            }

            return referees;
        }

        private static void AddFootballMatches(FootballStatsDbContext context, IEnumerable<int> tournamentIds, Dictionary<int, IEnumerable<int>> teamsPlayerIds, IEnumerable<Referee> referees)
        {
            var teamIds = teamsPlayerIds.Keys.ToArray();
            var random = new Random();

            foreach (var tournamentId in tournamentIds)
            {
                for (var i = 1; i <= 10; i++)
                {
                    var footballMatchId = AddFootballMatch(context, tournamentId);
                    AddFootballMatchReferees(context, footballMatchId, referees);

                    var homeTeamId = teamIds[random.Next(0, teamIds.Length - 1)];
                    AddFootballMatchTeam(context, footballMatchId, homeTeamId, false);
                    AddFootballMatchPlayers(context, footballMatchId, teamsPlayerIds[homeTeamId]);

                    var guestTeamId = GetGuestTeamId(random, teamIds, homeTeamId);
                    AddFootballMatchTeam(context, footballMatchId, guestTeamId, true);
                    AddFootballMatchPlayers(context, footballMatchId, teamsPlayerIds[guestTeamId]);

                    context.SaveChanges();
                }
            }

            context.SaveChanges();
        }

        private static void AddFootballMatchPlayers(FootballStatsDbContext context, int footballMatchId, IEnumerable<int> playerIds)
        {
            var index = 1;
            var random = new Random();
            var minuteToSubstitute = random.Next(1, 90);
            var getAwayIndex = random.Next(1, 11);

            foreach (var playerId in playerIds.Take(11).ToArray())
            {
                var footballMatchPlayer = new FootballMatchPlayer
                {
                    FootballMatchId = footballMatchId,
                    PlayerId = playerId,
                    IsStarted = true,
                    GetAwayTime = index == getAwayIndex ? TimeSpan.FromMinutes(minuteToSubstitute) : (TimeSpan?) null,
                    CreatedAt = DateTime.UtcNow
                };

                context.FootballMatchPlayers.Add(footballMatchPlayer);

                if (index % 3 == 0 && index != getAwayIndex)
                {
                    context.SaveChanges();

                    context.FootballMatchPlayerFouls.Add(new FootballMatchPlayerFoul
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(1, 90)),
                        Type = FoulType.YellowCard,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (random.Next(1, 11) == index && index != getAwayIndex)
                {
                    context.SaveChanges();

                    context.FootballMatchPlayerGoals.Add(new FootballMatchPlayerGoal
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(1, index == getAwayIndex ? minuteToSubstitute : 90)),
                        Type = index % 4 == 0 ? GoalType.Penalty : GoalType.Game,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                index++;
            }

            index = 1;
            var enterIndex = random.Next(1, 7);
            foreach (var playerId in playerIds.Skip(11).ToArray())
            {
                var footballMatchPlayer = new FootballMatchPlayer
                {
                    FootballMatchId = footballMatchId,
                    PlayerId = playerId,
                    IsStarted = false,
                    EnterTime = index == enterIndex ? TimeSpan.FromMinutes(minuteToSubstitute) : (TimeSpan?)null,
                    CreatedAt = DateTime.UtcNow
                };

                context.FootballMatchPlayers.Add(footballMatchPlayer);

                if (random.Next(1, 7) == index && index == enterIndex)
                {
                    context.SaveChanges();

                    context.FootballMatchPlayerFouls.Add(new FootballMatchPlayerFoul
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(minuteToSubstitute, 90)),
                        Type = FoulType.YellowCard,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (random.Next(1, 7) == index && index == enterIndex)
                {
                    context.SaveChanges();

                    context.FootballMatchPlayerGoals.Add(new FootballMatchPlayerGoal
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(minuteToSubstitute, 90)),
                        Type = random.Next(1, 7) == 5 ? GoalType.Penalty : GoalType.Game,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                index++;
            }

            context.SaveChanges();
        }

        private static void AddFootballMatchTeam(FootballStatsDbContext context, int footballMatchId, int teamId, bool isGuest)
        {
            context.FootballMatchTeams.Add(new FootballMatchTeam
            {
                FootballMatchId = footballMatchId,
                TeamId = teamId,
                IsGuest = isGuest,
                CreatedAt = DateTime.UtcNow
            });
        }

        private static int GetGuestTeamId(Random random, int[] teamIds, int homeTeamId)
        {
            while (true)
            {
                var guestTeamId = teamIds[random.Next(0, teamIds.Length - 1)];
                if (guestTeamId == homeTeamId)
                    continue;

                return guestTeamId;
            }
        }

        private static void AddFootballMatchReferees(FootballStatsDbContext context, int footballMatchId, IEnumerable<Referee> referees)
        {
            context.FootballMatchReferees.Add(new FootballMatchReferee
            {
                FootballMatchId = footballMatchId,
                RefereeId = referees.First(r => r.Type == RefereeType.Main).Id,
                CreatedAt = DateTime.UtcNow
            });

            foreach (var referee in referees.Where(r => r.Type == RefereeType.Linesman).Take(2).ToArray())
            {
                context.FootballMatchReferees.Add(new FootballMatchReferee
                {
                    FootballMatchId = footballMatchId,
                    RefereeId = referee.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }

            context.SaveChanges();
        }

        private static int AddFootballMatch(FootballStatsDbContext context, int tournamentId)
        {
            var footballMatch = new FootballMatch
            {
                TournamentId = tournamentId,
                StageType = FootballMatchStageType.GroupStage,
                CreatedAt = DateTime.UtcNow
            };

            context.FootballMatches.Add(footballMatch);
            context.SaveChanges();

            return footballMatch.Id;
        }
    }
}