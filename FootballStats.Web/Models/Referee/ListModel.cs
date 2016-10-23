using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FootballStats.Domain;

namespace FootballStats.Web.Models.Referee
{
    public class ListModel
    {
        public IEnumerable<Referee> Referees { get; set; }

        public class Referee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public RefereeType Type { get; set; }
        }
    }
}