using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Player
{
    public class ListModel
    {
        public IEnumerable<Player> Players { get; set; }

        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string TeamName { get; set; }
        }
    }
}