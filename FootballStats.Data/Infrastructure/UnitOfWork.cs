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
        //private Repository<Component> _componentRepository;
        //private Repository<Filling> _fillingRepository;
        //private Repository<Inhibitor> _inhibitorRepository;
        //private Repository<IntermediateGrille> _intermediateGrilleRepository;
        //private Repository<PressingScheme> _pressingSchemeRepository;
        //private Repository<Product> _productRepository;
        //private Repository<Site> _siteRepository;
        //private Repository<TechnicalSamplingResult> _technicalSamplingResultRepository;

        public Repository<FootballMatch> FootballMatchRepository => _footballMatchRepository ?? (_footballMatchRepository = new Repository<FootballMatch>(_dbContext));
        //public Repository<Component> ComponentRepository => _componentRepository ?? (_componentRepository = new Repository<Component>(_dbContext));
        //public Repository<Filling> FillingRepository => _fillingRepository ?? (_fillingRepository = new Repository<Filling>(_dbContext));
        //public Repository<Inhibitor> InhibitorRepository => _inhibitorRepository ?? (_inhibitorRepository = new Repository<Inhibitor>(_dbContext));
        //public Repository<IntermediateGrille> IntermediateGrilleRepository => _intermediateGrilleRepository ?? (_intermediateGrilleRepository = new Repository<IntermediateGrille>(_dbContext));
        //public Repository<PressingScheme> PressingSchemeRepository => _pressingSchemeRepository ?? (_pressingSchemeRepository = new Repository<PressingScheme>(_dbContext));
        //public Repository<Product> ProductRepository => _productRepository ?? (_productRepository = new Repository<Product>(_dbContext));
        //public Repository<Site> SiteRepository => _siteRepository ?? (_siteRepository = new Repository<Site>(_dbContext));
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