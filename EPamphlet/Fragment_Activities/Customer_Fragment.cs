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
using System.Net;
using System.IO;
using TabSample.Model_Classes;
using TabSample.Layout_Activities;
using Android.Graphics.Drawables;

namespace TabSample.Fragment_Activities
{
    public class Customer_Fragment : Android.Support.V4.App.Fragment
    {
        View v;
        static ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

        ISharedPreferencesEditor editor = prefs.Edit();
      
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            v = inflater.Inflate(Resource.Layout.customer, container, false);
            Button customer_signin = (Button)v.FindViewById(Resource.Id.customer_login);
            Button customer_signup = (Button)v.FindViewById(Resource.Id.customer_register);
           
            if (prefs.GetInt("CustomerID", 0) != 0)
            {

                Intent intent = new Intent(this.Activity, typeof(Customer_Selected_Items));
                string cust_id=prefs.GetInt("CustomerID", 0).ToString();
                intent.PutExtra("CustomerID", cust_id);
                StartActivity(intent);
            }
            customer_signin.Click += async (sender, e) =>
            {
                await signin();
            };
            customer_signup.Click +=  (sender, e) =>
                {
                    Intent intent = new Intent(this.Activity, typeof(Customer_signup));

                    StartActivity(intent);
                };
            return v;
        }

       

        private async System.Threading.Tasks.Task signin()
        {
            
            ProgressBar b2 = (ProgressBar)v.FindViewById(Resource.Id.progressBar2);
            b2.Visibility = ViewStates.Visible;
          
            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Customers/Authenticate";
            Drawable errorIcon = Resources.GetDrawable(Resource.Drawable.images);
        
            EditText customer_username = (EditText)v.FindViewById(Resource.Id.customer_username);
            EditText customer_password = (EditText)v.FindViewById(Resource.Id.customer_password);
            if (customer_username.Text==null)
            {
                customer_username.SetError("please enter username ", errorIcon);
                //b2.Visibility = ViewStates.Invisible;
            }

            else if (customer_password.Text==null)
            {
                customer_password.SetError("please enter password ", errorIcon);
                //b2.Visibility = ViewStates.Invisible;
            }

            else
            {
                Customer a = new Customer { Customer_username = customer_username.Text, Customer_password = customer_password.Text };
                string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                byte[] postDataByteArray = Encoding.UTF8.GetBytes(postDataString);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                request.ContentLength = postDataByteArray.Length;
                request.KeepAlive = true;
                try
                {
                    Stream dataStream = await request.GetRequestStreamAsync();
                    dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                    dataStream.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader r1 = new StreamReader(response.GetResponseStream());
                    var content = r1.ReadToEnd();
                    Toast.MakeText(this.Activity, content, ToastLength.Long).Show();
                    editor.PutInt("CustomerID", int.Parse(content));
                    editor.Apply();
                    editor.Commit();

                    Intent intent = new Intent(this.Activity, typeof(Customer_Selected_Items));
      
                    intent.PutExtra("CustomerID", content.ToString());
                    StartActivity(intent);
                    b2.Visibility = ViewStates.Invisible;
                    //p1.Dismiss();
                }
                catch (Exception ex)
                {
                   // p1.Dismiss();
                    b2.Visibility = ViewStates.Invisible;
                    Toast.MakeText(this.Activity, "Invalid Username or Password Please Reenter", ToastLength.Long).Show();
                
                }
            }
  
        }


       }
}