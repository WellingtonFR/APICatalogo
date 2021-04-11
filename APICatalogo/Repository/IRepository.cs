using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public interface IRepository<Tipo>
    {
        IQueryable<Tipo> Get();
        Tipo GetById(Expression<Func<Tipo, bool>> predicate);
        void Add(Tipo entity);
        void update(Tipo entity);
        void Delete(Tipo entity);
    }
}
