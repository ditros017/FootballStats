using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballStats.Domain;

namespace FootballStats.Data
{
    public class FootballStatsDbContext : DbContext
    {
        public DbSet<FootballMatch> FootballMatches { get; set; }
        //public DbSet<Component> Components { get; set; }
        //public DbSet<BlastingCap> BlastingCaps { get; set; }
        //public DbSet<Inhibitor> Inhibitors { get; set; }
        //public DbSet<Site> Sites { get; set; }
        //public DbSet<IntermediateGrille> IntermediateGrilles { get; set; }
        //public DbSet<TechnicalSamplingResult> TechnicalSamplingResults { get; set; }
        //public DbSet<PressingScheme> PressingSchemes { get; set; }
        //public DbSet<Filling> Fillings { get; set; }

        public FootballStatsDbContext() : base(DataConfig.FootballStatsDbConnectionString)
        {
            SetupConfiguration();
        }

        public FootballStatsDbContext(string connString)
            : base(connString)
        {
            SetupConfiguration();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ComplexType<RoundConfirmationPageSettings>();

            //modelBuilder.Entity<LocalRound>().HasMany(c => c.Judges).WithMany(a => a.LocalRounds).Map(m => m.ToTable("JudgeLocalRounds", ContextName));
            //modelBuilder.Entity<Judge>().HasMany(c => c.ApplicationJudgements).WithMany(a => a.Judges).Map(m => m.ToTable("ApplicationJudgementJudges", ContextName));
            //modelBuilder.Entity<Solicitation>().HasMany(c => c.JudgeEntryForms).WithMany(a => a.Solicitations).Map(m => m.ToTable("JudgeEntryFormSolicitations", ContextName));
            //modelBuilder.Entity<WinnerCategory>().HasMany(c => c.ApplicationJudgements).WithMany(a => a.WinnerCategories).Map(m => m.ToTable("ApplicationJudgementWinnerCategories", ContextName));
            //modelBuilder.Entity<LocalRound>().HasMany(c => c.ApplicationCategoriesRestrictedForJudging).WithMany(a => a.LocalRoundsWhereCategoryIsRestrictedForJudging).Map(m => m.ToTable("AppliationCategoriesRestrictedForJudgingInLocalRounds", ContextName));
        }

        private void SetupConfiguration()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
