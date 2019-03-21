using Acr.UserDialogs;
using AndroidPMSiU.Models;
using AndroidPMSiU.Services;
using AndroidPMSiU.Services.Realms;
using dotMorten.Xamarin.Forms;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.CreateMail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class CreateMailPage : ContentPage
    {
        private IProgressDialog dialog;
        private bool _isImagePicking;
        private List<ContactModel> _contacts;
        private List<ContactModel> _toContacts;
        private List<ContactModel> _toCC;
        private List<ContactModel> _toBCC;
        private List<AttachmentModel> _attachments;
        bool isMessageSent = false;

        public CreateMailPage(string email)
        {
            try
            {
                InitializeComponent();
                Entry_Message_To.Text = email;
            }
            catch (Exception)
            {

                throw;
            }
           

            dialog = UserDialogs.Instance.Loading("Учитавање...");
            dialog.Hide();

            _contacts = RealmMessageService.GetAllContacts();
            _toContacts = new List<ContactModel>();
            _toCC = new List<ContactModel>();
            _toBCC = new List<ContactModel>();
            _attachments = new List<AttachmentModel>();

        }

        async private void SendMessageProcedure(object sender, EventArgs e)
        {
            bool send = await DisplayAlert("Пошаљи", "Потврдите слање поруке", "Потврди", "Одустани");
            if (send == true)
            {
                await DisplayAlert("Пошаљи", string.Join(", ", _toContacts.Select(x => x.DisplayName).ToArray()), "Потврди");
                       
                if (_toContacts.Any() && !string.IsNullOrWhiteSpace(Entry_Message_Subject.Text) && !string.IsNullOrWhiteSpace(Entry_Message_Content.Text))
                {
                    SaveMessageProcedure(RealmMessageService.SENT_MESSAGE_TYPE, false);                       
                }
                else
                {
                    Console.WriteLine("Морате уписати име примаоца");
                }
            }
            else
            {
                Console.WriteLine("Порука није послата!");
            }
        }        

        protected async override void OnDisappearing()
        {
            //base.OnDisappearing();

            if (!_isImagePicking && !isMessageSent && (
                !string.IsNullOrEmpty(Entry_Message_To.Text) ||
                !string.IsNullOrEmpty(Entry_Message_CC.Text) ||
                _toContacts.Any() ||
                !string.IsNullOrEmpty(Entry_Message_BCC.Text) ||
                _toCC.Any() ||  
                _toBCC.Any() ||
                _attachments.Any() ||
                !string.IsNullOrEmpty(Entry_Message_Subject.Text) ||
                !string.IsNullOrEmpty(Entry_Message_Content.Text)))
            {
                await SaveMessageProcedure(RealmMessageService.DRAFT_MESSAGE_TYPE, true);
            }
        }


        private async Task<bool> SaveMessageProcedure(int type, bool draft)
        {

            MessageModel sentMessages = new MessageModel();
            sentMessages.Id = DateTime.Now.Ticks;
            sentMessages.FolderId = 1;
            sentMessages.From = null;
            sentMessages.Subject = Entry_Message_Content.Text;
            sentMessages.MessageContent = Entry_Message_Content.Text;
            sentMessages.IsCreatedInMobileApp = true;
            sentMessages.Type = type;
            

            SendMessageModel sentModel = new SendMessageModel();
            sentModel.Subject = Entry_Message_Subject.Text;
            sentModel.MessageContent = Entry_Message_Content.Text;
            sentModel.DateTime = DateTime.Now;

            foreach (var item in _toContacts)
            {
                sentMessages.ContactsToIds.Add(item.Id);
                sentModel.ContactsTo.Add(item.Id);
            }

            foreach (var item in _toCC)
            {
                sentMessages.ContactsCCIds.Add(item.Id);
                sentModel.ContactsCC.Add(item.Id);
            }

            foreach (var item in _toBCC)
            {
                sentMessages.ContactsBCCIds.Add(item.Id);
                sentModel.ContactsBCC.Add(item.Id);
            }

            foreach (var item in _attachments)
            {
                sentMessages.Attachments.Add(item);
                sentModel.Attachments.Add(item);
            }

            sentModel.IsDraft = draft;

            dialog.Show();
            bool isCreated = await MessageService.CreateSentMessage(sentModel);
            dialog.Hide();

            sentMessages.IsCreatedInMobileApp = !isCreated;
            RealmMessageService.InsertMessages(sentMessages);

            isMessageSent = true;

            if (!draft)
            {
                await Navigation.PopAsync();

            }
            return true;
        }



        #region ContactsTo
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                   
                    var term = sender.Text;                

                    if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
                        sender.ItemsSource = null;
                    else
                    {
                        sender.ItemsSource = _contacts
                        .Where(x => !_toContacts.Any(y => y.Id == x.Id))
                        .Where(x => !_toCC.Any(y => y.Id == x.Id))
                        .Where(x => !_toBCC.Any(y => y.Id == x.Id))
                        .Where(x => x.FirstName.ToLower().Contains(term.ToLower())
                        || x.LastName.ToLower().Contains(term.ToLower())
                        || x.DisplayName.ToLower().Contains(term.ToLower())
                        || x.EmailAddress.ToLower().Contains(term.ToLower()))                         
                        .Select(x => x.EmailAddress)
                        .ToList();                     
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {

                var selectedContact = _contacts.FirstOrDefault(x => x.EmailAddress.Equals(args.ChosenSuggestion));
                _toContacts.Add(selectedContact);
                DrawSelectedContacts(Stack_Contacts);
                sender.Text = "";
                sender.ItemsSource = null;
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }
        #endregion

        #region ContactsCC

        private void AutoSuggestBox_TextChangedCC(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    var term = sender.Text;

                    if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
                        sender.ItemsSource = null;
                    else
                    {
                         sender.ItemsSource = _contacts
                         .Where(x => !_toContacts.Any(y => y.Id == x.Id))
                         .Where(x => !_toCC.Any(y => y.Id == x.Id))
                         .Where(x => !_toBCC.Any(y => y.Id == x.Id))
                         .Where(x => x.FirstName.ToLower().Contains(term.ToLower())
                         || x.LastName.ToLower().Contains(term.ToLower())
                         || x.DisplayName.ToLower().Contains(term.ToLower())
                         || x.EmailAddress.ToLower().Contains(term.ToLower()))
                         .Select(x => x.EmailAddress)
                         .ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void AutoSuggestBox_QuerySubmittedCC(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {

                var selectedContact = _contacts.FirstOrDefault(x => x.EmailAddress.Equals(args.ChosenSuggestion));
                _toCC.Add(selectedContact);
                DrawSelectedContactsCC(Stack_CC);
                sender.Text = "";
                sender.ItemsSource = null;
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }

        #endregion

        #region ContactsBCC

        private void AutoSuggestBox_TextChangedBCC(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    var term = sender.Text;

                    if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
                        sender.ItemsSource = null;
                    else
                    {
                        sender.ItemsSource = _contacts
                        .Where(x => !_toContacts.Any(y => y.Id == x.Id))
                        .Where(x => !_toCC.Any(y => y.Id == x.Id))
                        .Where(x => !_toBCC.Any(y => y.Id == x.Id))
                        .Where(x => x.FirstName.ToLower().Contains(term.ToLower())
                        || x.LastName.ToLower().Contains(term.ToLower())
                        || x.DisplayName.ToLower().Contains(term.ToLower())
                        || x.EmailAddress.ToLower().Contains(term.ToLower()))
                        .Select(x => x.EmailAddress)
                        .ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void AutoSuggestBox_QuerySubmittedBCC(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {

                var selectedContact = _contacts.FirstOrDefault(x => x.EmailAddress.Equals(args.ChosenSuggestion));
                _toBCC.Add(selectedContact);
                DrawSelectedContactsBCC(Stack_BCC);
                sender.Text = "";
                sender.ItemsSource = null;
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }

        #endregion

        private void DrawSelectedContacts(StackLayout stack)
        {
            stack.Children.Clear();
            foreach (var contact in _toContacts)
            {
                stack.Children.Add(new Label { Text = contact.DisplayName });
                var clearImage = new Image { Source = "clear.png" };
                clearImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    TappedCallback = delegate
                    {
                        if (_toContacts.Any(x => x.Id == contact.Id))
                        {
                            _toContacts.Remove(contact);
                            DrawSelectedContacts(stack);
                        }
                    }
                });
                stack.Children.Add(clearImage);
            }
        }

        private void DrawSelectedContactsCC(StackLayout stackCC)
        {
            stackCC.Children.Clear();
            foreach (var contact in _toCC)
            {
                stackCC.Children.Add(new Label { Text = contact.DisplayName });
                var clearImage = new Image { Source = "clear.png" };
                clearImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    TappedCallback = delegate
                    {
                        if (_toCC.Any(x => x.Id == contact.Id))
                        {
                            _toCC.Remove(contact);
                            DrawSelectedContactsCC(stackCC);
                        }
                    }
                });
                stackCC.Children.Add(clearImage);
            }
        }

        private void DrawSelectedContactsBCC(StackLayout stackBCC)
        {
            stackBCC.Children.Clear();
            foreach (var contact in _toBCC)
            {
                stackBCC.Children.Add(new Label { Text = contact.DisplayName });
                var clearImage = new Image { Source = "clear.png" };
                clearImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    TappedCallback = delegate
                    {
                        if (_toBCC.Any(x => x.Id == contact.Id))
                        {
                            _toBCC.Remove(contact);
                            DrawSelectedContactsBCC(stackBCC);
                        }
                    }
                });
                stackBCC.Children.Add(clearImage);
            }
        }

        private async void FilePickerProcedure(object sender, EventArgs e)
        {
            try
            {
                _isImagePicking = true;
                var pickedFile = await CrossFilePicker.Current.PickFile();
                _isImagePicking = false;
                if (pickedFile != null)
                {
                    var bytes = ReadFully(pickedFile.GetStream());
                    AttachmentModel attachment = new AttachmentModel
                    {
                        Name = Path.GetFileName(pickedFile.FilePath).Split('.').FirstOrDefault(),
                        Type = Path.GetExtension(pickedFile.FilePath),
                        Data = Convert.ToBase64String(bytes)
                    };

                    _attachments.Add(attachment);
                    DrawSelectedAttachments(Stack_Attachments);
                    pickedFile.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DrawSelectedAttachments(StackLayout stackAttachments)
        {
            stackAttachments.Children.Clear();
            foreach (var attachment in _attachments)
            {
                stackAttachments.Children.Add(new Label { Text = attachment.Name });
                var clearImage = new Image { Source = "clear.png" };
                clearImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    TappedCallback = delegate
                    {
                        if (_attachments.Any(x => x.Name == attachment.Name))
                        {
                            _attachments.Remove(attachment);
                            DrawSelectedAttachments(stackAttachments);
                        }
                    }
                });
                stackAttachments.Children.Add(clearImage);
            }
        }

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;  
        }

    }

    
}