using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Api_EPamphalet.Models;

namespace Web_Api_EPamphalet.Controllers.Customer_Functions
{
    public class Customer_Functions_class
    {
        private static Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
        

        public static int Authenticate(string username, string password)
        {



            var query = (from cust in db.Customers
                         where cust.Customer_username== username && cust.Customer_password == password
                         select cust.Customer_id).FirstOrDefault();
            if (query != 0)
                return query;
            else
                return 0;

        }
    }
}