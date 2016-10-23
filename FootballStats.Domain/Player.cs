using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class Player : PersonBase
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<FootballMatchPlayer> FootballMatchPlayers { get; set; }
    }
}