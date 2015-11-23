using Access.Extensions;
using Access.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Access.Repositories
{
    public class FeedsRepository : BaseRepository<AccessContext, Feed>
    {



        private IQueryable<Feed> CommonSearch(FilterOptionModel filter, Guid? user)
        {
            IQueryable<Feed> query = GetSummary(user).Include(b => b.Category).Include(f => f.User);

            filter.SearchKeys.ForEach(k => query =
            query.Where(q => q.Phone.ToLower().Contains(k)
                           || q.Title.ToLower().Contains(k)
                           || q.Content.ToLower().Contains(k)
                           || q.Title.ToLower().Contains(k)
                           ));

            if (!string.IsNullOrEmpty(filter.CategoryName)) query = query.Where(q => q.Category.Name.ToLower().Contains(filter.CategoryName.Trim().ToLower()));

            if (filter.date.HasValue) query = query.Where(q => q.DateStart >= filter.date.Value);

            if (filter.end.HasValue) query = query.Where(q => q.DateEnd <= filter.end.Value);

            if (filter.FeedStatus.HasValue) query = query.Where(b => b.Status == filter.FeedStatus.Value);


            return filter.HasOrderByProperty ? query.CustomOrderby(filter) : query.OrderByDescending(o => o.DateStart);
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return Context.Categories.ToListAsync();
        }

        public IQueryable<Feed> GetSummary(Guid? userid = null)
        {

            // add access visibility  level
            IQueryable<Feed> query = Context.Feeds.Include(f => f.Category);


            ////filter by access level
            var centerId = ClaimsPrincipal.Current.CenterId();

            if (centerId.HasValue)
            {
                query = query.Where(q => q.User.CenterId == centerId);
            }



            return query;
        }

        public async Task<Center> GetCompanyAsync()
        {
            var centerId = ClaimsPrincipal.Current.CenterId();

            return centerId.HasValue ? (await Context.Centers.FindAsync(centerId.Value)) : null;
        }

        public Task<List<Feed>> GetSummaryAsync(FilterOptionModel filter)
        {
            return CommonSearch(filter, UserId)
                .OrderByDescending(o => o.LastUpdate).ThenBy(o => o.CategoryId)
                .Skip(filter.Skip).Take(filter.Limit).ToListAsync();
        }

        //public dynamic MessageForStatus(FeedStatus status)
        //{
        //    switch (status)
        //    {
        //        case FeedStatus.Pending: return "Do you Want set To pending?";
        //        case FeedStatus.Active: return "Do you want Activate ?";
        //        case FeedStatus.Inactive: return "Do you want Deactivate?";
        //        default: return string.Empty;
        //    }
        //}

        public async Task<int> GetPageLimit(FilterOptionModel filter)
        {
            return (await CommonSearch(filter, UserId).CountAsync()) / filter.Limit + 1;
        }

        public Task GetUserSummaryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void InsertOrUpdate(Feed entity)
        {
            if (entity.Id == 0) entity.IdPublisher = UserId.ToString();
            base.InsertOrUpdate(entity);
        }


    }
}
