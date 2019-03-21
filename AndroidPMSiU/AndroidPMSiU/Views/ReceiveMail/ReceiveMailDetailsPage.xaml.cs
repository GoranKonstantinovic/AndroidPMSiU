using AndroidPMSiU.Models;
using AndroidPMSiU.Services;
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

        private long messageId;
        public ReceiveMailDetailsPage(MessageModel message)
        {

            Title = "Порука";

            InitializeComponent();

            subject.Text = message.Subject;
            stringContactsFrom.Text = "Од: " + message.From;
            var ccs = message.ContactsCC.Select(x => x.EmailAddress);
            cc.Text = string.Join("; ", ccs);
            receiveTime.Text = "Примљено: " + message.StringDateTime;
            messageContent.Text = message.MessageContent;

            messageId = message.Id;
        }

        async private void DeleteEMail(object sender, SelectedItemChangedEventArgs e)
        {
            bool delete = await DisplayAlert("Брисање", "Да ли сте сигурни да желите обрисати поруку?", "Потврди", "Одустани");
            if (delete == true)
            {
                RealmMessageService.DeleteMessage(messageId);
                MessageService.DeleteMessage(messageId);
                await Navigation.PopAsync();

            }
        }
    }
}