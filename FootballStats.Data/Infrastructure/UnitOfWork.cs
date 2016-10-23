using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballStats.Domain;

namespace FootballStats.Data.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private readonly FootballStatsDbContext _dbContext = new FootballStatsDbContext();

        private bool _disposed;

        private Repository<FootballMatch> _footballMatchRepository;
        private Repository<Tournament> _tournamentRepository;
        private Repository<FootballMatchPlayerGoal> _footballMatchPlayerGoalRepository;
        private Repository<FootballMatchPlayer> _footballMatchPlayerRepository;
        private Repository<Team> _teamRepository;
        private Repository<Player> _playerRepository;
        private Repository<Coach> _coachRepository;
        private Repository<Referee> _refereeRepository;
        //private Repository<TechnicalSamplingResult> _technicalSamplingResultRepository;

        public Repository<FootballMatch> FootballMatchRepository => _footballMatchRepository ?? (_footballMatchRepository = new Repository<FootballMatch>(_dbContext));
        public Repository<Tournament> TournamentRepository => _tournamentRepository ?? (_tournamentRepository = new Repository<Tournament>(_dbContext));
        public Repository<FootballMatchPlayerGoal> FootballMatchPlayerGoalRepository => _footballMatchPlayerGoalRepository ?? (_footballMatchPlayerGoalRepository = new Repository<FootballMatchPlayerGoal>(_dbContext));
        public Repository<FootballMatchPlayer> FootballMatchPlayerRepository => _footballMatchPlayerRepository ?? (_footballMatchPlayerRepository = new Repository<FootballMatchPlayer>(_dbContext));
        public Repository<Team> TeamRepository => _teamRepository ?? (_teamRepository = new Repository<Team>(_dbContext));
        public Repository<Player> PlayerRepository => _playerRepository ?? (_playerRepository = new Repository<Player>(_dbContext));
        public Repository<Coach> CoachRepository => _coachRepository ?? (_coachRepository = new Repository<Coach>(_dbContext));
        public Repository<Referee> RefereeRepository => _refereeRepository ?? (_refereeRepository = new Repository<Referee>(_dbContext));
        //public Repository<TechnicalSamplingResult> TechnicalSamplingResultRepository => _technicalSamplingResultRepository ?? (_technicalSamplingResultRepository = new Repository<TechnicalSamplingResult>(_dbContext));

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}