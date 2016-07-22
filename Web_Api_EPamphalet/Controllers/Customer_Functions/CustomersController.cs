using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web_Api_EPamphalet.Models;

namespace Web_Api_EPamphalet.Controllers.Customer_Functions
{
    public class CustomersController : ApiController
    {
        private Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();

        [HttpPost]
        [ResponseType(typeof(Customer))]
        [Route("Customers/Authenticate")]
        public IHttpActionResult Authenticate(Customer cust)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                var employees = Customer_Functions_class.Authenticate(cust.Customer_username, cust.Customer_password);
                if (employees == 0)
                { return NotFound(); }
                else
                    return Ok(employees);




            }
        }

        [HttpPost]
        [ResponseType(typeof(Customer))]
        [Route("Customers/Register")]
        public IHttpActionResult Register(Customer cust)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                db.Customers.Add(cust);

                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = cust.Customer_id }, cust);
            }
        }
        [HttpPost]
        [ResponseType(typeof(Customer))]
        [Route("Customers/GenerateReference")]
        public IHttpActionResult GenerateReference(Reference reference)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                db.References.Add(reference);

                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = reference.Reference_no }, reference);
            }
        }
    
    }
}