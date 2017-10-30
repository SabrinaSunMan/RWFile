using System;
using System.Linq;
using System.Linq.Expressions;

namespace RWFile.Repositories
{
    public interface IDBRepositories<T> where T : class 
    {

        /// <summary>
        /// unit of work
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// GetAll.
        /// </summary>
        IQueryable<T> GetAll();

        /// <summary>
        /// 取得單一 entity
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        T GetSingle(Expression<Func<T, bool>> filter);
        /// <summary>
        /// save change
        /// </summary>
        void Commit(); 

    }
}
