using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BasicSearchApp.Entities;

namespace BasicSearchApp.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T GetByID(long id);

        IEnumerable<T> GetAll();


        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}