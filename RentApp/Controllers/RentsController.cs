using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;

namespace RentApp.Controllers
{
    [RoutePrefix("rent")]
    public class RentsController : ApiController
    {
        public const string ServerUrl = "http://localhost:51680";
        private RADBContext db = new RADBContext();
        public static object lockObj = new object();
        // GET: api/Rents
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("rents", Name = "RentApi")]
        public IHttpActionResult GetRents()
        {
            var l = db.Rents.ToList();
            return Ok(l);
        }

        // GET: api/Rents/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("rent/{id}")]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult GetRent(int id)
        {
            Rent rent = db.Rents.Find(id);
            if (rent == null)
            {
                return NotFound();
            }
            return Ok(rent);
        }

        // PUT: api/Rents/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("rent/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRent(int id, Rent rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rent.Id)
            {
                return BadRequest();
            }

            db.Entry(rent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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
        // PUT: api/Rents/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("rentAprrove/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRentApprove(int id,Rent rent)
        {
			lock (lockObj)
			{
				Rent rentForChange = db.Rents.Find(id);
				if (rent != null)
				{
					rentForChange.Approved = rent.Approved;
				}
				else
				{
					return StatusCode(HttpStatusCode.BadRequest);
				}
				db.Entry(rentForChange).State = EntityState.Modified;
			}
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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
        // POST: api/Rents
        [Authorize]
        [HttpPost]
        [Route("rent")]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult PostRent(Rent rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (validateReservation(rent))
            {
                lock (lockObj)
                {
                    db.Rents.Add(rent);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (RentExists(rent.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {
                var rents = this.db.Rents.Where(x => x.Vehicle_Id == rent.Vehicle_Id && x.Approved == true).ToList();

                string msg = "Vehicle is busy at: ";

                foreach (var rentPom in rents)
                {
                    msg += "  [" + rentPom.StartDate.Value.ToShortDateString() + "  -  " + rentPom.EndDate.Value.ToShortDateString() + "]";
                }
                return Content(HttpStatusCode.BadRequest, msg);
            }
           
            return CreatedAtRoute("RentApi", new { id = rent.Id }, rent);
        }

        // DELETE: api/Rents/5
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult DeleteRent(int id)
        {
            Rent rent = db.Rents.Find(id);
            if (rent == null)
            {
                return NotFound();
            }

            db.Rents.Remove(rent);
            db.SaveChanges();

            return Ok(rent);
        }

        [HttpGet]
        [Route("rent/image/{id}")]
        public string GetImage(int id)
        {
            Rent rent = this.db.Rents.FirstOrDefault(x => x.Id == id);
            if (rent.Image == null)
            {
                return null;
            }

            var filePath = rent.Image;
            var fullFilePath = HttpContext.Current.Server.MapPath("~/Content/Logos/" + Path.GetFileName(filePath));
            var relativePath = ServerUrl + "/Content/Logos/" + Path.GetFileName(filePath);

            if (File.Exists(fullFilePath))
            {
                return relativePath;

            }
            return null;
        }

        public bool validateReservation(Rent rent)
        {
            if (rent.StartDate > rent.EndDate)
                return false;
            List<Rent> rentList = db.Rents
                .Where(rentPom => rentPom.Vehicle_Id == rent.Vehicle_Id && rentPom.Approved == true)
                .ToList();

            foreach (var rentinL in rentList)
            {
                if (Intersects(rentinL, rent))
                    return false;
            }

            return true;
        }


        public bool Intersects(Rent rent1, Rent rent2)
        {
            if (rent1.StartDate > rent1.EndDate || rent2.StartDate > rent2.EndDate)
                return false;

            if (rent1.StartDate == rent1.EndDate || rent2.StartDate == rent2.EndDate)
                return false; // No actual date range

            if (rent1.StartDate == rent2.StartDate || rent1.EndDate == rent2.EndDate)
                return true; // If any set is the same time, then by default there must be some overlap. 

            if (rent1.StartDate < rent2.StartDate)
            {
                if (rent1.EndDate > rent2.StartDate && rent1.EndDate < rent2.EndDate)
                    return true; // Condition 1

                if (rent1.EndDate > rent2.EndDate)
                    return true; // Condition 3
            }
            else
            {
                if (rent2.EndDate > rent1.StartDate && rent2.EndDate < rent1.EndDate)
                    return true; // Condition 2

                if (rent2.EndDate > rent1.EndDate)
                    return true; // Condition 4
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RentExists(int id)
        {
            return db.Rents.Count(e => e.Id == id) > 0;
        }
    }
}