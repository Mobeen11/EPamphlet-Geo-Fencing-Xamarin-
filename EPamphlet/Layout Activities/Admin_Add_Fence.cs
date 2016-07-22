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
using Android.Locations;
using System.IO;
using System.Net;
using TabSample.Model_Classes;
using Android.Gms.Maps.Model;
using Android.Graphics;
using System.Threading;
using System.Threading.Tasks;
using TabSample.Google_Map_Classes;
using Newtonsoft.Json;
using Android.Views.InputMethods;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Admin_Add_Fence : Activity, IOnMapReadyCallback
    {
        const string strAutoCompleteGoogleApi = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";

        const string strGoogleApiKey = "AIzaSyDR_VMfI9GnEmLTt5bQNLIvL_vSTCTZePA";
        const string strGeoCodingUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        AutoCompleteTextView txtSearch;
        MapFragment mapFrag;
        ArrayAdapter adapter = null;
        GoogleMapPlaceClass objMapClass;
        GeoCodeJSONClass objGeoCodeJSONClass;
        string autoCompleteOptions;
        string[] strPredictiveText;
        int index = 0;
 
	    
        GoogleMap map;
        double longitude;
        double lattitude; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Admin_Add_Fence);
          
            SetupMap();
            Button b1 = (Button)FindViewById(Resource.Id.Search);
            b1.Click += b1_Click;
            txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch);
            txtSearch.Hint = "Enter Address";
            txtSearch.ItemClick += txtSearch_ItemClick;
            txtSearch.TextChanged += async delegate(object sender, Android.Text.TextChangedEventArgs e)
            {
                ProgressDialog p1 = new ProgressDialog(this);
                p1.SetMessage("Loading..");
                p1.SetProgressStyle(ProgressDialogStyle.Spinner);
                p1.Show();
                try
                {
                    test_data(strAutoCompleteGoogleApi + txtSearch.Text + "&key=" + strGoogleApiKey);
                    Console.WriteLine(objMapClass.status);

                    strPredictiveText = new string[objMapClass.predictions.Count];
                    index = 0;
                    foreach (Prediction objPred in objMapClass.predictions)
                    {
                        strPredictiveText[index] = objPred.description;
                        index++;
                    }
                    p1.Hide();
                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveText);
                    txtSearch.Adapter = adapter;
                }
                catch
                {
                    p1.Hide();
                    Toast.MakeText(this, "Unable to process at this moment!!!", ToastLength.Short).Show();
                }

            };  
		
        }

      async  void txtSearch_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(txtSearch.WindowToken, HideSoftInputFlags.NotAlways);
            map.Clear();
            if (txtSearch.Text != string.Empty)
            {
                var sb = new StringBuilder();
                sb.Append(strGeoCodingUrl);
                sb.Append("?address=").Append(txtSearch.Text);
                test_data2(sb.ToString());
                LatLng Position = new LatLng(objGeoCodeJSONClass.results[0].geometry.location.lat, objGeoCodeJSONClass.results[0].geometry.location.lng);
                updateCameraPosition(Position);
                MarkOnMap("MyLocation", Position);
            }
       
        }

      private void test_data2(string p)
      {
          string url6 = p;
          try
          {

              HttpWebRequest request4 = (HttpWebRequest)HttpWebRequest.Create(new Uri(url6));

              request4.ContentType = "application/json";
              request4.Method = "GET";
              HttpWebResponse response4 = (HttpWebResponse)request4.GetResponse();

              StreamReader r4 = new StreamReader(response4.GetResponseStream());
              var content4 = r4.ReadToEnd();
              objGeoCodeJSONClass = JsonConvert.DeserializeObject<GeoCodeJSONClass>(content4);

          }
          catch (Exception ex)
          { }

      }

       void MarkOnMap(string p, LatLng Position)
      {

          RunOnUiThread(() =>
          {
              var marker = new MarkerOptions();
              marker.SetTitle(p);
              marker.SetPosition(Position);
              map.AddMarker(marker);
          });
      }

      private void updateCameraPosition(LatLng pos)
      {
          CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
          builder.Target(pos);
          builder.Zoom(14);
          builder.Bearing(45);
          builder.Tilt(90);
          CameraPosition cameraPosition = builder.Build();
          CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
          map.AnimateCamera(cameraUpdate);
      }


        private void test_data(string p)
        {
            string url6 = p;
            try
            {

                HttpWebRequest request4 = (HttpWebRequest)HttpWebRequest.Create(new Uri(url6));

                request4.ContentType = "application/json";
                request4.Method = "GET";
                HttpWebResponse response4 = (HttpWebResponse)request4.GetResponse();

                StreamReader r4 = new StreamReader(response4.GetResponseStream());
                var content4 = r4.ReadToEnd();
                objMapClass = JsonConvert.DeserializeObject<GoogleMapPlaceClass>(content4);

            }
            catch (Exception ex)
            { }

        }

        
       public async void b1_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(txtSearch.WindowToken, HideSoftInputFlags.NotAlways);
           
            var geo = new Geocoder(this);
            ProgressDialog p1 = new ProgressDialog(this);
      
            try
            {
      
                p1.SetProgressStyle(ProgressDialogStyle.Spinner);
                p1.SetCancelable(false);
                p1.SetMessage("Loading  Address...");
                p1.Show();
              //  EditText t1 = (EditText)FindViewById(Resource.Id.textview1);
                AutoCompleteTextView t1 =(AutoCompleteTextView) FindViewById(Resource.Id.txtTextSearch);
                IList<Address> addresses;

                double longitude;
                double lattitude;

                addresses = await geo.GetFromLocationNameAsync(t1.Text, 1);
                map.MyLocationEnabled = true;
              
                if (addresses.Count > 0)
                {
                    foreach (var item in addresses)
                    {
                        longitude = item.Longitude;
                        lattitude = item.Latitude;
                        LatLng location = new LatLng(lattitude, longitude);
                    //    MarkerOptions m1 = new MarkerOptions().SetPosition(location).SetTitle("Test");
                    //    map.AddMarker(m1);
                    //    CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(location, 15);
                        MarkOnMap("Marker", location);
                        updateCameraPosition(location);
                    //    map.MoveCamera(camera);
                    }


                }
                else
                {
                    Toast.MakeText(this, "Please Enter valid address", ToastLength.Long).Show();
                }
                p1.Hide();
            }
            catch (Exception ex)
            {
                p1.Hide();
                Toast.MakeText(this, ex.Message, ToastLength.Long);
                
            }
        }

        private void SetupMap()
        {
            if (map == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            if (map != null)
            {
               // map.MyLocationEnabled = true;
                map.MyLocationEnabled = true;
                LatLng l1 = new LatLng(33.7182, 73.0605);
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(l1, 12);

                map.MoveCamera(camera);
            
                map.MapLongClick += map_MapLongClick;

            }

        }

        private void map_MapLongClick(object sender, GoogleMap.MapLongClickEventArgs e)
        {
            var geo = new Geocoder(this);
            longitude = e.Point.Longitude;
            lattitude = e.Point.Latitude;
            new Android.App.AlertDialog.Builder(this).SetView(LayoutInflater.Inflate(Resource.Layout.Add_Fence_Dialog, null)).SetTitle("Add New Fence")
               .SetPositiveButton("Ok", fence_add
               ).Show();
        
        }
      
        private async void fence_add(object sender, DialogClickEventArgs e)
        {
                     
      
            int fence_id;
           // LayoutInflater layoutInflater = LayoutInflater.From(this);
            var dialog = (Android.App.AlertDialog)sender;
            dialog.Cancel();
          //  AlertDialog.Builder b1 = new AlertDialog.Builder(this);
          //  LayoutInflater layoutInflater = LayoutInflater.From(this);
          //  View b = layoutInflater.Inflate(Resource.Layout.test, null);

          //  b1.SetView(b);
          //  // b1.Show();
          //AlertDialog ad = b1.Create();
          //ad.Show();
           var vendorname = (EditText)dialog.FindViewById(Resource.Id.vendorname);
            var fencename = (EditText)dialog.FindViewById(Resource.Id.fencename);
            var radius = (EditText)dialog.FindViewById(Resource.Id.fenceradius);
            ProgressDialog a1 = new ProgressDialog(this);

            a1.SetMessage("Making Fence");
            a1.Show();
            if ((vendorname.Text != String.Empty) && (fencename.Text != String.Empty) && (radius.Text != String.Empty))
            {
                try
                {
                    string url1 = GetString(Resource.String.url);
                    string url = url1 + "/Vendors/SearchVendor";
                    Vendor a = new Vendor { Vendor_name = vendorname.Text };
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
                    Toast.MakeText(this, content, ToastLength.Long).Show();
                    fence_id = int.Parse(content);
                    string fence_name;
                    double fence_radius;
                    fence_radius = double.Parse(radius.Text);
                    fence_name = fencename.Text;

                    await addfence(fence_id, fence_name, fence_radius);
                    a1.Hide();
                }
                catch (Exception ex)
                {
                    a1.Hide();
                    Toast.MakeText(this, "Vendor Not Found", ToastLength.Long).Show();
                }
            }
            else
            {
                a1.Hide();

                Toast.MakeText(this, "Please Enter valid information", ToastLength.Long).Show();
            }
        }

        public async System.Threading.Tasks.Task addfence(int vendor_id, string fence_name, double fence_radius)
        {
            
             
            try
            {

               
           
                //   Marker stopmarker = map.AddMarker(new MarkerOptions().Draggable(false).SetPosition(new LatLng(lattitude, longitude)).SetTitle(fence_name).InfoWindowAnchor(10, 10));
                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Fences/AddFence";
                Fence a = new Fence { Fence_name = fence_name, Fence_latitude = (decimal)lattitude, Fence_longitude = (decimal)longitude, Fence_radius = fence_radius, FKvendor_id = vendor_id };
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
                map.AddCircle(new CircleOptions().InvokeCenter(new LatLng(lattitude, longitude)).InvokeRadius(fence_radius).InvokeStrokeColor(Color.GreenYellow).InvokeFillColor(Color.Gray));
            //    a1.Dismiss();
              
            }
            catch (Exception ex)
            {
          //      a1.Dismiss();
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();

            }
        }
       
    }
}
   