using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access
{
    public class BaseRepository<TContext, TEntity>
        where TContext : DbContext
        where TEntity : BaseModel
       //where TEntity : class 
    {

        //[Dependency]
        public TContext Context;
        public BaseRepository(TContext db)
        {
            this.Context = db;

        }

        protected BaseRepository()
        {

        }

        #region IRepository<T> Members



        public virtual IQueryable<TEntity> AsQueryable { get { return Context.Set<TEntity>(); } }

        public virtual IQueryable<TEntity> AsIQueryable(params string[] children)
        {

            if (children == null || children.Length == 0)
            {
                return Context.Set<TEntity>();
            }

            IQueryable<TEntity> query = children.Aggregate<string, IQueryable<TEntity>>(Context.Set<TEntity>(), (current, child) => current.Include(child));
            return query;
        }



        public virtual Task<TEntity> FindByIdAsync<T>(T id)// where TEntity : BaseModel
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public virtual Task<TEntity> FindByIdAsync(int id, params string[] children)
        {
            if (children == null || children.Length == 0)
            {
                return Context.Set<TEntity>().FindAsync(id);
            }

            IQueryable<TEntity> query = children.Aggregate<string, IQueryable<TEntity>>(Context.Set<TEntity>(), (current, child) => current.Include(child));
            return query.FirstOrDefaultAsync(e => e.Id == id);
        }
        public virtual async Task AddAsync(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            await Context.SaveChangesAsync();
        }


        public virtual void InsertOrUpdate(TEntity entity) //where TEntity : BaseModel
        {
            if (entity.Id != 0) Context.Entry(entity).State = EntityState.Modified;

            else Context.Set<TEntity>().Add(entity);
        }

        public virtual void InsertOrUpdate(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(InsertOrUpdate);
        }



        public virtual void Delete(TEntity entity) //where TEntity : BaseModel
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            items.ToList().ForEach(i => Context.Set<TEntity>().Remove(i));
            await SaveAsync();
        }

        
        public async Task SaveAsync(Guid? currentUserId =null)
        {
            try
            {
                if (currentUserId == null)
                {
                    await Context.SaveChangesAsync();
                    return;
                }


                var entities =
                    Context.ChangeTracker.Entries()
                        .Where(
                            x =>
                                x.Entity is TrackedEntity &&
                                (x.State == EntityState.Added || x.State == EntityState.Modified));



                foreach (var entity in entities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((TrackedEntity) entity.Entity).UserSign = currentUserId.Value;
                    }
                    ((TrackedEntity) entity.Entity).UserSign = currentUserId.Value;
                } 
                
                await Context.SaveChangesAsync();
            }
             
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }


           
        }


        #endregion
    }
}
