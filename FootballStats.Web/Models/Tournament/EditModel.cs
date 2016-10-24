using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Tournament
{
    public class EditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}