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
    class Vendor
    {
        public int Vendor_id { get; set; }
        public string Vendor_name { get; set; }
        public string Vendor_password { get; set; }
        public string Vendor_phone { get; set; }
        public string Vendor_address { get; set; }
    }
}