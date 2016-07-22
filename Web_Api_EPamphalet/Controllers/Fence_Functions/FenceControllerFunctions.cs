using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Api_EPamphalet.Models;

namespace Web_Api_EPamphalet.Controllers.Fence_Functions
{
    public class FenceControllerFunctions
    {
        private static Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
        public static int Authenticate(int fenceid)
        {


            var query = (from fence in db.Fences
                        where fence.Fence_id == fenceid
                        select fence.FKvendor_id).FirstOrDefault();
                           
                    
            if (query != 0)
                return int.Parse(query.ToString());
            else
                return 0;
        }
    }
}