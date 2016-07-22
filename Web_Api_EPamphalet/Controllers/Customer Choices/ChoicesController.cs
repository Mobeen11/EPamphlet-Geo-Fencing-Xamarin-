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

namespace Web_Api_EPamphalet.Controllers.Customer_Choices
{
    public class ChoicesController : ApiController
    {
        private Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();

        //choice ka function bnaana ha aur data insert krwadena ha
        [HttpPost]
        [ResponseType(typeof(Choice))]
        [Route("Choices/Add")]
        public IHttpActionResult Add(Choice c)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                db.Choices.Add(c);

                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = c.Choices_Id }, c);
            }
        }



        [HttpPost]
        [ResponseType(typeof(Choice))]
        [Route("Choices/Get")]
        public IHttpActionResult Get(Customer c)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = from choices in db.Choices
                        where choices.FKCustomer_id == c.Customer_id
                        select new
                        {

                            FKVendor_id = choices.FKVendor_id

                        };
            if (query != null)
                return Ok(query);
            else
                return NotFound();

        }
        [HttpPost]
        [ResponseType(typeof(Choice))]
        [Route("Choices/Remove")]
        public IHttpActionResult Remove(Vendor v)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var query = (from choice in db.Choices
                             where choice.FKVendor_id == v.Vendor_id
                             select choice).FirstOrDefault();
                db.Choices.Remove(query);
                db.SaveChanges();
                return Ok();

            }
        }
    }
}