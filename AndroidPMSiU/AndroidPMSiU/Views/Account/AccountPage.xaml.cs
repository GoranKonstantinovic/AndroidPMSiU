using Acr.UserDialogs;
using AndroidPMSiU.Models;
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
        //private IProgressDialog dialog;
        public AccountPage()
		{
            try
            {
			    InitializeComponent ();

            }
            catch (Exception ex)
            {

                throw;
            }
            //dialog = UserDialogs.Instance.Loading("Учитавање...");

        }

        public void LogoutProcedure(object sender, EventArgs e)
        {
            AuthenticationService.ClearToken();
            RealmMessageService.ClearData();
            Navigation.PushAsync(new LoginPage());
        }
    }
}
