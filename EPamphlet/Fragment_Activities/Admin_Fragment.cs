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
using TabSample.Layout_Activities;
using Android.Graphics.Drawables;

namespace TabSample.Fragment_Activities
{
    public class Admin_Fragment : Android.Support.V4.App.Fragment
    {
        View v;

        static ISharedPreferences prefs= Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);

        ISharedPreferencesEditor editor = prefs.Edit();
         
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            v = inflater.Inflate(Resource.Layout.admin, container, false);
            Button b1 = (Button)v.FindViewById(Resource.Id.admin_login);
        
            if(prefs.GetInt("Admin ID",0)!=0)
             {
                
                 Intent intent = new Intent(this.Activity, typeof(Admin_main));
                 Toast.MakeText(this.Activity, "Hello",ToastLength.Short).Show();
                //  b2.Visibility = ViewStates.Invisible;
                 StartActivity(intent);
             }
            b1.Click += async (sender, e) =>
            {

                await login_click();
            };
            
            return v;
      
        }

        private async System.Threading.Tasks.Task login_click()
        {
            ProgressBar b2 = (ProgressBar)v.FindViewById(Resource.Id.progressBar1);
            b2.Visibility = ViewStates.Visible;
          
            string url1 = GetString(Resource.String.url);
            string url = url1 + "/Admins/Authenticate";
            Drawable errorIcon = Resources.GetDrawable(Resource.Drawable.images);
        
            EditText admin_username = (EditText)v.FindViewById(Resource.Id.admin_username);
            EditText admin_password = (EditText)v.FindViewById(Resource.Id.admin_password);
            if (admin_username.Text==String.Empty)
           {
               admin_username.SetError("please enter username", errorIcon);
               b2.Visibility = ViewStates.Invisible;
           }
            else if (admin_password.Text == String.Empty)
           {
               admin_password.SetError("please enter password ", errorIcon);
               b2.Visibility = ViewStates.Invisible;
           }
        
           else
           {
                       Admin a = new Admin { Admin_username = admin_username.Text, Admin_password = admin_password.Text };
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
                           editor.PutInt("Admin ID", int.Parse(content));
                           editor.Apply();
                           editor.Commit();

                           Toast.MakeText(this.Activity, content, ToastLength.Long).Show();
                           Intent intent = new Intent(this.Activity, typeof(Admin_main));
                           b2.Visibility = ViewStates.Invisible;
                           StartActivity(intent);
                       }
                       catch (Exception ex)
                       {
                           b2.Visibility = ViewStates.Gone;
                           Toast.MakeText(this.Activity, "Admin Not Found", ToastLength.Long).Show();
                       }
               
           }
        }
       
       
    }
}