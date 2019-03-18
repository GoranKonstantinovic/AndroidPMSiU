using AndroidPMSiU.Services;
using AndroidPMSiU.Services.Realms;
using AndroidPMSiU.Views.Login;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.Account
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{
		public AccountPage ()
		{
			InitializeComponent ();
		}

        public void LogoutProcedure(object sender, EventArgs e)
        {
            AuthenticationService.ClearToken();
            RealmMessageService.ClearData();
            Navigation.PushAsync(new LoginPage());
        }
    }
}