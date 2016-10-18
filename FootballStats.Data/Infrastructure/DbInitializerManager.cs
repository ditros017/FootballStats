using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Data.Infrastructure
{
    public static class DbInitializerManager
    {
        public static void DropCreateDbIfModelChanges()
        {
            Database.SetInitializer(new FootballStatsTestDbInitializer());

            using (var context = new FootballStatsDbContext())
            {
                context.Database.Initialize(false);
            }
        }

        public static void DontTrackChanges()
        {
            Database.SetInitializer<FootballStatsDbContext>(null);
        }
    }
}