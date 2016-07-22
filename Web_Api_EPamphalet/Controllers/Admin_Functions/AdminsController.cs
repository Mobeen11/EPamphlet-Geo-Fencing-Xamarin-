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

namespace Web_Api_EPamphalet.Controllers.Admin_Functions
{
    public class AdminsController : ApiController
    {
        private Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
        [HttpPost]
        [ResponseType(typeof(Admin))]
        [Route("Admins/Authenticate")]
        public IHttpActionResult Authenticate(Admin ad)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                var employees = Admin_Functions_class.Authenticate(ad.Admin_username, ad.Admin_password);
                if (employees == 0)
                { return NotFound(); }
                else
                    return Ok(employees);
               



            }
        }



    }
}