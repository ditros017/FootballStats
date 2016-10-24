﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FootballStats.Domain;

namespace FootballStats.Web.Models.Referee
{
    public class CreateModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public RefereeType Type { get; set; }
    }
}