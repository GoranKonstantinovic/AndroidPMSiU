using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidPMSiU.Views.Email
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailsPageMaster : ContentPage
    {
        public ListView ListView;

        public EmailsPageMaster()
        {
            InitializeComponent();

            BindingContext = new EmailsPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class EmailsPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<EmailsPageMenuItem> MenuItems { get; set; }

            public EmailsPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<EmailsPageMenuItem>(new[]
                {
                    new EmailsPageMenuItem { Id = 0, Title = "Послате" },
                    new EmailsPageMenuItem { Id = 1, Title = "Примљене" },
                    new EmailsPageMenuItem { Id = 2, Title = "Контакти" },
                    new EmailsPageMenuItem { Id = 3, Title = "Недовршене" },
                    new EmailsPageMenuItem { Id = 4, Title = "Профил" },
                    new EmailsPageMenuItem { Id = 5, Title = "Општа подешавања" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}