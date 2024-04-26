using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entities;
using Store.Repository.Interface;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repository
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey> 
    {
        private readonly StoreDBContext _context;

        public GenericRepository(StoreDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)=>
       await _context.Set<TEntity>().AddAsync(entity) ;

        public async Task<int> CountSpecificationAsyn(ISpecification<TEntity> specs)
                => await ApplySpecification(specs).CountAsync();
        public  void Delete(TEntity entity)
       =>  _context.Set<TEntity>().Remove(entity);


        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
       => await _context.Set<TEntity>().ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationsAsync(ISpecification<TEntity> spec)
            => await ApplySpecification(spec).ToListAsync();
   

        public async Task<TEntity> GetByIdAsync(Tkey? id)
         => await _context.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> GetWithSpecificationsByIdAsync(ISpecification<TEntity> spec)
       => await ApplySpecification(spec).FirstOrDefaultAsync();


        public void Update(TEntity entity)
       => _context.Set<TEntity>().Update(entity);

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specs)
             => SpecificationEvaluater<TEntity, Tkey>.GetQuery(_context.Set<TEntity>(), specs );
    }
}
