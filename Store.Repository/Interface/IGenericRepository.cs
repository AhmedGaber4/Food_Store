using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Store.Data.Entities;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Interface
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<TEntity> GetByIdAsync(Tkey? id);

        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetWithSpecificationsByIdAsync(ISpecification<TEntity> spec);

        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationsAsync(ISpecification<TEntity> spec);

        Task<int> CountSpecificationAsyn(ISpecification<TEntity> specs);

        Task AddAsync(TEntity entity);
   
        void Update(TEntity entity);
    
        void Delete(TEntity entity);
    }
}
