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
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<FootballMatch> FootballMatches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<FootballMatchReferee> FootballMatchReferees { get; set; }
        public DbSet<FootballMatchPlayer> FootballMatchPlayers { get; set; }
        public DbSet<FootballMatchPlayerGoal> FootballMatchPlayerGoals { get; set; }
        public DbSet<FootballMatchPlayerFoul> FootballMatchPlayerFouls { get; set; }
        public DbSet<FootballMatchTeam> FootballMatchTeams { get; set; }

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