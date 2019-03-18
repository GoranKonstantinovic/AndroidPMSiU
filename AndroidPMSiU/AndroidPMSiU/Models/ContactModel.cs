using Realms;

namespace AndroidPMSiU.Models
{
    public class ContactModel : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }


        public string PhotoURL { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
        public string FullName { get => string.Join(" ", FirstName, LastName); }
    }
}