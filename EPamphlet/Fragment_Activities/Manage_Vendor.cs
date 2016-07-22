using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TabSample.Layout_Activities;

namespace TabSample.Fragment_Activities
{
    public class Manage_Vendor : Android.Support.V4.App.Fragment
    {
        View v;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            v = inflater.Inflate(Resource.Layout.admin_manage_vendor, container, false);
            Button add_vendor = (Button)v.FindViewById(Resource.Id.Add_Vendor);
            Button delete_vendor = (Button)v.FindViewById(Resource.Id.Delete_Vendor);
            Button b1 = (Button)v.FindViewById(Resource.Id.admin_signout);
            b1.Click += b1_Click;
            add_vendor.Click += add_vendor_Click;
            delete_vendor.Click += delete_vendor_Click;
            return v;
        }

        void b1_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this.Activity, typeof(MainActivity));
            StartActivity(i);
        }

        void delete_vendor_Click(object sender, EventArgs e)
        {

            ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove("Admin ID");

            editor.Commit();
            Intent intent = new Intent(this.Activity, typeof(Admin_Delete_Vendor));
            StartActivity(intent);
        
        }

        void add_vendor_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(Admin_Add_Vendor));
            StartActivity(intent);
        
                   }
    }
}