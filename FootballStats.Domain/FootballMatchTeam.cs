using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class FootballMatchTeam : EntityBase
    {
        public int FootballMatchId { get; set; }
        public FootballMatch FootballMatch { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public bool IsGuest { get; set; }
    }
}