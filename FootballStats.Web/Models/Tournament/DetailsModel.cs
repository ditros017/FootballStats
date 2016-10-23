using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballStats.Domain;

namespace FootballStats.Web.Models.Tournament
{
    public class DetailsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<FootballMatch> FootballMatches { get; set; }

        public DetailsModel()
        {
            FootballMatches = new List<FootballMatch>();
        }

        public class FootballMatch
        {
            public int Id { get; set; }
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