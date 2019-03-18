using AndroidPMSiU.Models;
using AndroidPMSiU.Services.Realms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.CreateMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SentMailDetailsPage : ContentPage
	{
		public SentMailDetailsPage (MessageModel message)
		{
            Title = "Порука";
            InitializeComponent ();
        
            subject.Text = message.Subject;
            stringContactsFrom.Text = message.From;
            messageContent.Text = message.MessageContent;
        }
    }
}