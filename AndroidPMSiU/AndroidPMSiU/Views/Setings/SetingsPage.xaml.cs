using AndroidPMSiU.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AndroidPMSiU.Views.Setings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SetingsPage : ContentPage
	{
		public SetingsPage ()
		{
			InitializeComponent ();
        }

        private void SyncTimeSettings(object sender, EventArgs e)
        {
            var selectedIndex = RadioButton_Sync.SelectedIndex;
            int sec = 10; 
            switch (selectedIndex)
            {
                case 0:
                    sec = 10;
                    break;
                case 1:
                    sec = 20;
                    break;
                case 2:
                    sec = 60;
                    break;
                case 3:
                    sec = 600;
                    break;
            }
            SettingsService.SaveSyncTime(sec);
            DisplayAlert("Синхронизовано", "Поруке ће бити синхронизоване у жељеном интервалу", "Ок.");
        }
    }
}