using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Team
{
    public class ListModel
    {
        public IEnumerable<Team> Teams { get; set; }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public int PlayerCount { get; set; }
        }
    }
}