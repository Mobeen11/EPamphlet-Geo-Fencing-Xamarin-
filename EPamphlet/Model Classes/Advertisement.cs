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
    public partial class Advertisement
    {
        public int Advert_id { get; set; }
        public string Advert_text { get; set; }
        public string Advert_image { get; set; }
        public Nullable<int> FKvendor_id { get; set; }

    }
}