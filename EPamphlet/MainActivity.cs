using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using TabSample.Fragment_Activities;
using Xamarin;
using Fragment = Android.Support.V4.App.Fragment;

namespace TabSample
{
    [Activity(Label = "EPamphalet",  Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity 
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.main);
            //Insights.Initialize("be871998f99b4f69ff7495b4704a8ae4a7f73c23", this);
            var fragments = new Fragment[]
            {
                new Admin_Fragment(),               
                new Customer_Fragment()
            };
           
        
            var titles = CharSequence.ArrayFromStringArray(new[]
                {
                    "Admin",
                    "Customer",
                });

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);

            // Give the TabLayout the ViewPager
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
        }
    }
}