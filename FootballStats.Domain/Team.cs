using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class Team : EntityBase
    {
        public int CoachId { get; set; }
        public Coach Coach { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Country { get; set; }

        [Required, StringLength(100)]
        public string City { get; set; }

        public ICollection<Player> Players { get; set; }
        public ICollection<FootballMatchTeam> FootballMatchTeams { get; set; }
    }
}