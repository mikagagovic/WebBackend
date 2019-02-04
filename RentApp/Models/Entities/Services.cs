using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Service
    {
        public Service()
        {
            this.Branches = new HashSet<Branch>();
            this.Vehicles = new HashSet<Vehicle>();
			this.Comments = new HashSet<Comment>();
		}
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }
        [ForeignKey("Country")]
        public int Country_Id { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}

