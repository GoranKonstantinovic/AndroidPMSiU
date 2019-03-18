using AndroidPMSiU.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AndroidPMSiU.Views.Email;
using AndroidPMSiU.Services;
using AndroidPMSiU.Services.Realms;
using System.Collections.Generic;
using System.Linq;
using Realms;
using Acr.UserDialogs;

namespace AndroidPMSiU.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private List<AccountModel> accounts;
        private IProgressDialog dialog;
        public LoginPage()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

                throw;
            }
            Entry_Username.Text = "goranpms@gmail.com";
            Entry_Password.Text = "android123.";
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => LoginProcedure(s, e);
            dialog = UserDialogs.Instance.Loading("Учитавање...");
            dialog.Hide();
        }

        async void LoginProcedure(object sender, EventArgs e)
        {
            
            AccountModel account = new AccountModel(Entry_Username.Text, Entry_Password.Text);

            if (account.CheckInformation())
            {               
                bool isSuccess = await AuthenticationService.Login(account);
                if (isSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Пријављивање", "Успешно сте се пријавили", "OK.");
                    dialog.Show();
                    DataModel data = await DataService.GetData();
                    dialog.Hide();
                    RealmMessageService.InsertData(data);
                    SyncService.TryToSync();
                    Application.Current.MainPage = new EmailsPage();
                }
                else
                {
                    await DisplayAlert("Пријављивање", "Пријава неуспешна. Неисправно корисничко име или лозинка.", "OK.");
                }
            }
            else
            {
                await DisplayAlert("Пријављивање", "Пријава неуспешна.", "OK.");
            }
        }
    }
}