using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Api_EPamphalet.Models;

namespace Web_Api_EPamphalet.Controllers.Admin_Functions
{
    public class Admin_Functions_class
    {
        private static Epamphalet_dbEntities4 db = new Epamphalet_dbEntities4();
        

        public static int Authenticate(string username, string password)
        {


            var query = (from admin in db.Admins
                         where admin.Admin_username == username && admin.Admin_password == password
                         select admin.Admin_id).FirstOrDefault();
            if (query != 0)
                return query;
            else
                return 0;

        }
    }
}