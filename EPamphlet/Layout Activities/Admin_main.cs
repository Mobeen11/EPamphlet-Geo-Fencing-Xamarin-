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
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.App;
using TabSample.Fragment_Activities;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace TabSample.Layout_Activities
{
    [Activity(Label = "")]
    public class Admin_main : FragmentActivity 
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.main);

            var fragments = new Fragment[]
            {
                new Manage_Fence(),               
                new Manage_Vendor()
            };
            var titles = CharSequence.ArrayFromStringArray(new[]
                {
                    "Manage Fence",
                    "Manage Vendor",
                });
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);

            // Give the TabLayout the ViewPager
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
           
        }

        
        public override void OnBackPressed()
        {
           // Toast.MakeText(this, "Please Click on sign out button", ToastLength.Short).Show();
            //base.OnBackPressed();
        }
    }
}