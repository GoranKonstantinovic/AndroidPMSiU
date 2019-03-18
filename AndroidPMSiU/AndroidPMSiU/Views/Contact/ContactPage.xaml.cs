using Acr.UserDialogs;
using AndroidPMSiU.Models;
using AndroidPMSiU.Services.Realms;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.Contact
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactPage : ContentPage
	{
        private IProgressDialog dialog;
        public ContactPage ()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw;
            }
            dialog = UserDialogs.Instance.Loading("Учитавање...");
        }

        private void CreateContactProcedure(object obj, EventArgs e)
        {
            Navigation.PushAsync(new CreateContactPage());

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            dialog.Show();
            var contacts = RealmMessageService.GetAllContacts();
            MyList.ItemsSource = contacts;
            dialog.Hide();
        }

        private void MyList_ContactSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = (sender as ListView).SelectedItem as ContactModel;
            if (data == null)
            {
                return;
            }

            Page page = new ContactDetailsPage(data);
            Navigation.PushAsync(page);
            (sender as ListView).SelectedItem = null;
        }

	}
}