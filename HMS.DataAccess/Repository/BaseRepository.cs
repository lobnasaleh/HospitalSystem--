using HMS.DataAccess.Data;
using HMS.Entites;
using HMS.Entites.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataAccess.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly HospitalContext Context;

        public BaseRepository(HospitalContext Context)
        {
            this.Context = Context;
        }

        public async Task<T> AddAsync(T entity)
        {
          await Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
           
        }

        public async Task<IEnumerable<T>> getAllAsync(Expression<Func<T, bool>>? condition = null, string[] include = null, Expression<Func<T, object>>? orderBy = null, string orderByDirection = null, int pagenumber = 1, int pagesize = 0)
        {
           IQueryable<T>query=Context.Set<T>();
            if (condition != null) { 
            query=query.Where(condition);
            }
            if (include != null) {
                foreach (var incl in include)
                {
                    query=query.Include(incl);
                }
            }
            if (orderBy != null)
            {

                if (orderByDirection == Order.Descending)
                {
                    query.OrderByDescending(orderBy);
                }
                else if (orderByDirection == Order.Ascending)
                {
                    query.OrderBy(orderBy);
                }
            }
            if (pagesize > 0)
            {
                if (pagesize > 100)
                {
                    pagesize = 100;
                }
                query = query.Skip(pagesize * (pagenumber - 1)).Take(pagesize);
            }

            return await query.ToListAsync();
        }

        public async Task<T> getAsync(Expression<Func<T, bool>>? condition, bool tracking = true, string[] include = null)
        {
            IQueryable<T> query = Context.Set<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            if (include != null)
            {
                foreach (var incl in include)
                {
                    query = query.Include(incl);
                }
            }
            if (!tracking)
            {
                query=query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public void Update(T newentity)
        {
           Context.Set<T>().Update(newentity);
        }
    }
}
