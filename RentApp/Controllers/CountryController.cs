using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    [RoutePrefix("country")]
    public class CountryController : ApiController
    {

        private RADBContext db = new RADBContext();
        public const string ServerUrl = "http://localhost:51680";
        public const int MaxImageSize = 1024 * 1024 * 6;

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public CountryController(DbContext context)
        {
            db = context as RADBContext;
        }

        [HttpGet]
        [Authorize]
        [Route("countries", Name = "CountryApi")]
        public IHttpActionResult GetCountries()
        {
            var l = db.Countries.ToList();
            return Ok(l);
        }

        [Authorize]
        [HttpGet]
        [Route("country/{id}")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult GeCountry(int id)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("country/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCountry(int id, Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != country.Id)
            {
                return BadRequest();
            }
            db.Entry(country).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountyExists(id))
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("country")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult PostCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Countries.Add(country);
            db.SaveChanges();

            return CreatedAtRoute("CountryApi", new { id = country.Id }, country);
        }

        // DELETE: api/Country/5
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("country/{id}")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteCountry(int id)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            int serviceNumber = country.Services.Count;
            if (serviceNumber > 0)
                return NotFound();

            db.Countries.Remove(country);
            db.SaveChanges();

            return Ok(country);
        }

        [HttpGet]
        [Route("country/flag/{id}")]
        public string GetImage(int id)
        {
            Country country = this.db.Countries.FirstOrDefault(x => x.Id == id);
            if (country.Flag == null)
            {
                return null;
            }
            var filePath = country.Flag;
            var fullFilePath = HttpContext.Current.Server.MapPath("~/Content/Logos/" + Path.GetFileName(filePath));
            var relativePath = ServerUrl + "/Content/Logos/" + Path.GetFileName(filePath);

            if (File.Exists(fullFilePath))
            {
                return relativePath;
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountyExists(int id)
        {
            return db.Countries.Count(e => e.Id == id) > 0;
        }

    }
}

