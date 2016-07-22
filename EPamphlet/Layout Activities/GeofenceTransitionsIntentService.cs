using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TabSample.Model_Classes;
using Xamarin;

namespace TabSample.Layout_Activities
{
   [Service]
    public class GeofenceTransitionsIntentService :IntentService
     {

         protected const string TAG = "geofence-transitions-service";
         int id;
         int i = 0;
         string Customer_Id;
         public GeofenceTransitionsIntentService()
             : base(TAG)
         {
         }

         string test = "";
         List<string> test_fence= new List<string>();
         //int vendorid;
         protected override void OnHandleIntent(Intent intent)
         {
             var geofencingEvent = GeofencingEvent.FromIntent(intent);
             if (geofencingEvent.HasError)
             {
                 return;
             }

             int geofenceTransition = geofencingEvent.GeofenceTransition;

             if (geofenceTransition == Geofence.GeofenceTransitionEnter ||
                 geofenceTransition == Geofence.GeofenceTransitionExit)
             {

                 IList<IGeofence> triggeringGeofences = geofencingEvent.TriggeringGeofences;

               //  string geofenceTransitionDetails = GetGeofenceTransitionDetails(this, geofenceTransition, triggeringGeofences);
                 test_fence = GetGeofenceTransitionDetails(this, geofenceTransition, triggeringGeofences);
                 //ye rahe id ab is se mene Vendor name nikalna ha
               //  test = geofenceTransitionDetails;

                 //      await SendAdvertisement(geofenceTransitionDetails).ConfigureAwait(false);
                 //  SendNotification(geofenceTransitionDetails);
             }
             else
             {
                 Toast.MakeText(this, "Nothing Happened", ToastLength.Long).Show();
             }
         }

         private async System.Threading.Tasks.Task SendAdvertisement(string geofenceTransitionDetails)
         {
             //yahan se phle mene fence id ke through vendor id nikaalne ha and then mene yahan
             //se advertisement nikalne ha
             try
             {
                 string url = "http://epamhaletwebservice.azurewebsites.net/api/Fences/GetVendor";
                 Fence a = new Fence { Fence_id = int.Parse(geofenceTransitionDetails) };
                 string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                 byte[] postDataByteArray = System.Text.Encoding.UTF8.GetBytes(postDataString);
                 HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                 request.ContentType = "application/json";
                 request.Method = "POST";

                 request.ContentLength = postDataByteArray.Length;
                 request.KeepAlive = true;
                 System.IO.Stream dataStream = request.GetRequestStream();
                 dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                 dataStream.Close();
                 HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                 StreamReader r1 = new StreamReader(response.GetResponseStream());
                 var vendor_id = r1.ReadToEnd();
                 id = Convert.ToInt32(vendor_id); ;
                 Toast.MakeText(this, "In Send Advertisement", ToastLength.Long).Show();

                 Toast.MakeText(this, vendor_id.ToString(), ToastLength.Short).Show();
                 //  string ac = "1";
                 //  vendorid = id;
                 await SendEpamphalet(vendor_id).ConfigureAwait(false);
             }
             catch (Exception ex)
             { }
         }

         private async System.Threading.Tasks.Task SendEpamphalet(string vendor_id)
         {
             try
             {
                 Toast.MakeText(this, "In Send Epamphalet", ToastLength.Long).Show();
                 string url = "http://epamhaletwebservice.azurewebsites.net/api/Vendors/GetAdvertisement";

                 Vendor a = new Vendor { Vendor_id = id };
                 string postDataString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                 byte[] postDataByteArray = System.Text.Encoding.UTF8.GetBytes(postDataString);
                 HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                 request.ContentType = "application/json";
                 request.Method = "POST";

                 request.ContentLength = postDataByteArray.Length;
                 request.KeepAlive = true;
                 System.IO.Stream dataStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
                 dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                 dataStream.Close();
                 HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                 StreamReader r1 = new StreamReader(response.GetResponseStream());
                 var content = r1.ReadToEnd();

                 var _Advertisements = JsonConvert.DeserializeObject<List<Advertisement>>(content);


                 foreach (Advertisement advert in _Advertisements)
                 {
                     SendNotification(advert.Advert_id, advert.Advert_image, advert.Advert_text);
                 }
             }
             catch (Exception ex)
             { }
         }
         List<string> GetGeofenceTransitionDetails(Context context, int geofenceTransition, IList<IGeofence> triggeringGeofences)
         {
             string geofenceTransitionString = GetTransitionString(geofenceTransition);

             var triggeringGeofencesIdsList = new List<string>();
             foreach (IGeofence geofence in triggeringGeofences)
             {
                 triggeringGeofencesIdsList.Add(geofence.RequestId);
             }
          
             var triggeringGeofencesIdsString = string.Join(", ", triggeringGeofencesIdsList);//yahan ha ID
             //yahan se mene ID ke through poore row get krne hai jisme sare information hge triggering geofence ki name and 
             //vendor id lena ha
             return triggeringGeofencesIdsList;
            // return triggeringGeofencesIdsString;
         }

         async System.Threading.Tasks.Task SendNotification(int id, string imagepath, string AdvertisementText)
         {
             Toast.MakeText(this, "In Send Notification", ToastLength.Long).Show();

             try
             {


                 var webClient = new WebClient();

                 Bitmap bit = null;
                 var data = await webClient.DownloadDataTaskAsync(new Uri(imagepath)).ConfigureAwait(false);
                 bit = BitmapFactory.DecodeByteArray(data, 0, data.Length);
                 var notificationIntent = new Intent(ApplicationContext, typeof(Show_Pamphalet));
                 var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
                 stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(Show_Pamphalet)));
                 stackBuilder.AddNextIntent(notificationIntent);
                 notificationIntent.PutExtra("Customer_Id", Customer_Id);
                 notificationIntent.PutExtra("Notification Id", id.ToString());
                 notificationIntent.PutExtra("Image", data);
                 notificationIntent.PutExtra("Advertisement", AdvertisementText);
                 notificationIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);

                 var notificationPendingIntent = stackBuilder.GetPendingIntent(id, (int)PendingIntentFlags.UpdateCurrent);

                 var builder = new NotificationCompat.Builder(this);
                 builder.SetSmallIcon(Resource.Drawable.Icon)
                     .SetLargeIcon(bit)
                     .SetColor(Color.Red)
                     .SetContentTitle(AdvertisementText)

                        .SetContentIntent(notificationPendingIntent);

                 builder.SetAutoCancel(true);

                 var mNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

                 mNotificationManager.Notify(i, builder.Build());
                 i++;


             }
             catch (Exception ex)
             { }
         }

         string GetTransitionString(int transitionType)
         {
             switch (transitionType)
             {
                 case Geofence.GeofenceTransitionEnter:
                     return "Enter";
                 case Geofence.GeofenceTransitionExit:
                     return "Exited";
                 default:
                     return "Unknown";
             }
         }
         [Obsolete("deprecated")]
         public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
         {
            
                 OnHandleIntent(intent);
                
             if (test_fence.Count>0)
                 {
                
                 Customer_Id = intent.GetStringExtra("customerid") ?? "Data not available";
                     Toast.MakeText(this, "Geofence Triggered" , ToastLength.Long).Show();
                     foreach (var item in test_fence)
                     {
                         SendAdvertisement(item).ConfigureAwait(false);
                     }
                 
                     return StartCommandResult.Sticky;
                 }
                 else
                 {
                     Toast.MakeText(this, "No fence triggered", ToastLength.Long).Show();
                     return StartCommandResult.Sticky;
                 }
             
            
         }
       


    }
}
