using Realms;

namespace AndroidPMSiU.Models
{
    public class AccountModel : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string SMTP { get; set; }
        public string Pop3Imap { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public AccountModel() { }

        public AccountModel(/*int Id, string SMTP, string Pop3Imap, */string Username, string Password)
        {
            //this.Id = Id;
            //this.SMTP = SMTP;
            //this.Pop3Imap = Pop3Imap;
            this.Username = Username;
            this.Password = Password;
        }

        public bool CheckInformation()
        {
            if (Username != null && Password != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class AccountResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

}
