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

namespace Web_Api_EPamphalet.Controllers.Fence_Functions
{
    public class FencesController : ApiController
    {
        private Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
       
              
        // POST: api/Fences
       [HttpPost]
       
        [Route("Fences/AddFence")]
        [ResponseType(typeof(Fence))]
        public IHttpActionResult AddFence(Fence fence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Fences.Add(fence);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                
            }

            return CreatedAtRoute("DefaultApi", new { id = fence.Fence_id }, fence);
        }
         
       [HttpPost]
        [Route("Fences/GetFence")]
        [ResponseType(typeof(Fence))]
       public IHttpActionResult GetFence(Vendor v)
       {
           var query = from fence in db.Fences
                       where fence.FKvendor_id == v.Vendor_id
                       select new
              {
                  Fence_id=fence.Fence_id,
                  Fence_name = fence.Fence_name,
                  Fence_longitude = fence.Fence_longitude,
                  Fence_latitude = fence.Fence_latitude,
                  Fence_radius = fence.Fence_radius
              };
           if (query != null)
           {
               return Ok(query);
           }
           else
               return NotFound();


       }



       [HttpPost]
       [Route("Fences/DeleteFence")]
       [ResponseType(typeof(Fence))]
       public IHttpActionResult DeleteFence(Fence f)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }

           try
           {
               var query = (from fence in db.Fences
                            where fence.Fence_id == f.Fence_id
                            select fence).SingleOrDefault();
               db.Fences.Remove(query);
               db.SaveChanges();
               return Ok();
           }
           catch (DbUpdateException)
           {

               return NotFound();
           }

       }

       [HttpPost]
       [Route("Fences/GetVendor")]
       [ResponseType(typeof(Fence))]
       public IHttpActionResult GetVendor(Fence f)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }
           else
           {
               var query = FenceControllerFunctions.Authenticate(f.Fence_id);
               if (query != 0)
               {
                   return Ok(query);
               }
               else
               {
                   return NotFound();

               }




           }
       }


      [HttpPost]
        [Route("Fences/UpdateFence")]
        [ResponseType(typeof(Fence))]
       public IHttpActionResult UpdateFence(Fence f)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }     

      try
      
      {
          var query = (from fence in db.Fences
                       where fence.Fence_id == f.Fence_id
                       select fence).SingleOrDefault();
          query.Fence_radius = f.Fence_radius;
          db.SaveChanges();
          return Ok();
      }
          catch(DbUpdateException)
      {

          return NotFound();
      }
      
      }

     
    }
}