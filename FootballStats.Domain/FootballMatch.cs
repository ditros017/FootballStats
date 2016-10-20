using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public class FootballMatch : EntityBase
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public FootballMatchStageType StageType { get; set; }

        public ICollection<FootballMatchTeam> FootballMatchTeams { get; set; }
        public ICollection<FootballMatchPlayer> FootballMatchPlayers { get; set; }
        public ICollection<FootballMatchReferee> FootballMatchReferees { get; set; }
    }

    public enum FootballMatchStageType
    {
        GroupStage,
        Quarterfinal,
        SemiFinal,
        Final
    }
}