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
using Newtonsoft.Json;
using TabSample.Model_Classes;
using Android.Gms.Common.Apis;
using Android.Util;
using Android.Gms.Location;
using System.Security;
using System.Threading.Tasks;
using System.Collections;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Customer_main : Activity, IResultCallback, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
   {
        ListView listView;
        List<String> lItems;
        IList abc= new List<String>();
        List<String> vendor_name = new List<string>();
        int customer_id;
        int vendor_id;
        protected const string TAG = "creating-and-monitoring-geofences";
        PendingIntent mGeofencePendingIntent;
        protected IList<IGeofence> mGeofenceList;
        public GoogleApiClient mGoogleApiClient;
        IList<String> test_list;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string test = Intent.GetStringExtra("CustomerID") ?? "Data not available";
          
            customer_id = int.Parse(test);
            SetContentView(Resource.Layout.Customer_Choice);
           listView = FindViewById<ListView>(Resource.Id.customers_choice);
           mGeofenceList = new List<IGeofence>();
           mGeofencePendingIntent = null;
            Button b1 = (Button)FindViewById(Resource.Id.choose_vendor);
            Button b2 = (Button)FindViewById(Resource.Id.signoutcustomer);
            b2.Click += b2_Click;
           b1.Click += b1_Click;
           BuildGoogleApiClient();

            SetupList();
                    
        }

        void b2_Click(object sender, EventArgs e)
        {

            ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove("CustomerID");

            editor.Commit();
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        void b1_Click(object sender, EventArgs e)
        {
      
            var builder = new StringBuilder();
            var sparsearray = FindViewById<ListView>(Resource.Id.customers_choice).CheckedItemPositions;
            for (var i = 0; i < sparsearray.Size(); i++)
            {

                string a = listView.GetItemAtPosition(sparsearray.KeyAt(i)).ToString();
                vendor_name.Add(a);
            }

      
            new Android.App.AlertDialog.Builder(this).SetView(LayoutInflater.Inflate(Resource.Layout.Customer_Dialog, null)).SetTitle("Register Vendors").SetPositiveButton("Ok",fence_add).Show();
         
        }

       

        private async void fence_add(object sender, DialogClickEventArgs e)
        {
            ProgressDialog a1 = new ProgressDialog(this);
            a1.SetMessage("Adding Vendor..");
            a1.Show();

            foreach (var item in vendor_name)
            {
                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Vendors/VendorId";
                Vendor a = new Vendor { Vendor_name = item };
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
                vendor_id = Convert.ToInt32(content);
                Toast.MakeText(this, vendor_id.ToString(), ToastLength.Long).Show();
                addchoices(customer_id, vendor_id);
                
            await    populatefence(vendor_id);
                
            }
            vendor_name.Clear();
            addfence();
            a1.Hide();
        }

        private async void addfence()
        {
            if (!mGoogleApiClient.IsConnected)
            {
                Toast.MakeText(this, "Not Connected", ToastLength.Short).Show();
                return;
            }

            try
            {
                var status = await LocationServices.GeofencingApi.AddGeofencesAsync(mGoogleApiClient, GetGeofencingRequest(),
                    GetGeofencePendingIntent());
            }
            catch (SecurityException securityException)
            {
                //LogSecurityException(securityException);
            }
        }

        private async Task populatefence(int vendor_id)
        {

            try
            {
                string url = "http://epamhaletwebservice.azurewebsites.net/api/Fences/GetFence";
              
                Vendor a = new Vendor { Vendor_id = vendor_id };
                string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                byte[] postDataByteArray = System.Text.Encoding.UTF8.GetBytes(postDataString);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                request.ContentLength = postDataByteArray.Length;
                request.KeepAlive = true;
                System.IO.Stream dataStream = await request.GetRequestStreamAsync();
                dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader r1 = new StreamReader(response.GetResponseStream());
                var content = r1.ReadToEnd();

                var _Data = JsonConvert.DeserializeObject<List<Fence>>(content);
                foreach (Fence f in _Data)
                {

                    double l = Convert.ToDouble(f.Fence_longitude);
                    double lat = Convert.ToDouble(f.Fence_latitude);
                    double radius = Convert.ToDouble(f.Fence_radius);
                    mGeofenceList.Add(new GeofenceBuilder()
                    .SetRequestId(f.Fence_id.ToString())
                    .SetCircularRegion(lat, l, Convert.ToSingle(radius))
                    .SetExpirationDuration(Geofence.NeverExpire)
                     .SetTransitionTypes(Geofence.GeofenceTransitionEnter | Geofence.GeofenceTransitionExit)
                    .Build());
                    

                }
                Toast.MakeText(this, "Fences are Added", ToastLength.Long).Show();



            }
            catch (System.Exception ex)
            { }
        }

        private async void addchoices(int customer_id, int vendor_id)
        {
            try
            {
                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Choices/Add";
                Choice c = new Choice { FKCustomer_id = customer_id, FKVendor_id = vendor_id };
                string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(c);
                byte[] postDataByteArray = Encoding.UTF8.GetBytes(postDataString);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                request.ContentLength = postDataByteArray.Length;
                request.KeepAlive = true;

                Stream dataStream = await request.GetRequestStreamAsync();
                dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                dataStream.Close();
                Toast.MakeText(this, "Choices Added", ToastLength.Long).Show();
            }
            catch(Exception ex)
            { }
        }



        private async void SetupList()
        {

            ProgressDialog a1 = new ProgressDialog(this);
            a1.SetMessage("Loading Please wait..");
            a1.Show();
            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Choices/Get";
            try
            {
                Customer a = new Customer { Customer_id = customer_id };
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

                var _Data = JsonConvert.DeserializeObject<List<Choice>>(content);
                //yahan pe sare vendor ID's aye hain
                //ab is se mene vendor name lene hain
                if (_Data.Count == 0)
                {

                    string url6 = url1 + "/Vendors/GetVendor";
                    try
                    {
                        HttpWebRequest request4 = (HttpWebRequest)HttpWebRequest.Create(new Uri(url6));

                        request4.ContentType = "application/json";
                        request4.Method = "GET";
                        HttpWebResponse response4 = (HttpWebResponse)request4.GetResponse();

                        StreamReader r4 = new StreamReader(response4.GetResponseStream());
                        var content4 = r4.ReadToEnd();

                        var _Data4 = JsonConvert.DeserializeObject<List<Vendor>>(content4);
                        lItems = new List<string>();

                        foreach (var item4 in _Data4)
                        {
                            lItems.Add(item4.Vendor_name);
                        }
                        listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemMultipleChoice, lItems);

                        listView.ChoiceMode = ChoiceMode.Multiple;
                      
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    foreach (var item in _Data)
                    {
                        // GetVendorName(int.Parse(item.FKVendor_id.ToString()));
                        //  lItems.Add(item.Vendor_name);
                        string url2 = GetString(Resource.String.url);
                        string url3 = url2 + "/Vendors/VendorName";
                        try
                        {
                            Vendor ab = new Vendor { Vendor_id = int.Parse(item.FKVendor_id.ToString()) };
                            string postDataString1 = Newtonsoft.Json.JsonConvert.SerializeObject(ab);
                            byte[] postDataByteArray1 = Encoding.UTF8.GetBytes(postDataString1);
                            HttpWebRequest request1 = (HttpWebRequest)HttpWebRequest.Create(new Uri(url3));
                            request1.ContentType = "application/json";
                            request1.Method = "POST";

                            request1.ContentLength = postDataByteArray1.Length;
                            request1.KeepAlive = true;

                            Stream dataStream1 = await request1.GetRequestStreamAsync();
                            dataStream1.Write(postDataByteArray1, 0, postDataByteArray1.Length);
                            dataStream1.Close();
                            HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();


                            StreamReader r11 = new StreamReader(response1.GetResponseStream());
                            var content1 = r11.ReadToEnd();
                            var _Data1 = JsonConvert.DeserializeObject<List<Vendor>>(content1);

                            foreach (Vendor v in _Data1)
                            {
                                //      lItems.Add(v.Vendor_name);

                                string url5 = url1 + "/Vendors/GetVendor";
                                try
                                {
                                    HttpWebRequest request3 = (HttpWebRequest)HttpWebRequest.Create(new Uri(url5));

                                    request3.ContentType = "application/json";
                                    request3.Method = "GET";
                                    HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();

                                    StreamReader r3 = new StreamReader(response3.GetResponseStream());
                                    var content3 = r3.ReadToEnd();

                                    var _Data3 = JsonConvert.DeserializeObject<List<Vendor>>(content3);
                                    lItems = new List<string>();

                                    foreach (var item3 in _Data3)
                                    {
                                        if (v.Vendor_name == item3.Vendor_name)
                                        {

                                        }
                                        else
                                        {
                                            lItems.Add(item3.Vendor_name);

                                        }
                                    }
                                    listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemMultipleChoice, lItems);

                                    listView.ChoiceMode = ChoiceMode.Multiple;


                                }
                                catch (Exception ex)
                                {
                                    a1.Hide();
                                }




                            }


                        }
                        catch (Exception ex)
                        {

                            a1.Hide();
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                a1.Hide();
            }
            a1.Hide();
        
                }

            
        
        GeofencingRequest GetGeofencingRequest()
        {
            var builder = new GeofencingRequest.Builder();
            builder.SetInitialTrigger(GeofencingRequest.InitialTriggerEnter);
            builder.AddGeofences(mGeofenceList);

            return builder.Build();
        }
        PendingIntent GetGeofencePendingIntent()
        {
            if (mGeofencePendingIntent != null)
            {
                return mGeofencePendingIntent;
            }
            var intent = new Intent(this, typeof(GeofenceTransitionsIntentService));
           intent.PutExtra("customerid", customer_id.ToString());
            return PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);
        }
        protected void BuildGoogleApiClient()
        {
            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(LocationServices.API)
                .Build();
        }
        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }
        protected override void OnResume()
        {
            base.OnResume();
            mGoogleApiClient.Connect();


        }

        public void OnResult(Java.Lang.Object result)
        {
            throw new NotImplementedException();
        }

        public void OnConnected(Bundle connectionHint)
        {
            Log.Info(TAG, "Connected to GoogleApiClient");
        }

        public void OnConnectionSuspended(int cause)
        {
            Log.Info(TAG, "Connection suspended");

        }

        public void OnConnectionFailed(Android.Gms.Common.ConnectionResult result)
        {
            Log.Info(TAG, "Connection failed: ConnectionResult.getErrorCode() = " + result.ErrorCode);

        }

    }
}