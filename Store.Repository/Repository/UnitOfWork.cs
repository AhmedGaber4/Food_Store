using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entities;
using Store.Repository.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDBContext _context;
        private Hashtable _repositores;
        public UnitOfWork(StoreDBContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
      => await _context.SaveChangesAsync();

        public IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            if (_repositores is null) 
            _repositores = new Hashtable();

            var entitykey= typeof(TEntity).Name;
            if (!_repositores.ContainsKey(entitykey))
            {
                var repositoryType= typeof(GenericRepository<,>);
                var repositorylnstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(Tkey)), _context);

                
                _repositores.Add(entitykey, repositorylnstance);


            }

            return (IGenericRepository<TEntity, Tkey>)_repositores[entitykey];


        }
    }
}
