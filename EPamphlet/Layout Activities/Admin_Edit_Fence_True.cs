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
using TabSample.Model_Classes;
using System.Net;
using Android.Locations;
using Android.Gms.Maps.Model;
using System.IO;
using Newtonsoft.Json;
using Android.Graphics;
using TabSample.Google_Map_Classes;
using Android.Views.InputMethods;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Admin_Edit_Fence_True : Activity, IOnMapReadyCallback
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
        int _fenceid;
        int _vendorid;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string id = Intent.GetStringExtra("Vendor_Id") ?? "0";
            _vendorid = int.Parse(id);
            SetContentView(Resource.Layout.Admin_Edit_Fence);
            SetupMap();
            Button b1 = (Button)FindViewById(Resource.Id.edit_Search);
            b1.Click += b1_Click;
            txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.edit_textview1);
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

        private void txtSearch_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
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

        private void MarkOnMap(string p, LatLng Position)
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

        private async void b1_Click(object sender, EventArgs e)
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
                AutoCompleteTextView t1 = (AutoCompleteTextView)FindViewById(Resource.Id.edit_textview1);
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

  

             private async System.Threading.Tasks.Task getfence() 

      {
          try
          {
              string url1 = GetString(Resource.String.url);
              string url = url1 + "/Fences/GetFence";

              Vendor a = new Vendor { Vendor_id = _vendorid };
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
              map.Clear(); 
              var _Data = JsonConvert.DeserializeObject<List<Fence>>(content);
              foreach (Fence f in _Data)
              {

                  double l = Convert.ToDouble(f.Fence_longitude);
                  double lat = Convert.ToDouble(f.Fence_latitude);
                  double radius = Convert.ToDouble(f.Fence_radius);
                  map.AddCircle(new CircleOptions().InvokeCenter(new LatLng(lat, l)).InvokeRadius(radius).InvokeStrokeColor(Color.GreenYellow).InvokeFillColor(Color.Gray));
                  string id = Convert.ToString(f.Fence_id);
                  Marker stopmarker = map.AddMarker(new MarkerOptions().Draggable(false)
                      .SetPosition(new LatLng(lat, l))
                      .SetTitle(f.Fence_name)
                      .SetSnippet(id
                      ).InfoWindowAnchor(10, 10));
                  stopmarker.ShowInfoWindow();
              }
          }
          catch (Exception ex)
          {
              Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
          }
      } 
        void SetupMap()
        {
            if (map == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.edit_map).GetMapAsync(this);


            }
        }

        public async void OnMapReady(GoogleMap googleMap)
        {
            ProgressDialog a1 = new ProgressDialog(this);

            a1.SetMessage("Loading Map");
            a1.Show();


            map = googleMap;
            if (map != null)
            {
                map.MyLocationEnabled = true;
                LatLng l1 = new LatLng(33.7182, 73.0605);
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(l1, 12);

                map.MoveCamera(camera);
            
                map.MarkerClick += map_MarkerClick;
                a1.Hide();
                await getfence();
            }

        }
        string fence_name;
        private void map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            try
            {
                 fence_name = e.Marker.Title;
                string id = e.Marker.Snippet;
               
                _fenceid = Convert.ToInt32(id);
                Android.App.AlertDialog.Builder b1 = new AlertDialog.Builder(this);
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View b = layoutInflater.Inflate(Resource.Layout.Edit_Fence_Dialog, null);
               
                b1.SetView(b);
                b1.SetTitle("Update Fence");
                b1.SetMessage("Fence "+fence_name);
                b1.SetPositiveButton("Update", update_fence);
                b1.Show();
                
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        private async void update_fence(object sender, DialogClickEventArgs e)
        {
           ProgressDialog a1 = new ProgressDialog(this);

            var dialog = (Android.App.AlertDialog)sender;
            dialog.Cancel();

            a1.SetMessage("Updating Fence");
            a1.Show();

            var radius = (EditText)dialog.FindViewById(Resource.Id.edit_fenceradius);
            if (radius.Text != String.Empty)
            {
                double r = Convert.ToDouble(radius.Text);

                await update_fence_true(r);

                a1.Hide();
            }
            else
            {
                a1.Hide();
                Toast.MakeText(this, "Please enter valid radius", ToastLength.Short).Show();

            }
      }

        private async System.Threading.Tasks.Task update_fence_true(double r)
        {
            
     
            try
            {
                string url1 = GetString(Resource.String.url);
                string url = url1 + "/Fences/UpdateFence";

                Fence a = new Fence { Fence_id = _fenceid, Fence_radius = r };
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
                map.Clear();
            await getfence();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();

            }
        }
    }
}