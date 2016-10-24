using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballStats.Web.Models.Team
{
    public class EditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        public int CoachId { get; set; }

        public IEnumerable<SelectListItem> Coaches { get; set; }
    }
}