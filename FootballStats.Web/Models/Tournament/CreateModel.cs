using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballStats.Web.Models.Tournament
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}