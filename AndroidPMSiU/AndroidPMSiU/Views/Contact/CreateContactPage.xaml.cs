using AndroidPMSiU.Models;
using AndroidPMSiU.Services;
using AndroidPMSiU.Services.Realms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.Contact
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateContactPage : ContentPage
	{
        private string fileName;
        private byte[] filebytes;

		public CreateContactPage ()
		{
			InitializeComponent ();
            Entry_Lastname.Completed += (s, e) => Entry_DisplayName.Focus();
            Entry_DisplayName.Completed += (s, e) => Entry_Email_Adress.Focus();
        }


        async private void SaveContactProcedure(object obj, EventArgs e)
        {
            bool save = await DisplayAlert("Сачувај","Да ли желите да сачувате контакт", "Потврди", "Одустани");
            if (save == true)
            {
                string fileURL = null;
                
                ContactModel contactDetails = new ContactModel();
                contactDetails.FirstName = Entry_Firstname.Text;
                contactDetails.LastName = Entry_Lastname.Text;
                contactDetails.DisplayName = Entry_DisplayName.Text;
                contactDetails.EmailAddress = Entry_Email_Adress.Text;
                if(string.IsNullOrEmpty(Entry_Firstname.Text) || string.IsNullOrEmpty(Entry_Lastname.Text) || string.IsNullOrEmpty(Entry_DisplayName.Text) || string.IsNullOrEmpty(Entry_Email_Adress.Text)) { 
                    await DisplayAlert("Неуспешно", "Морате попунити сва поља", "Ок.");
                }
                else
                {
                    if (!IsEmailValid(Entry_Email_Adress.Text))
                    {
                        await DisplayAlert("E-Mail", "Неисправна адреса!", "Ок.");
                    }
                    else
                    {
                        if (filebytes != null)
                        {
                            fileURL = await UploadService.UploadImage(filebytes, fileName);
                        }
                        contactDetails.PhotoURL = fileURL;
                        ContactModel newContact = await ContactService.CreateContact(contactDetails);
                        if (newContact != null)
                        {
                            await DisplayAlert("Успешно сачуван", "Контакт је успешно сачуван", "Ок.");

                            RealmMessageService.InsertContact(newContact);

                            Page pageSuccess = new ContactPage();

                            
                            await Navigation.PushAsync(pageSuccess);
                        }
                        else
                        {
                            await DisplayAlert("Неуспешно", "Контакт није сачуван", "Ок.");
                        }
                    }
                }
            }
            else
            {
                await DisplayAlert("Одустани", "Да ли сте сигурни да желите одустати?", "Да.");
            }
        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            filebytes = null;
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);


            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {

                var action = await DisplayActionSheet("Изабери:", "Поништи", null, "Камера", "Галерија");
                MediaFile file = null;
                if (action.Equals("Камера"))
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("Камера", "Камера није доступна", "OK");
                        return;
                    }

                    file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Test",
                        SaveToAlbum = true,
                        CompressionQuality = 75,
                        CustomPhotoSize = 50,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 2000,
                        DefaultCamera = CameraDevice.Front
                    });

                    if (file == null)
                        return;

                    await DisplayAlert("File Location", file.Path, "OK");
                    filebytes = ReadFully(file.GetStream());
                    fileName = Path.GetFileName(file.Path);
                    Image_Contact.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                }
                else if(action.Equals("Галерија"))
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Фотоградије нису подржане", "Одбијен приступ галерији", "OK");
                        return;
                    }
                    file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                    });


                    if (file == null)
                        return;


                    filebytes = ReadFully(file.GetStream());
                    fileName = Path.GetFileName(file.Path);

                    Image_Contact.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                }
            }
            else
            {
                await DisplayAlert("Приступ одбијен", "Није могуће изабрати фотографију", "OK");
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

        private bool IsEmailValid(string emailAddress)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(emailAddress);
        }
    }
}