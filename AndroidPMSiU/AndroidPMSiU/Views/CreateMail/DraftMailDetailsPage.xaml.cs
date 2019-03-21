using AndroidPMSiU.Models;
using AndroidPMSiU.Services.Realms;
using AndroidPMSiU.Views.ReceiveMail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.CreateMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DraftMailDetailsPage : ContentPage
	{
        private long messageId;
        public DraftMailDetailsPage (MessageModel message)
		{
			InitializeComponent ();

            subject.Text = message.Subject;
            stringContactsTo.Text = message.From;
            messageContent.Text = message.MessageContent;

            messageId = message.Id;
        }

        async private void DeleteEMail(object sender, SelectedItemChangedEventArgs e)
        {
            bool delete = await DisplayAlert("Брисање", "Да ли сте сигурни да желите обрисати поруку?", "Потврди", "Одустани");
            if (delete == true)
            {
                RealmMessageService.DeleteMessage(messageId);
                Page page = new DraftMailPage();
                await Navigation.PopAsync();
            }
        }
    }
}