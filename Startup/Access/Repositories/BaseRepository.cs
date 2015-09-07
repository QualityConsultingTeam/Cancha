using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Security.Claims;

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

        private static string[] PotentialIdClaim = { System.Security.Claims.ClaimTypes.NameIdentifier, "sub" };


        protected Guid UserId
        {
            get
            {
                var userIdClaim = ClaimsPrincipal.Current.FindFirst(c => PotentialIdClaim.Contains(c.Type));

                if (userIdClaim == null)   throw new Exception("Authentication Failed, verify identity  Settings");

                return new Guid(userIdClaim.Value);
            }
        }

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

        public virtual void InsertOrupdateModel<Tmodel>(Tmodel model)where Tmodel :BaseModel
        {
            if (model.Id != 0) Context.Entry(model).State = EntityState.Modified;

            else Context.Set<Tmodel>().Add(model);
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
 
        
        public async Task SaveAsync(Guid? currentUserId = null)
        {
            try
            {
                if (currentUserId == null)
                  currentUserId = ClaimsPrincipal.Current.FindFirst(c => PotentialIdClaim.Contains(c.Type)) !=null? (Guid?) UserId:null;

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
            catch(Exception ex)
            {

            }


           
        }


        #endregion
    }
}
