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
    public partial class Reference
    {
        public decimal Reference_no { get; set; }
        public string SalesCheck { get; set; }
        public Nullable<System.DateTime> Date_time { get; set; }
        public Nullable<int> FKadvert_id { get; set; }
        public Nullable<int> FKcustomer_id { get; set; }
    }
}