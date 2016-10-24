using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Domain;
using FootballStats.Web.Models.FootballMatch;

namespace FootballStats.Web.Controllers
{
    public class FootballMatchesController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Details(int id)
        {
            var model = _unitOfWork.FootballMatchRepository.GetById(id, m => new DetailsModel
            {
                Id = m.Id,
                TournamentId = m.TournamentId,
                TournamentName = m.Tournament.Name,
                Teams = m.FootballMatchTeams.Select(mt => new DetailsModel.Team
                {
                    Id = mt.TeamId,
                    Name = mt.Team.Name,
                    IsGuest = mt.IsGuest,
                    Coach = new DetailsModel.Coach
                    {
                        Id = mt.Team.CoachId,
                        Name = mt.Team.Coach.LastName + " " + mt.Team.Coach.FirstName
                    },
                    Players = m.FootballMatchPlayers.Where(mp => mp.Player.TeamId == mt.TeamId).Select(mp => new DetailsModel.Player
                    {
                        Id = mp.PlayerId,
                        Name = mp.Player.LastName + " " + mp.Player.FirstName,
                        IsStarted = mp.IsStarted,
                        GetAwayTime = mp.GetAwayTime,
                        EnterTime = mp.EnterTime,
                        Fouls = mp.FootballMatchPlayerFouls.Select(f => new DetailsModel.Foul
                        {
                            Type = f.Type,
                            Time = f.Time
                        }),
                        Goals = mp.FootballMatchPlayerGoals.Select(g => new DetailsModel.Goal
                        {
                            Type = g.Type,
                            Time = g.Time
                        })
                    })
                }),
                Referees = m.FootballMatchReferees.Select(r => new DetailsModel.Referee
                {
                    Id = r.RefereeId,
                    Name = r.Referee.LastName + " " + r.Referee.FirstName,
                    Type = r.Referee.Type
                })
            });

            return View("~/Views/FootballMatches/Details.cshtml", model);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View("~/Views/FootballMatches/List.cshtml", new ListModel
            {
                FootballMatches = _unitOfWork.FootballMatchRepository.GetAll(m => new ListModel.FootballMatch
                {
                    Id = m.Id,
                    TournamentName = m.Tournament.Name,
                    Teams = m.FootballMatchTeams.Select(mt => new ListModel.Team
                    {
                        Name = mt.Team.Name,
                        IsGuest = mt.IsGuest
                    })
                }).ToArray()
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateModel
            {
                Tournaments = _unitOfWork.TournamentRepository.GetAll(t => new CreateModel.Tournament
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToArray(),
                Teams = _unitOfWork.TeamRepository.GetAll(t => new CreateModel.Team
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToArray(),
                Referees = _unitOfWork.RefereeRepository.GetAll(r => new CreateModel.Referee
                {
                    Id = r.Id,
                    Name = r.LastName + " " + r.FirstName,
                    Type = r.Type
                }).ToArray()
            };

            return View("~/Views/FootballMatches/Create.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                var footballMatch = new FootballMatch
                {
                    TournamentId = model.TournamentId,
                    StageType = model.StageType
                };

                _unitOfWork.FootballMatchRepository.Save(footballMatch);
                _unitOfWork.Commit();

                AddFootballMatchTeam(footballMatch.Id, model.HomeTeamId, false);
                AddFootballMatchPlayers(footballMatch.Id, _unitOfWork.PlayerRepository.GetMany(p => p.TeamId == model.HomeTeamId, p => p.Id).ToArray());

                AddFootballMatchTeam(footballMatch.Id, model.GuestTeamId, true);
                AddFootballMatchPlayers(footballMatch.Id, _unitOfWork.PlayerRepository.GetMany(p => p.TeamId == model.GuestTeamId, p => p.Id).ToArray());

                _unitOfWork.Commit();

                return RedirectToAction("List");
            }

            model.Tournaments = _unitOfWork.TournamentRepository.GetAll(t => new CreateModel.Tournament
            {
                Id = t.Id,
                Name = t.Name
            }).ToArray();
            model.Teams = _unitOfWork.TeamRepository.GetAll(t => new CreateModel.Team
            {
                Id = t.Id,
                Name = t.Name
            }).ToArray();
            model.Referees = _unitOfWork.RefereeRepository.GetAll(r => new CreateModel.Referee
            {
                Id = r.Id,
                Name = r.LastName + " " + r.FirstName,
                Type = r.Type
            }).ToArray();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return null;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _unitOfWork.FootballMatchRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(new {redirectToUrl = Url.Action("List")});
        }

        private void AddFootballMatchTeam(int footballMatchId, int teamId, bool isGuest)
        {
            _unitOfWork.FootballMatchTeamRepository.Save(new FootballMatchTeam
            {
                FootballMatchId = footballMatchId,
                TeamId = teamId,
                IsGuest = isGuest
            });
        }

        private void AddFootballMatchPlayers(int footballMatchId, IEnumerable<int> playerIds)
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
                    GetAwayTime = index == getAwayIndex ? TimeSpan.FromMinutes(minuteToSubstitute) : (TimeSpan?)null
                };

                _unitOfWork.FootballMatchPlayerRepository.Save(footballMatchPlayer);

                if (index % 3 == 0 && index != getAwayIndex)
                {
                    _unitOfWork.Commit();

                    _unitOfWork.FootballMatchPlayerFoulRepository.Save(new FootballMatchPlayerFoul
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(1, 90)),
                        Type = FoulType.YellowCard
                    });
                }

                if (random.Next(1, 11) == index && index != getAwayIndex)
                {
                    _unitOfWork.Commit();

                    _unitOfWork.FootballMatchPlayerGoalRepository.Save(new FootballMatchPlayerGoal
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(1, index == getAwayIndex ? minuteToSubstitute : 90)),
                        Type = index % 4 == 0 ? GoalType.Penalty : GoalType.Game
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

                _unitOfWork.FootballMatchPlayerRepository.Save(footballMatchPlayer);

                if (random.Next(1, 7) == index && index == enterIndex)
                {
                    _unitOfWork.Commit();

                    _unitOfWork.FootballMatchPlayerFoulRepository.Save(new FootballMatchPlayerFoul
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(minuteToSubstitute, 90)),
                        Type = FoulType.YellowCard,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (random.Next(1, 7) == index && index == enterIndex)
                {
                    _unitOfWork.Commit();

                    _unitOfWork.FootballMatchPlayerGoalRepository.Save(new FootballMatchPlayerGoal
                    {
                        FootballMatchPlayerId = footballMatchPlayer.Id,
                        Time = TimeSpan.FromMinutes(random.Next(minuteToSubstitute, 90)),
                        Type = random.Next(1, 7) == 5 ? GoalType.Penalty : GoalType.Game,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                index++;
            }

            _unitOfWork.Commit();
        }
    }
}