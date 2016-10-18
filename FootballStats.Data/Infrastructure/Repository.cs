using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using FootballStats.Common.Extensions;
using FootballStats.Domain;

namespace FootballStats.Data.Infrastructure
{
    public class Repository<TEntity> where TEntity : EntityBase
    {
        protected FootballStatsDbContext DbContext;
        protected DbSet<TEntity> DbSet;

        protected ObjectContext ObjectContext => DbContext.GetObjectContext();

        public Repository(FootballStatsDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public virtual void Save(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.IsNew())
                Add(entity);
            else
                Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            DbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            entities.ToList().ForEach(Delete);
        }

        public virtual void Delete(int id)
        {
            var found = GetById(id);
            if (found == null)
                throw new ArgumentException($"Entity '{id}' doesnt't exist.");

            Delete(found);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            foreach (var entity in GetMany(where))
                Delete(entity);
        }

        public void SqlBulkDelete(Expression<Func<TEntity, bool>> where)
        {
            if (DbSet.Any(where))
                DbSet.Where(where).Delete();
        }

        public virtual void DeleteFrom(Expression<Func<TEntity, object>> navigationPropertySelector, TEntity parent, IEntity child)
        {
            ObjectContext.ObjectStateManager.ChangeRelationshipState(parent, child, navigationPropertySelector, EntityState.Deleted);
        }

        public virtual void AddTo(Expression<Func<TEntity, object>> navigationPropertySelector, TEntity parent, IEntity child)
        {
            ObjectContext.ObjectStateManager.ChangeRelationshipState(parent, child, navigationPropertySelector, EntityState.Added);
        }

        public virtual TEntity GetById(int id, bool throwExceptionIfNotFound = true)
        {
            var found = DbSet.Find(id);

            if (found == null && throwExceptionIfNotFound)
                throw new InvalidOperationException($"Failed to find entity of type '{typeof(TEntity)}' by id '{id}'.");

            return found;
        }

        public virtual TResult GetById<TResult>(int id, Expression<Func<TEntity, TResult>> selector)
        {
            var result = GetMany(e => e.Id == id, selector).Take(1).ToArray();
            if (!result.Any())
                throw new InvalidOperationException($"Failed to find entity of type '{typeof(TEntity)}' by id '{id}'.");

            return result.First();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual IQueryable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Select(selector);
        }

        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().Where(where);
        }

        public virtual IQueryable<TResult> GetMany<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Where(where).Select(selector);
        }

        public virtual TEntity FirstOrDefault()
        {
            return GetAll().FirstOrDefault();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().FirstOrDefault(where);
        }

        public virtual TEntity First()
        {
            return GetAll().First();
        }

        public TEntity First(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().First(where);
        }

        public TEntity SingleOrDefault()
        {
            return GetAll().SingleOrDefault();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().SingleOrDefault(where);
        }

        public TEntity Single()
        {
            return GetAll().Single();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().Single(where);
        }

        public TResult FirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Select(selector).FirstOrDefault();
        }

        public virtual TResult FirstOrDefault<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetMany(where).Select(selector).FirstOrDefault();
        }

        public TResult First<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Select(selector).First();
        }

        public virtual TResult First<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetMany(where).Select(selector).First();
        }

        public TResult SingleOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Select(selector).SingleOrDefault();
        }

        public TResult SingleOrDefault<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetMany(where).Select(selector).SingleOrDefault();
        }

        public TResult Single<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Select(selector).Single();
        }

        public TResult Single<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetMany(where).Select(selector).Single();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().Any(where);
        }

        public virtual bool Any()
        {
            return GetAll().Any();
        }

        public virtual bool Exist(int id)
        {
            return Any(e => e.Id == id);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> where = null)
        {
            return where == null ? GetAll().Count() : GetAll().Count(where);
        }

        public virtual IQueryable<TEntity> AsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<TElement>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        protected virtual void Add(TEntity entity)
        {
            if (entity.CreatedAt.IsEmpty())
                entity.CreatedAt = DateTime.UtcNow;

            DbSet.Add(entity);
        }

        protected virtual void Update(TEntity entity)
        {
            var entityState = DbContext.Entry(entity).State;

            if (entityState == EntityState.Detached)
                throw new InvalidOperationException("Can't update detached entity.");

            if (entityState == EntityState.Modified)
                entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
