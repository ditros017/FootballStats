using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class FootballMatchPlayer : EntityBase
    {
        public int FootballMatchId { get; set; }
        public FootballMatch FootballMatch { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public bool IsStarted { get; set; }

        public TimeSpan? EnterTime { get; set; }
        public TimeSpan? GetAwayTime { get; set; }

        public ICollection<FootballMatchPlayerGoal> FootballMatchPlayerGoals { get; set; }
        public ICollection<FootballMatchPlayerFoul> FootballMatchPlayerFouls { get; set; }
    }
}