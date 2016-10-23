using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Referee
{
    public class DetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int FootballMatchCount { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}