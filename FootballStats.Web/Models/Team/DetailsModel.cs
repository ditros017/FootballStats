using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Team
{
    public class DetailsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int PlayerCount { get; set; }
        public int FootballMatchCount { get; set; }
        public CoachModel Coach { get; set; }

        public class CoachModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}