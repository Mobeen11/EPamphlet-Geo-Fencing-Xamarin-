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
    class Fence
    {
        public int Fence_id { get; set; }
        public string Fence_name { get; set; }
        public Nullable<decimal> Fence_longitude { get; set; }
        public Nullable<decimal> Fence_latitude { get; set; }
        public Nullable<int> FKvendor_id { get; set; }
        public Nullable<double> Fence_radius { get; set; }
    
    }
}