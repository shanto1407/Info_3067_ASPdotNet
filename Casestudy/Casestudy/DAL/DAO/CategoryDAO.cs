using Casestudy.DAL.DomainClasses;
using System.Collections.Generic;
using System.Linq;
namespace Casestudy.DAL.DAO
{
    public class BrandDAO
    {
        private AppDbContext _db;
        public BrandDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Brand> GetAll()
        {
            return _db.Brands.ToList<Brand>();
        }
    }
}