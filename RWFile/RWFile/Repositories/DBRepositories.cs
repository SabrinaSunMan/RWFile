using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RWFile.Repositories
{
    public class DBRepositories<T> : IDBRepositories<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }

        private DbSet<T> _Objectset;
        private DbSet<T> ObjectSet
        {
            get
            {
                if (_Objectset == null)
                {
                    _Objectset = UnitOfWork.Context.Set<T>();
                }
                return _Objectset;
            }
        }
        public DBRepositories(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Create(T entity)
        {
            ObjectSet.Add(entity);
        }

        public void Delete(T entity)
        {
            ObjectSet.Remove(entity);
        }

        public IQueryable<T> GetAll()
        { 
            return ObjectSet;
        }

        public T GetSingle(Expression<Func<T, bool>> filter)
        {
            return ObjectSet.SingleOrDefault(filter);
        }

        public void Commit()
        {
            UnitOfWork.Save();
        }
    }
}
