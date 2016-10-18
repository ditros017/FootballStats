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
        [Required, StringLength(255)]
        public string Name { get; set; }
    }
}