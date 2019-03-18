using AndroidPMSiU.Services;
using AndroidPMSiU.Views.Account;
using AndroidPMSiU.Views.Contact;
using AndroidPMSiU.Views.CreateMail;
using AndroidPMSiU.Views.Login;
using AndroidPMSiU.Views.ReceiveMail;
using AndroidPMSiU.Views.Setings;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.Email
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailsPage : MasterDetailPage
    {
        public EmailsPage()
        {

            InitializeComponent();

            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as EmailsPageMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            switch (item.Id)
            {
                case 0:
                    page = new SentMailPage();
                    break;
                case 1:
                    page = new ReceiveMailPage();
                    break;
                case 2:
                    page = new ContactPage();
                    break;
                case 3:
                    page = new DraftMailPage();
                    break;
                case 4:
                    page = new AccountPage();
                    break;
                case 5:
                    page = new SetingsPage();
                    break;
            }

            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}