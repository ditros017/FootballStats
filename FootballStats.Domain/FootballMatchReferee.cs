using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class FootballMatchReferee : EntityBase
    {
        public int FootballMatchId { get; set; }
        public FootballMatch FootballMatch { get; set; }

        public int RefereeId { get; set; }
        public Referee Referee { get; set; }
    }
}