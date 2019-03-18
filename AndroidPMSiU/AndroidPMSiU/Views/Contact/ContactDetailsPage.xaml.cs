using AndroidPMSiU.Models;
using AndroidPMSiU.Views.CreateMail;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.Contact
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactDetailsPage : ContentPage
	{
		public ContactDetailsPage (ContactModel contact)
		{
            Title = "Детаљи о контакту";
			InitializeComponent ();

            fullName.Text = contact.FullName;
            displayName.Text = contact.DisplayName;
            emailAdress.Text = contact.EmailAddress;


        }

        //public void SendMessageToContact(object sender, EventArgs e)
        //{
 
        //    var page = new ContactDetailsPage()
        //    {
                
        //    };

        //}

    }
}