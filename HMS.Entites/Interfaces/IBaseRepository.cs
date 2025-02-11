using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        //add
        public Task <T> AddAsync (T entity);
        //update
        public void Update(T newentity);
        //delete
        public void Delete(T entity);
        //getAllByCondition
        public Task<IEnumerable<T>> getAllAsync(Expression<Func<T, bool>>? condition = null,
           string[] include = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = Order.Ascending,
            int pagenumber = 1, int pagesize = 0);

        //getbycondition
        public Task<T> getAsync(Expression<Func<T, bool>>? condition,
            bool tracking = true,
            string[] include = null
            );
  
    }
}
