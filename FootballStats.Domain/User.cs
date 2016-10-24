using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FootballStats.Domain
{
    public class User : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        None,
        Admin
    }
}
