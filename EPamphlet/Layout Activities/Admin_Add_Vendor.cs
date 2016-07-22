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
using TabSample.Model_Classes;
using System.Net;
using System.IO;
using Android.Graphics.Drawables;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Admin_Add_Vendor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Admin_Add_Vendor);
            Button add = (Button)FindViewById(Resource.Id.vendor_signup);
            add.Click += add_Click;

        }

       async void add_Click(object sender, EventArgs e)
        {
            await add_vendor(); 
        }

       private async System.Threading.Tasks.Task add_vendor()
       {
           Drawable errorIcon = Resources.GetDrawable(Resource.Drawable.images);
           ProgressDialog a1 = new ProgressDialog(this);

           a1.SetMessage("Adding Vendor");
           a1.Show();

           try

           {
               EditText vendor_name = (EditText)FindViewById(Resource.Id.vendor_name);
               EditText vendor_password = (EditText)FindViewById(Resource.Id.vendor_password);
               EditText vendor_address = (EditText)FindViewById(Resource.Id.vendor_address);
               EditText vendor_phone = (EditText)FindViewById(Resource.Id.vendor_phone);
               if (vendor_name.Text == String.Empty)
               {
                   a1.Hide();
                   vendor_name.SetError("please enter username", errorIcon);
               }
               else if (vendor_password.Text == String.Empty)
               {
                   a1.Hide();
                   vendor_password.SetError("please enter password", errorIcon);
        
               }
               else if (vendor_address.Text == String.Empty)
               {
                   a1.Hide();
                   vendor_address.SetError("please enter password", errorIcon);
        
               }
               else if (vendor_phone.Text == String.Empty)
               {
                   a1.Hide();
                   vendor_phone.SetError("please enter Telephone Number", errorIcon);
        
               }
               else
               {
                   string url1 = GetString(Resource.String.url);
                   string url = url1 + "/Vendors/AddVendor";
                   Vendor a = new Vendor { Vendor_name = vendor_name.Text, Vendor_password = vendor_password.Text, Vendor_address = vendor_address.Text, Vendor_phone = vendor_phone.Text };
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
                   a1.Hide();
                   Toast.MakeText(this, "Vendor Succesfully Added", ToastLength.Short).Show();
               }
           }
           catch (Exception ex)
           {
               a1.Hide();
               Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
           }
       }
    }
}