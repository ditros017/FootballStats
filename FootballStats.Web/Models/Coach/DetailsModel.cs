using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Coach
{
    public class DetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Team CoachingTeam { get; set; }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}