using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class Repository<Tipo> : IRepository<Tipo> where Tipo : class
    {

        protected AppDbContext _context;

        public Repository(AppDbContext contexto)
        {
            _context = contexto;
        }

        public void Add(Tipo entity)
        {
            _context.Set<Tipo>().Add(entity);
        }

        

        public IQueryable<Tipo> Get()
        {
            return _context.Set<Tipo>().AsNoTracking();
        }

        public Tipo GetById(Expression<Func<Tipo, bool>> predicate)
        {
            return _context.Set<Tipo>().SingleOrDefault(predicate);
        }

        public void update(Tipo entity)
        {
            _context.Set<Tipo>().Update(entity);
        }

        public void Delete(Tipo entity)
        {
            throw new NotImplementedException();
        }
    }
}
