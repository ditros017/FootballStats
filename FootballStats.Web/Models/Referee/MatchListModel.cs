using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballStats.Domain;

namespace FootballStats.Web.Models.Referee
{
    public class MatchListModel
    {
        public IEnumerable<FootballMatch> FootballMatches { get; set; }

        public class FootballMatch
        {
            public int Id { get; set; }
            public int TournamentId { get; set; }
            public string TournamentName { get; set; }
            public FootballMatchStageType StageType { get; set; }

            public IEnumerable<Team> Teams { get; set; }

            public class Team
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public bool IsGuest { get; set; }
                public int Score { get; set; }

                public override string ToString()
                {
                    return IsGuest ? $"({Score}) {Name}" : $"{Name} ({Score})";
                }
            }

            public override string ToString()
            {
                return $"{Teams.Single(t => !t.IsGuest)} – {Teams.Single(t => t.IsGuest)}";
            }
        }
    }
}