using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
   public interface ICountryRepository:IRepository<Country, int>
    {
       
            IEnumerable<Country> GetAll(int pageIndex, int pageSize); 
        
    }
}
