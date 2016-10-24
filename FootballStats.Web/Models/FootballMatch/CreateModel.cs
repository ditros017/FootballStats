using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballStats.Domain;

namespace FootballStats.Web.Models.FootballMatch
{
    public class CreateModel
    {
        public int TournamentId { get; set; }
        public FootballMatchStageType StageType { get; set; }

        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }

        public int MainRefereeId { get; set; }
        public int FirstLinesmanRefereeId { get; set; }
        public int SecondLinesmanRefereeId { get; set; }

        public IEnumerable<Tournament> Tournaments { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Referee> Referees { get; set; }

        public class Tournament
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        public class Referee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public RefereeType Type { get; set; }
        }

        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public bool IsStarted { get; set; }
            public TimeSpan? GetAwayTime { get; set; }
            public TimeSpan? EnterTime { get; set; }

            public IEnumerable<Goal> Goals { get; set; }
            public IEnumerable<Foul> Fouls { get; set; }
        }

        public class Goal
        {
            public GoalType Type { get; set; }
            public TimeSpan Time { get; set; }

            public override string ToString()
            {
                return $"{Time.TotalMinutes}'";
            }
        }

        public class Foul
        {
            public FoulType Type { get; set; }
            public TimeSpan Time { get; set; }

            public override string ToString()
            {
                return $"{Time.TotalMinutes}'";
            }
        }
    }
}