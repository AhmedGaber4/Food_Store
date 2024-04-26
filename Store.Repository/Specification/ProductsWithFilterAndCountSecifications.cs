using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public class ProductsWithFilterAndCountSecifications : BaseSpecification<Product>
    {
        public ProductsWithFilterAndCountSecifications(ProductSpecification specs) :
            base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) &&
                             (!specs.TypeId.HasValue || product.TypeId == specs.TypeId.Value) &&
                             (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search))
            )
        {
        }
    }
}
