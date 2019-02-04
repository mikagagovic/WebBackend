using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class CountryRepository : Repository<Country, int>, ICountryRepository 
    {
        public CountryRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Country> GetAll(int pageIndex, int pageSize)
        {
            return DemoContext.Countries.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext DemoContext { get { return context as RADBContext; } }
    }
}