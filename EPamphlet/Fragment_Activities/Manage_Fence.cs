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
using TabSample.Model_Classes;
using System.Net;
using System.IO;

namespace TabSample.Fragment_Activities
{
    public class Manage_Fence : Android.Support.V4.App.Fragment 
    {
        View v;

     
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            v = inflater.Inflate(Resource.Layout.admin_manage_fence, container, false);
            
            Button add_fence = (Button)v.FindViewById(Resource.Id.Add_Fence);
            Button edit_fence = (Button)v.FindViewById(Resource.Id.Edit_Fence);
            Button delete_fence = (Button)v.FindViewById(Resource.Id.Delete_Fence);
            add_fence.Click += add_fence_Click;
            Button signout = (Button)v.FindViewById(Resource.Id.adminsignout);
            signout.Click += signout_Click;
            edit_fence.Click += (sender, ea) => {
             
                AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
                View v1 = inflater.Inflate(Resource.Layout.Edit_Fence_Dialog, container, false);
                new Android.App.AlertDialog.Builder(this.Activity).SetView(inflater.Inflate(Resource.Layout.Search_Vendor_Dialog, null)).SetTitle("Search Vendor")
                   .SetPositiveButton("Search", search_vendor
                   ).Show();
            };
            delete_fence.Click += (sender, ea) => {
                AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
                View v1 = inflater.Inflate(Resource.Layout.Edit_Fence_Dialog, container, false);
                new Android.App.AlertDialog.Builder(this.Activity).SetView(inflater.Inflate(Resource.Layout.Search_Vendor_Dialog, null)).SetTitle("Search Vendor")
                   .SetPositiveButton("Search", delete_search_vendor
                   ).Show();
            };
          
            return v;
        }

        void signout_Click(object sender, EventArgs e)
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove("Admin ID");
           
            editor.Commit();
            Intent i = new Intent(this.Activity, typeof(MainActivity));
            StartActivity(i);
        }

        private async void delete_search_vendor(object sender, DialogClickEventArgs e)
        {
            var dialog = (Android.App.AlertDialog)sender;
            dialog.Cancel();
            ProgressDialog a1 = new ProgressDialog(this.Activity);
            a1.SetMessage("Searching Vendor");
            a1.Show();

            var vendorname = (EditText)dialog.FindViewById(Resource.Id.vendor_name);
            await delete_search_vendor_true(vendorname.Text);
            a1.Hide();                
        
        }

        private async System.Threading.Tasks.Task delete_search_vendor_true(string vendorname)
        {

            try
            {


                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Vendors/SearchVendor";
                Vendor a = new Vendor { Vendor_name = vendorname };
                string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                byte[] postDataByteArray = Encoding.UTF8.GetBytes(postDataString);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                request.ContentLength = postDataByteArray.Length;
                request.KeepAlive = true;

                Stream dataStream = await request.GetRequestStreamAsync();
                dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader r1 = new StreamReader(response.GetResponseStream());
                var content = r1.ReadToEnd();
                Toast.MakeText(this.Activity, content, ToastLength.Long).Show();
                int vendorid = Convert.ToInt32(content);
                if (vendorid != 0)
                {
                    var activity2 = new Intent(this.Activity, typeof(Admin_Delete_Fence));
                    activity2.PutExtra("Vendor_Id", Convert.ToString(vendorid));
                    StartActivity(activity2);

                }
                else
                {

                    Toast.MakeText(this.Activity, "No Vendor Found please Try again", ToastLength.Long).Show();
                    var activity2 = new Intent(this.Activity, typeof(Manage_Fence));
                    StartActivity(activity2);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Activity, ex.Message, ToastLength.Long).Show();
            }
     
        }

        private async void search_vendor(object sender, DialogClickEventArgs e)
        {
            
            var dialog = (Android.App.AlertDialog)sender;
            dialog.Cancel();
            ProgressDialog a1 = new ProgressDialog(this.Activity);
            a1.SetMessage("Searching Vendor");
            a1.Show();

            var vendorname = (EditText)dialog.FindViewById(Resource.Id.vendor_name);
            await search_vendor_true(vendorname.Text);
            a1.Hide();                
        }

        private async System.Threading.Tasks.Task search_vendor_true(string vendorname)
        {
            try
            {

               
                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Vendors/SearchVendor";
                Vendor a = new Vendor { Vendor_name = vendorname };
                string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                byte[] postDataByteArray = Encoding.UTF8.GetBytes(postDataString);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                request.ContentLength = postDataByteArray.Length;
                request.KeepAlive = true;

                Stream dataStream = await request.GetRequestStreamAsync();
                dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader r1 = new StreamReader(response.GetResponseStream());
                var content = r1.ReadToEnd();
                Toast.MakeText(this.Activity, content, ToastLength.Long).Show();
                int vendorid = Convert.ToInt32(content);
                if (vendorid != 0)
                {
                    var activity2 = new Intent(this.Activity, typeof(Admin_Edit_Fence_True));
                    activity2.PutExtra("Vendor_Id", Convert.ToString(vendorid));
                    StartActivity(activity2);
               
                }
                else
                {

                    Toast.MakeText(this.Activity, "No Vendor Found please Try again", ToastLength.Long).Show();
                    var activity2 = new Intent(this.Activity, typeof(Manage_Fence));
                     StartActivity(activity2);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Activity, ex.Message, ToastLength.Long).Show();
                  }
        }

       



      
        void add_fence_Click(object sender, EventArgs e)
        {
            
            Intent intent = new Intent(this.Activity, typeof(Admin_Add_Fence));
            StartActivity(intent);
        
        }
    }
}