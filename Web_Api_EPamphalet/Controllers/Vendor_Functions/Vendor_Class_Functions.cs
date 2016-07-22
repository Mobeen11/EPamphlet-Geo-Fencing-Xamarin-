using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Api_EPamphalet.Models;

namespace Web_Api_EPamphalet.Controllers.Vendor_Functions
{
    public class Vendor_Class_Functions
    {
        private static Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();

        public static int Search(string vendorname)
        {
            var query = (from vendor in db.Vendors
                         where vendor.Vendor_name == vendorname
                         select vendor.Vendor_id).FirstOrDefault();
            if(query!=0)
            {
                return query;


            }
            else 
            {
                return 0;
            }
        
        }

                  public static int Authenticate(string username, string password)
        {
            var query = (from vendor in db.Vendors
                         where vendor.Vendor_name == username && vendor.Vendor_password == password
                         select vendor.Vendor_id).FirstOrDefault();
            if (query != 0)
                return query;
            else
                return 0;
        }
        
      
      public static int GetVendorid(string vendorname)
        {
         
          var query = (from vendor in db.Vendors
                         where vendor.Vendor_name == vendorname
                         select vendor.Vendor_id
                         ).FirstOrDefault();

       if (query != 0)
                return int.Parse(query.ToString());
            else
                return 0;

      }

      //public static string GetVendorAddress(int id)
      //{

      //    var query = (from vendor in db.Vendors
      //                 where vendor.Vendor_id == id
      //                 select vendor.Vendor_address).FirstOrDefault();
      //    if (query != null)
      //        return (query);
      //    else
      //        return "error";

      //}

    }
}