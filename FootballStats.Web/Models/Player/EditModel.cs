using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballStats.Web.Models.Player
{
    public class EditModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public int TeamId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}