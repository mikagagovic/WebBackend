using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
	[RoutePrefix("comment")]
	public class CommentController : ApiController
	{
		private RADBContext db = new RADBContext();
		public const string ServerUrl = "http://localhost:51680";
		[HttpGet]
		[Route("comments", Name = "CommentApi")]
		public IHttpActionResult GetComments()
		{
			var l = db.Comments.ToList();
			return Ok(l);
		}
		[HttpGet]
		[Route("comment/{id}")]
		[ResponseType(typeof(Comment))]
		public IHttpActionResult GetComment(int id)
		{
			Branch branch = db.Branches.Find(id);
			if (branch == null)
			{
				return NotFound();
			}

			return Ok(branch);
		}
		[HttpGet]
		[Route("commentsForServiceId/{id}")]
		public IHttpActionResult GetCommentsForServiceId(int id)
		{
			var l = db.Comments.Where(x => x.Service_Id == id);
			return Ok(l);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut]
		[Route("comment/{id}")]
		[ResponseType(typeof(void))]
		public IHttpActionResult PutComments(int id, Comment comment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != comment.Id)
			{
				return BadRequest();
			}

			db.Entry(comment).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CommentExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return StatusCode(HttpStatusCode.NoContent);
		}
		[HttpPost]
		[Route("comment")]
		[ResponseType(typeof(Comment))]
		public IHttpActionResult PostComment(Comment comment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			db.Comments.Add(comment);
			db.SaveChanges();

			return CreatedAtRoute("BranchApi", new { id = comment.Id }, comment);
		}

		// DELETE: api/Branches/5
		[Authorize(Roles = "Admin")]
		[HttpDelete]
		[Route("comment/{id}")]
		[ResponseType(typeof(Branch))]
		public IHttpActionResult DeletComment(int id)
		{
			Comment comment = db.Comments.Find(id);
			if (comment == null)
			{
				return NotFound();
			}
			db.Comments.Remove(comment);
			db.SaveChanges();

			return Ok(comment);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool CommentExists(int id)
		{
			return db.Comments.Count(e => e.Id == id) > 0;
		}
	}
}