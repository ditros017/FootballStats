using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Data
{
    public class DataConfig
    {
        public static string FootballStatsDbConnectionString => ConfigurationManager.ConnectionStrings["FootballStatsDb"].ConnectionString;
    }
}
