using AndroidPMSiU.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.CreateMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DraftMailDetailsPage : ContentPage
	{
		public DraftMailDetailsPage (MessageModel message)
		{
			InitializeComponent ();

            subject.Text = message.Subject;
            stringContactsTo.Text = message.From;
            messageContent.Text = message.MessageContent;
        } 
    }
}