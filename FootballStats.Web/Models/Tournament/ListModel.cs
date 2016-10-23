using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Tournament
{
    public class ListModel
    {
        public IEnumerable<Tournament> Tournaments { get; set; }

        public ListModel()
        {
            Tournaments = new List<Tournament>();
        }

        public class Tournament
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int FootballMatchCount { get; set; }
        }
    }
}