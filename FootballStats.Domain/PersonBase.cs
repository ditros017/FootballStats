using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Domain
{
    public abstract class PersonBase : EntityBase
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}