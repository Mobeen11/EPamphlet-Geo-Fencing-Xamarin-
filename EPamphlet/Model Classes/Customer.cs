using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TabSample.Model_Classes
{
    public class Customer
    {
        public int Customer_id { get; set; }
        public string Customer_username { get; set; }
        public string Customer_name { get; set; }
        public string Customer_password { get; set; }
    }
}