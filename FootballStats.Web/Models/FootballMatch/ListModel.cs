using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.FootballMatch
{
    public class ListModel
    {
        public IEnumerable<FootballMatch> FootballMatches { get; set; }

        public class FootballMatch
        {
            public int Id { get; set; }
            public string TournamentName { get; set; }

            public IEnumerable<Team> Teams { get; set; }

            public string Name => $"{Teams.Single(t => !t.IsGuest).Name} – {Teams.Single(t => t.IsGuest).Name}";
        }

        public class Team
        {
            public bool IsGuest { get; set; }
            public string Name { get; set; }
        }
    }
}