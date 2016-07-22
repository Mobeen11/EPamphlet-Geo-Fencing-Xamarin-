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
    public class Customer_signup : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Customer_Signup);
             Button signup = (Button)FindViewById(Resource.Id.customer_signup);
            signup.Click+=async(s,ea)=>
                {
                await customer_signup();
                };
        }

        private async System.Threading.Tasks.Task customer_signup()
        {
            ProgressBar p1 = (ProgressBar)FindViewById(Resource.Id.progressBar3);
            p1.Visibility = ViewStates.Visible;
                EditText customer_name = (EditText)FindViewById(Resource.Id.customer_name);
                EditText customer_password = (EditText)FindViewById(Resource.Id.customer_password);
                EditText customer_username = (EditText)FindViewById(Resource.Id.customer_username);
                Drawable errorIcon = Resources.GetDrawable(Resource.Drawable.images);

                if (customer_name.Text == String.Empty)
                {
                    customer_name.SetError("please enter name", errorIcon);
                    p1.Visibility = ViewStates.Invisible;
                }
                else if (customer_username.Text == String.Empty)
                {
                    customer_username.SetError("please enter username", errorIcon);
                    p1.Visibility = ViewStates.Invisible;
                }
                else if (customer_password.Text == String.Empty)
                {
                    customer_password.SetError("please enter password ", errorIcon);
                    p1.Visibility = ViewStates.Invisible;
               
                }
                else
            {
                    try
                    {
                        p1.Visibility = ViewStates.Visible;
               
                            string url1 = GetString(Resource.String.url);
                            string url = url1 + "/Customers/Register";
                            Customer a = new Customer { Customer_username = customer_username.Text, Customer_password = customer_password.Text, Customer_name = customer_name.Text };
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

                            Toast.MakeText(this, "Customer Succesfully Added", ToastLength.Short).Show();

                            p1.Visibility = ViewStates.Invisible;
                    }
                    
                    catch (Exception ex)
                    {
                        p1.Visibility = ViewStates.Invisible;
                        Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                    }
                }




               
        }
       
    }
}