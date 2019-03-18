using AndroidPMSiU.Views.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AndroidPMSiU.Views.Email;
using AndroidPMSiU.Services;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AndroidPMSiU
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            if (AuthenticationService.GetToken() != null)
            {
                MainPage = new EmailsPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            SyncService.TryToSync();
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
