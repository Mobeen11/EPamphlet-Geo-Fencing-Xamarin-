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
using System.Net;
using System.IO;
using TabSample.Model_Classes;
using Newtonsoft.Json;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Admin_Delete_Vendor : Activity
    {
        ListView listView;
        List<String> lItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Admin_Delete_Vendor);
            listView = FindViewById<ListView>(Resource.Id.listView1);
            Button donebutton = FindViewById<Button>(Resource.Id.delete_vendor);
            donebutton.Click += donebutton_Click;
            SetupList();
           
            // Create your application here
        }

       public async void donebutton_Click(object sender, EventArgs e)
        {
            ProgressDialog p1 = new ProgressDialog(this);
            p1.SetMessage("Deleting Vendor...");
            p1.Show();
          
           var sparsearray = FindViewById<ListView>(Resource.Id.listView1).CheckedItemPositions;
            for (var i = 0; i < sparsearray.Size(); i++)
            {

                string a = listView.GetItemAtPosition(sparsearray.KeyAt(i)).ToString();
                try
                {
                    string url1 = GetString(Resource.String.url);
                    string url = url1 + "/Vendors/DeleteVendor";

                    Vendor v = new Vendor { Vendor_name = a };
                    string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(v);
                    byte[] postDataByteArray = System.Text.Encoding.UTF8.GetBytes(postDataString);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                    request.ContentType = "application/json";
                    request.Method = "POST";

                    request.ContentLength = postDataByteArray.Length;
                    request.KeepAlive = true;
                    System.IO.Stream dataStream = await request.GetRequestStreamAsync();
                    dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                    dataStream.Close();
                    
                    Toast.MakeText(this, "Vendor Deleted",ToastLength.Short).Show();
                    p1.Hide();
                    SetupList();
                }
                catch (Exception ex)
                {
                    p1.Hide();
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();

                }
            }
          
        }

        private void SetupList()
        {
            ProgressDialog p1 = new ProgressDialog(this);
            p1.SetMessage("Loading Vendors...");
            p1.Show();
            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Vendors/GetVendor";
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));

                request.ContentType = "application/json";
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader r1 = new StreamReader(response.GetResponseStream());
                var content = r1.ReadToEnd();

                var _Data = JsonConvert.DeserializeObject<List<Vendor>>(content);

                lItems = new List<string>();

                foreach (var item in _Data)
                {

                    lItems.Add(item.Vendor_name);

                }
                listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemMultipleChoice, lItems);
                
                listView.ChoiceMode = ChoiceMode.Multiple;
                p1.Hide();
            }
            catch (Exception ex)
            {
                p1.Hide();
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            
            }
        }
    }
}