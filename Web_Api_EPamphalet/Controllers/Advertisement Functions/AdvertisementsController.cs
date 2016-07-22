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

namespace Web_Api_EPamphalet.Controllers.Advertisement_Functions
{
    public class AdvertisementsController : ApiController
    {
        private Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
        [HttpPost]
        [ResponseType(typeof(Advertisement))]
        [Route("Advertisements/GetAdvertisement")]
        public IHttpActionResult GetAdvertisement(Fence ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {//fence se mene vendor ki id lene ha
                var query = (from advertisement in db.Advertisements
                             where advertisement.FKvendor_id == ad.FKvendor_id
                             select advertisement);
                if (query != null)
                    return Ok(query);
                else
                    return NotFound();
            }
        }

        [HttpPost]
        [ResponseType(typeof(Advertisement))]
        [Route("Advertisements/GetVendorId")]
        public IHttpActionResult GetVendorId(Advertisement advert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            { //Advertisement id se vendor id nikaalne ha
                var query = (from advertisement in db.Advertisements
                             where advertisement.Advert_id == advert.Advert_id
                             select advertisement.FKvendor_id).FirstOrDefault();
                if (query != null)
                    return Ok(query);
                else
                    return NotFound();
            }
        }
    }
}