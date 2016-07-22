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
using Android.Gms.Maps;
using Android.Graphics;
using Android.Gms.Maps.Model;
using TabSample.Model_Classes;
using System.Net;
using System.IO;
using Android.Locations;
using Newtonsoft.Json;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Show_Pamphalet : Activity, IOnMapReadyCallback
    {
        GoogleMap map;
        string text;
        string vendor_address;
        protected async override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            text = Intent.GetStringExtra("Notification Id") ?? "Data not available";
            string customerid = Intent.GetStringExtra("Customer_Id") ?? "Data not available";

            Bitmap bit = null;
            Byte[] by;
            by = Intent.GetByteArrayExtra("Image");

            SetContentView(Resource.Layout.Pamphalet);

            string advert = Intent.GetStringExtra("Advertisement") ?? "Data not available";
            ImageView img = (ImageView)FindViewById(Resource.Id.imageView1);
            TextView t1 = (TextView)FindViewById(Resource.Id.pamph_text1);
            TextView t3 = (TextView)FindViewById(Resource.Id.pamph_text3);
            TextView t2 = (TextView)FindViewById(Resource.Id.pamph_text2);

            Button Save = (Button)FindViewById(Resource.Id.save);
            Button Getlocation = (Button)FindViewById(Resource.Id.getlocation);
            Save.Click += Save_Click;
            t2.Text = advert;
            Getlocation.Click += Getlocation_Click;
            // Create your application here
            int ref_no;
            try
            {
                bit = BitmapFactory.DecodeByteArray(by, 0, by.Length);


                img.SetImageBitmap(bit);
                Random rand1 = new Random();
                ref_no = rand1.Next(10000000);
                t3.Text = "Coupon Number:" + ref_no.ToString();
                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Customers/GenerateReference";
                DateTime now = DateTime.Now.ToLocalTime();
                Reference a = new Reference { Reference_no = (decimal)ref_no, SalesCheck = "0", FKadvert_id = int.Parse(text), FKcustomer_id = int.Parse(customerid), Date_time = now };
                string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                byte[] postDataByteArray = System.Text.Encoding.UTF8.GetBytes(postDataString);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                request.ContentLength = postDataByteArray.Length;
                request.KeepAlive = true;
                Stream dataStream = await request.GetRequestStreamAsync();
                dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                dataStream.Close();
                Toast.MakeText(this, "Reference Generated", ToastLength.Long).Show();

            }
            catch (Exception ex)
            {


            }
        }
        void Getlocation_Click(object sender, EventArgs e)
        {
            View v;
            v = LayoutInflater.Inflate(Resource.Layout.GetLocation, null);

            new AlertDialog.Builder(this).SetView(v).SetTitle("Google Map")
                      .Show();
            if (map == null)
            {
                MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.getlocationmap);
                mapFragment.GetMapAsync(this);


            }

        }

        void Save_Click(object sender, EventArgs e)
        {
            var screenshotPath =
                Android.OS.Environment.GetExternalStoragePublicDirectory("Pictures/Screenshots").AbsolutePath + Java.IO.File.Separator + "screenshot1.png";
            var rootView = ((Android.App.Activity)this).Window.DecorView.RootView;
            using (var screenshot = Android.Graphics.Bitmap.CreateBitmap(
               rootView.Width,
               rootView.Height,
                Android.Graphics.Bitmap.Config.Argb8888))
            {
                var canvas = new Android.Graphics.Canvas(screenshot);
                rootView.Draw(canvas);

                using (var screenshotOutputStream = new System.IO.FileStream(
                            screenshotPath,
                            System.IO.FileMode.Create))
                {
                    screenshot.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 90, screenshotOutputStream);
                    screenshotOutputStream.Flush();
                    screenshotOutputStream.Close();
                }
            }
            Toast.MakeText(this, "Screen Shot Saved", ToastLength.Short).Show();

        }

        public async void OnMapReady(GoogleMap googleMap)
        {
            ProgressDialog p1 = new ProgressDialog(this);
      
            map = googleMap;
            var geo = new Geocoder(this);
            IList<Address> addresses;

            double longitude;
            double lattitude;
            p1.SetProgressStyle(ProgressDialogStyle.Spinner);
            p1.SetCancelable(false);
            p1.SetMessage("Loading  Address...");
            p1.Show();
               
            if (map != null)
            {
                //Advertisement id se vendor id nikaalne ha

                map.MyLocationEnabled = true;



                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Advertisements/GetVendorId";
                try
                {
                    Advertisement a = new Advertisement { Advert_id = int.Parse(text) };
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
                    int vendor_id = int.Parse(content);
                    //vendor id se mene address nikalna ha
                 //  vendor_getadress(vendor_id);



                   
                   string url2 = url1 + "/Vendors/GetVendorAddress";

                   Vendor ab = new Vendor { Vendor_id = vendor_id };
                   string postDataString1 = Newtonsoft.Json.JsonConvert.SerializeObject(ab);
                   byte[] postDataByteArray1 = Encoding.UTF8.GetBytes(postDataString1);
                   HttpWebRequest request1 = (HttpWebRequest)HttpWebRequest.Create(new Uri(url2));
                   request1.ContentType = "application/json";
                   request1.Method = "POST";

                   request1.ContentLength = postDataByteArray1.Length;
                   request1.KeepAlive = true;

                   Stream dataStream1 = await request1.GetRequestStreamAsync();
                   dataStream1.Write(postDataByteArray1, 0, postDataByteArray1.Length);
                   dataStream1.Close();
                   HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();


                   StreamReader r2 = new StreamReader(response1.GetResponseStream());
                   var content1 = r2.ReadToEnd();
                       vendor_address = content1;
                       addresses = await geo.GetFromLocationNameAsync(vendor_address, 1);
                       map.MyLocationEnabled = true;
                       if (addresses.Count > 0)
                       {
                           foreach (var item1 in addresses)
                           {
                               longitude = item1.Longitude;
                               lattitude = item1.Latitude;
                               LatLng location = new LatLng(lattitude, longitude);
                               MarkerOptions m1 = new MarkerOptions().SetPosition(location).SetTitle("Vendors Location");
                               map.AddMarker(m1);
                               CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(location, 12);

                               map.MoveCamera(camera);
                           }


                       }


                   
    

                    p1.Hide();
                }
                catch (Exception ex)
                {
                    p1.Hide();
                }




                p1.Hide();
            }
        }

        private async void vendor_getadress(int vendor_id)
        {
                }
    }
}