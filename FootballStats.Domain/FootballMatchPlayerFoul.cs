using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class FootballMatchPlayerFoul : EntityBase
    {
        public int FootballMatchPlayerId { get; set; }
        public FootballMatchPlayer FootballMatchPlayer { get; set; }

        public FoulType Type { get; set; }
        public TimeSpan Time { get; set; }
    }

    public enum FoulType
    {
        YellowCard,
        RedCard
    }
}