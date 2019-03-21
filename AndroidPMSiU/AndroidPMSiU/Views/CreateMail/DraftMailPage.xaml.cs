using Acr.UserDialogs;
using AndroidPMSiU.Models;
using AndroidPMSiU.Services.Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.CreateMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DraftMailPage : ContentPage
	{
        private IProgressDialog dialog;
        private List<MessageModel> emails;

        public DraftMailPage ()
		{
            try
            {
			    InitializeComponent ();

            }
            catch (Exception)
            {
                throw;
            }
            dialog = UserDialogs.Instance.Loading("Учитавање...");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            dialog.Show();
            emails = RealmMessageService.GetMessagesByType(RealmMessageService.DRAFT_MESSAGE_TYPE);
            MyList.ItemsSource = emails;
            dialog.Hide();
        }

        private void CreateMailProcedure(object obj, EventArgs e)
        {
            Navigation.PushAsync(new CreateMailPage(""));
        }

        int filter = 1;
        private void FilterMailProcedure(object sender, EventArgs e)
        {

            filter++;
            if ((filter % 2) == 0)
            {
                Entry_Filter.Text = "";
                ShowFilter();
            }
            else
            {
                HideFilter();
            }
        }

        int sorter = 1;
        private void SortMailProcedure(object obj, EventArgs e)
        {
            sorter++;
            if ((sorter % 2) == 0)
            {
                emails = emails.OrderBy(x => x.DateTime).ToList();
            }
            else
            {
                emails = emails.OrderByDescending(x => x.DateTime).ToList();
            }
            MyList.ItemsSource = emails;
        }


        private void FilterMailParameterProcedure(object sender, EventArgs e)
        {
            var selectedIndex = Picker_Filter.SelectedIndex;
            emails = RealmMessageService.GetMessagesByType(RealmMessageService.RECIVE_MESSAGE_TYPE);
            if (selectedIndex == 0)
            {
                HideFilter();
                emails = emails.Where(x => x.Subject.ToLower().Contains(Entry_Filter.Text.ToLower())).ToList();
            }
            if (selectedIndex == 1)
            {
                HideFilter();
                emails = emails.Where(x => x.MessageContent.ToLower().Contains(Entry_Filter.Text.ToLower())).ToList();
            }
            if (selectedIndex == 2)
            {
                HideFilter();
                emails = emails.Where(x => x.ContactsTo.Any(z => z.DisplayName.ToLower().Contains(Entry_Filter.Text.ToLower()))).ToList();
            }
            if (selectedIndex == 3)
            {
                HideFilter();
                emails = emails.Where(x => x.From.ToLower().Contains(Entry_Filter.Text.ToLower())).ToList();
            }
            if (selectedIndex == 4)
            {
                HideFilter();
                emails = emails.Where(x => x.ContactsCC.Any(z => z.DisplayName.ToLower().Contains(Entry_Filter.Text.ToLower()))).ToList();
            }
            if (selectedIndex == 5)
            {
                HideFilter();
                emails = emails.Where(x => x.ContactsBCC.Any(z => z.DisplayName.ToLower().Contains(Entry_Filter.Text.ToLower()))).ToList();
            }
            MyList.ItemsSource = emails;
        }

        private void ShowFilter()
        {
            FilterContainer.Opacity = 0;
            FilterContainer.IsVisible = true;
            FilterContainer.FadeTo(1, 300);
            MainContainer.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
            MainContainer.IsEnabled = false;
            MyList.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
        }

        private void HideFilter()
        {

            FilterContainer.FadeTo(1, 300);
            Task.Delay(300);
            MainContainer.BackgroundColor = Color.White;
            FilterContainer.IsVisible = false;
            MainContainer.IsEnabled = true;
            MyList.BackgroundColor = Color.FromHex("#fff0d3");
        }

        private void MyList_EmailSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = (sender as ListView).SelectedItem as MessageModel;
            if (data == null)
            {
                return;
            }

            Page page = new DraftMailDetailsPage(data);
            Navigation.PushAsync(page);
            (sender as ListView).SelectedItem = null;
        }
    }
}