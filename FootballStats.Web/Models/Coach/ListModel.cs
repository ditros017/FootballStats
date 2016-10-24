using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballStats.Web.Models.Coach
{
    public class ListModel
    {
        public IEnumerable<Coach> Coaches { get; set; }

        public class Coach
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}