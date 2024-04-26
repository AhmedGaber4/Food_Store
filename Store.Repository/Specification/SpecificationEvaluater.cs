using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Store.Repository.Specification
{
    public class SpecificationEvaluater<TEntity, Tkey> where TEntity :BaseEntity<Tkey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> entities, ISpecification<TEntity> specs)
        {
            var query= entities;

            if (specs.Criteria is not null)
                query = query.Where(specs.Criteria);

            if (specs.OrderBy is not null)
                query = query.OrderBy(specs.OrderBy);

            if (specs.OrderByDescending is not null)
                query = query.OrderByDescending(specs.OrderByDescending); 

            if (specs.Ispaginated)
                query = query.Skip(specs.Skip).Take(specs.Take);
            query =specs.Includes.Aggregate(query,(current,includeEx)=>current.Include(includeEx));

            return query;
        }
    }
}
