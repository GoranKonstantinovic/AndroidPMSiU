using AndroidPMSiU.Models;
using AndroidPMSiU.Services.Realms;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.ReceiveMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReceiveMailDetailsPage : ContentPage
	{
		public ReceiveMailDetailsPage (MessageModel message)
		{

            Title = "Порука";

            InitializeComponent ();

            subject.Text = message.Subject;
            stringContactsFrom.Text = "Од: " + message.From;
            var ccs = message.ContactsCC.Select(x => x.EmailAddress);
            cc.Text = string.Join("; ", ccs);
            receiveTime.Text = "Примљено: " + message.StringDateTime;
            messageContent.Text = message.MessageContent;


        }

        //private void DeleteEMail()
        //{
        //    RealmMessageService.DeleteMessage(message.Id);
        //}
    }
}