using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballStats.Domain;

namespace FootballStats.Data.Infrastructure
{
    public class FootballStatsTestDbInitializer : DropCreateDatabaseIfModelChanges<FootballStatsDbContext>
    {
        protected override void Seed(FootballStatsDbContext context)
        {
            AddFootballMatches(context);
        }

        private static void AddFootballMatches(FootballStatsDbContext context)
        {
            for (var i = 1; i <= 10; i++)
            {
                context.FootballMatches.Add(new FootballMatch
                {
                    Name = $"Match {i}",
                    CreatedAt = DateTime.UtcNow
                });
            }

            context.SaveChanges();
        }
    }
}