using AndroidPMSiU.Models;
using AndroidPMSiU.Services.Realms;
using AndroidPMSiU.Views.ReceiveMail;
using PCLStorage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.CreateMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SentMailDetailsPage : ContentPage
	{
        private long messageId;
        public SentMailDetailsPage (MessageModel message)
		{
            Title = "Порука";
            InitializeComponent ();
        
            subject.Text = message.Subject;
            stringContactsFrom.Text = message.From;

            var ccs = message.ContactsCC.Select(x => x.EmailAddress);
            cc.Text = string.Join("; ", ccs);
            receiveTime.Text = "Примљено: " + message.StringDateTime;

            messageContent.Text = message.MessageContent;
            foreach (var item in message.Attachments)
            {
                attachments.Children.Add(new Label
                {
                    Text = $"{item.Name}{item.Type}",
                    
                });
               //SaveFile(item);
            }

            messageId = message.Id;
        }

        async private void DeleteEMail(object sender, SelectedItemChangedEventArgs e)
        {
            bool delete = await DisplayAlert("Брисање", "Да ли сте сигурни да желите обрисати поруку?", "Потврди", "Одустани");
            if (delete == true)
            {
                RealmMessageService.DeleteMessage(messageId);
                Page page = new SentMailPage();
                await Navigation.PopAsync();
            }
        }


        //public void SaveFile(AttachmentModel attachment)
        //{
        //    string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        //    string settingsPath = Path.Combine(path, attachment.Name + attachment.Type);
        //    StreamWriter stream = File.CreateText(settingsPath);
        //    byte[] bytes = System.Convert.FromBase64String(attachment.Data);
        //    stream.Write(bytes);
        //    stream.Close();
        //}
    }
}