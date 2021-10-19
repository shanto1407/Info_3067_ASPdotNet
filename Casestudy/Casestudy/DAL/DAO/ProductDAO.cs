using Casestudy.DAL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.DAL.DAO
{
    public class ProductDAO
    {
        private AppDbContext _db;
        public ProductDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(item => item.Brand.Id == id).ToList();
        }

    }
}
