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
using System.Threading.Tasks;
using TabSample.Fragment_Activities;
using com.refractored.fab;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "Customer_Selected_Items")]
    public class Customer_Selected_Items : Activity
    {
        ListView listView;
        List<String> lItems= new List<string>();
        Button vendors_see;
        int customer_id;
        protected  override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string test = Intent.GetStringExtra("CustomerID") ?? "Data not available";
            customer_id = int.Parse(test);
            SetContentView(Resource.Layout.Customer_Choice_True);
            Button signout = (Button)FindViewById(Resource.Id.cust_signout);
            signout.Click += signout_Click;
            listView = FindViewById<ListView>(Resource.Id.customers_selected_choice);
 
            vendors_see = (Button)FindViewById(Resource.Id.See_vendor);
        
            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
           fab.AttachToListView(listView);
           fab.Show();
           fab.Click += fab_Click;
          vendors_see.Click += vendors_see_Click;
           listView.ItemClick += listView_ItemClick;
    
            SetupList();
  
         }

     

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            CheckedTextView c1 = (CheckedTextView)e.View;
            if ((c1.Checked)&&listView.CheckedItemCount>0)
            {
                
                vendors_see.Enabled = true;
             
                vendors_see.SetBackgroundColor(Resources.GetColor(Resource.Color.primary));
            }
            else if (listView.CheckedItemCount <= 0)
            {   vendors_see.Enabled = false;
            vendors_see.SetBackgroundColor(Resources.GetColor(Resource.Color.disabled_color));
            }
        }

       

        void fab_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(Customer_main));
            i.PutExtra("CustomerID", customer_id.ToString());
            StartActivity(i);
        }

        void signout_Click(object sender, EventArgs e)
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove("CustomerID");

            editor.Commit();
           // StartActivity(Customer_Fragment);    
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);

        }

       async void vendors_see_Click(object sender, EventArgs e)
        {
            ProgressDialog a1 = new ProgressDialog(this);
            a1.SetMessage("Performing Operation ..");
            a1.Show();

            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Vendors/VendorId";
            var builder = new StringBuilder();
            var sparsearray = FindViewById<ListView>(Resource.Id.customers_selected_choice).CheckedItemPositions;
            for (var i = 0; i < sparsearray.Size(); i++)
            {

                try
                {
                    string vendor_name = listView.GetItemAtPosition(sparsearray.KeyAt(i)).ToString();
                    Vendor a = new Vendor { Vendor_name=vendor_name };
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
                   int vendor_id = Convert.ToInt32(content);
                   delete_choice(vendor_id);
                }
                catch (Exception ex)
                { a1.Hide(); }
            }

            a1.Hide();
            RefreshList();
       }

       private async void RefreshList()
       {
           ProgressDialog a1 = new ProgressDialog(this);
           a1.SetMessage("Refreshing..");
           a1.Show();

           a1.SetProgressStyle(ProgressDialogStyle.Spinner);
            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Choices/Get";
            try
            {
                List<string> refresh = new List<string>();
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
        if(_Data.Count==0)
        {
            vendors_see.Enabled = false;
            vendors_see.SetBackgroundColor(Resources.GetColor(Resource.Color.disabled_color));
        }
        else
        {
            foreach (var item in _Data)
            {
                string url2 = GetString(Resource.String.url);
                string url3 = url2 + "/Vendors/VendorName";

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
                    refresh.Add(v.Vendor_name);
                }


            }
        }
                listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemMultipleChoice, refresh);
                listView.ChoiceMode = ChoiceMode.Multiple;
                a1.Hide();
            }
           catch(Exception ex)
            { a1.Hide(); }

       }

       private async void delete_choice(int vendor_id)
       {
           string url1 = GetString(Resource.String.url);
           string url = url1 + "/Choices/Remove";
           Vendor a = new Vendor { Vendor_id = vendor_id };
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

           Toast.MakeText(this, "Choice Removed", ToastLength.Short).Show();
           
       }
        private async void SetupList()
        {
            ProgressDialog a1 = new ProgressDialog(this);
            a1.SetMessage("Loading..");
            a1.Show();

            a1.SetProgressStyle(ProgressDialogStyle.Spinner);
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
                            lItems.Add(v.Vendor_name);
                        }
                      
                        
                    }
                    catch (Exception ex)
                    {
                        a1.Hide();
                    }
         
                }
                if (lItems.Count == 0)
                {
                    a1.Hide();
                    new Android.App.AlertDialog.Builder(this).SetView(LayoutInflater.Inflate(Resource.Layout.Warning, null)).SetTitle("Welcome")
              .SetPositiveButton("Ok", test
              ).Show();
                }
                else
                {

                    listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemMultipleChoice, lItems);
                    listView.ChoiceMode = ChoiceMode.Multiple;
                    a1.Hide();
                }
            }
            catch (Exception ex)
            {
                a1.Hide();
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }

        private void test(object sender, DialogClickEventArgs e)
        {
     
        }

          protected  override void OnRestart()
{
    base.OnRestart();
    ProgressDialog a1 = new ProgressDialog(this);
    a1.SetMessage("Loading..");
    a1.Show();

              listView.SetAdapter(null);
    lItems.Clear();
         SetupList();
         a1.Hide();
          }



        private async void GetVendorName(int vendorid)
        {


            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Vendors/VendorName";
            try
            {
                Vendor a = new Vendor { Vendor_id = vendorid };
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

            }
            catch (Exception ex)

            {
            }
            
    
        }
        public override void OnBackPressed()
        {
            // Toast.MakeText(this, "Please Click on sign out button", ToastLength.Short).Show();
            //base.OnBackPressed();
        }
       

    }
}