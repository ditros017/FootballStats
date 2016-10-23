using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Player
{
    public class DetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int FootballMatchCount { get; set; }
        public int FootballMatchGoalCount { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}