using Casestudy.DAL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.DAL.DAO
{
    public class CustomerDAO
    {
        private AppDbContext _db;
        public CustomerDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        public Customer Register(Customer customer)
        {
            _db.Customers.Add(customer);
            _db.SaveChanges();
            return customer;
        }

        public Customer GetByEmail(string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(u => u.Email == email);
            return customer;
        }
    }
}
