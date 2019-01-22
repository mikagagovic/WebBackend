using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public int Grade { get; set; }
		public string Text { get; set; }
		[ForeignKey("Service")]
		public int Service_Id { get; set; }
		public virtual Service Service { get; set; }
		public virtual AppUser User { get; set; }
	}
}