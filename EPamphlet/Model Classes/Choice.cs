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
    public partial class Choice
    {
        public int Choices_Id { get; set; }
        public Nullable<int> FKCustomer_id { get; set; }
        public Nullable<int> FKVendor_id { get; set; }
    }
}