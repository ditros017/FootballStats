using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Data.Infrastructure
{
    public static class DbContextExtensions
    {
        public static ObjectContext GetObjectContext(this DbContext self)
        {
            return ((IObjectContextAdapter) self).ObjectContext;
        }
    }
}