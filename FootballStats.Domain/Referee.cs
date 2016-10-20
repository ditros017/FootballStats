using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class Referee : PersonBase
    {
        public RefereeType Type { get; set; }

        public ICollection<FootballMatchReferee> FootballMatchReferees { get; set; }
    }

    public enum RefereeType
    {
        Main,
        Linesman
    }
}