using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballStats.Domain;

namespace FootballStats.Web.Models.FootballMatch
{
    public class DetailsModel
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public string TournamentName { get; set; }

        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Referee> Referees { get; set; }

        public string Name => $"{Teams.Single(t => !t.IsGuest)} – {Teams.Single(t => t.IsGuest)}";

        public class Team
        {
            public int Id { get; set; }
            public bool IsGuest { get; set; }
            public string Name { get; set; }
            public Coach Coach { get; set; }

            public IEnumerable<Player> Players { get; set; }

            public int Score => Players.SelectMany(p => p.Goals).Count();

            public override string ToString()
            {
                return IsGuest ? $"({Score}) {Name}" : $"{Name} ({Score})";
            }
        }

        public class Coach
        {
            public int Id { get; set; }
            public string Name { get; set; }
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

        public class Referee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public RefereeType Type { get; set; }
        }
    }
}