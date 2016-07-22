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
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Web_Api_EPamphalet.Models;

namespace Web_Api_EPamphalet.Controllers.Vendor_Functions
{
    public class VendorsController : ApiController
    {
        private Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
        
        [HttpPost]
        [ResponseType(typeof(Vendor))]
        [Route("Vendors/SearchVendor")]
        public IHttpActionResult SearchVendor(Vendor vendor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendorid = Vendor_Class_Functions.Search(vendor.Vendor_name);
            if (vendorid == 0)
            { return NotFound(); }
            else
                return Ok(vendorid);
               
        
        }

        [HttpPost]
        [ResponseType(typeof(Advertisement))]
        [Route("Vendors/AddAdvertisement")]
        public IHttpActionResult AddAdvertisement(Advertisement advert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Advertisements.Add(advert);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = advert.Advert_id }, advert);
        }

        [HttpPost]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
       
        [Route("Vendors/Authenticate")]
        public IHttpActionResult Authenticate(Vendor ad)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                //var vendor = Vendor_Class_Functions.Authenticate(ad.Vendor_name, ad.Vendor_password);

                
                    var query = (from ven in db.Vendors
                                 where ven.Vendor_name == ad.Vendor_name && ven.Vendor_password == ad.Vendor_password
                                 select new { 
                                 Vendor_Id=ven.Vendor_id
                                 }).FirstOrDefault();
                 
                    return Ok(query);

                }


            }
        
        [HttpPost]
        [Route("Vendors/DeleteAdvertisement")]
        [ResponseType(typeof(Advertisement))]
        public IHttpActionResult DeleteAdvertisement(Advertisement a)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var query = (from advert in db.Advertisements
                             where advert.FKvendor_id== a.FKvendor_id
                             select advert).SingleOrDefault();
            
                db.Advertisements.Remove(query);
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateException)
            {

                return NotFound();
            }

        }
        [HttpPost]
        [Route("Vendors/ViewAdvertisement")]
        [ResponseType(typeof(Advertisement))]
        public IHttpActionResult ViewAdvertisement(Advertisement a)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var query = (from advert in db.Advertisements
                             where advert.FKvendor_id == a.FKvendor_id
                             select new
                             {
                              Advert_id=   advert.Advert_id,
                              Advert_text=advert.Advert_text,
                          Advert_image=  advert.Advert_image
                             }
                             );

                     return Ok(query);
            }
            catch (DbUpdateException)
            {

                return NotFound();
            }

        }
        
        
        [Route("Vendors/ValidateReference")]
        [ResponseType(typeof(Advertisement))]
        public IHttpActionResult ValidateReference(Reference a)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var query = (from reference in db.References
                             where reference.Reference_no == a.Reference_no
                             select reference).SingleOrDefault();
                if (query != null)
                {
                    query.SalesCheck = "1";
                    db.SaveChanges();
               
                    return Ok("Reference Validated");
                
                
                }
                else
                    return NotFound();
            }
            catch (DbUpdateException)
            {

                return NotFound();
            }

        }

        [HttpPost]
        [ResponseType(typeof(Advertisement))]
        [Route("Vendors/EditAdvertisement")]
        public IHttpActionResult EditAdvertisement(Advertisement a)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var query = (from advert in db.Advertisements
                             where advert.Advert_id == a.Advert_id
                             select advert).SingleOrDefault();
                query.Advert_image = a.Advert_image;
                query.Advert_text = a.Advert_text;
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateException )
            {
                return NotFound();

            }
             }


        [HttpPost]
        [Route("Vendors/DeleteVendor")]
        [ResponseType(typeof(Vendor))]
        public IHttpActionResult DeleteVendor(Vendor a)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }   

            try
            {
                var query = (from advert in db.Vendors
                             where advert.Vendor_name == a.Vendor_name
                             select advert).SingleOrDefault();

                db.Vendors.Remove(query);
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateException)
            {

                return NotFound();
            }

        }
     
        [HttpPost]
        [ResponseType(typeof(Vendor))]
        [Route("Vendors/AddVendor")]
        public IHttpActionResult AddVendor(Vendor vendor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Vendors.Add(vendor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vendor.Vendor_id }, vendor);
        }
        [Route("Vendors/GetVendor")]
       public IHttpActionResult GetVendor()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var query = from v in db.Vendors
                        select new
                        {
                            v.Vendor_name

                        };
                        return Ok(query);

            
        }
        [HttpPost]
        [ResponseType(typeof(Vendor))]
        [Route("Vendors/VendorName")]
        public IHttpActionResult VendorName(Vendor vendor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = (from v in db.Vendors
                         where v.Vendor_id == vendor.Vendor_id
                         select new { Vendor_name=v.Vendor_name });
            return Ok(query);
        }

        [HttpPost]
        [ResponseType(typeof(Vendor))]
        [Route("Vendors/VendorId")]
        public IHttpActionResult VendorId(Vendor v)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int query = Vendor_Class_Functions.GetVendorid(v.Vendor_name);
            if (query != 0)
            {
                return Ok(query);
            }
            else
            {
                return NotFound();

            }

        }

        [HttpPost]
        [ResponseType(typeof(Vendor))]
        [Route("Vendors/GenerateReport")]
        public IHttpActionResult GenerateReport(Vendor v)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

           
            //if (query != 0)
            //{
            //    return Ok(query);
            //}
            //else
            //{
            //    return NotFound();

            //}
            return Ok();
        }  
        
        [HttpPost]
        [ResponseType(typeof(Advertisement))]
        [Route("Vendors/GetAdvertisement")]
        public IHttpActionResult GetAdvertisement(Vendor ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {//fence se mene vendor ki id lene ha
                var query = (from advertisement in db.Advertisements
                             where advertisement.FKvendor_id == ad.Vendor_id
                             select new { 
                                 Advert_id=advertisement.Advert_id,
                                Advert_image= advertisement.Advert_image,
                             Advert_Text=advertisement.Advert_text
                             });
                if (query != null)
                    return Ok(query);
                else
                    return NotFound();
            }
        }

       

        [HttpPost]
        [ResponseType(typeof(Advertisement))]
        [Route("Vendors/GetVendorAddress")]
        public IHttpActionResult GetVendorAddress(Vendor ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {   var query = (from vendor in db.Vendors
                             where vendor.Vendor_id == ad.Vendor_id
                             select new {
                             Vendor_address=    vendor.Vendor_address
                             }).FirstOrDefault();
            string address = query.Vendor_address;
                if (query != null)

                    return Ok(address);
                else
                    return NotFound();
            }
        }

    }
}