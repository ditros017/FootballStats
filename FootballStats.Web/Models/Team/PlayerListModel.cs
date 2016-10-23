using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Team
{
    public class PlayerListModel
    {
        public IEnumerable<Player> Players { get; set; }

        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public int FootballMatchCount { get; set; }
            public int FootballMatchGoalCount { get; set; }
        }
    }
}