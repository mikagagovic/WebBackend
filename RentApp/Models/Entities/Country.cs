using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Country
    {
        public Country()
        {
             this.Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public int CallNumber { get; set; }
        public string Registration { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}