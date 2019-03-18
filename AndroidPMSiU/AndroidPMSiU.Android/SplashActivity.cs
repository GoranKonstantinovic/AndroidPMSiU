using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using AndroidPMSiU.Droid;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace com.xamarin.sample.splashscreen
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");

        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            var currentTime = System.DateTime.Now;
            bool fiveMinutesPass = false;
            while (!fiveMinutesPass)
            {
                fiveMinutesPass = (System.DateTime.Now - currentTime).TotalSeconds >= 5;
            }
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            //Log.Debug(TAG, "Performing some startup work that takes a bit of time.");

            if (!CrossConnectivity.Current.IsConnected)
            {
                //Toast.MakeText(Application.Context, "Уређај није повезан на интернет", ToastLength.Long).Show();
                ShowToast("Уређај није повезан на интернет", true);

                CrossConnectivity.Current.ConnectivityChanged += (sender, e) =>
                {
                    if (e.IsConnected)
                    {
                        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    }
                };
            }
            else
            {
                // await Task.Delay(5000); // Simulate a bit of startup work.

                Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
        }

        public void ShowToast(string text, bool IsLengthShort = false)
        {
            Handler mainHandler = new Handler(Looper.MainLooper);
            Java.Lang.Runnable runnableToast = new Java.Lang.Runnable(() =>
            {
                var duration = IsLengthShort ? ToastLength.Short : ToastLength.Long;
                Toast.MakeText(ApplicationContext, text, duration).Show();
            });

            mainHandler.Post(runnableToast);
        }

    }
}