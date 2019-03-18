using System;

namespace AndroidPMSiU.Views.Email
{

    public class EmailsPageMenuItem
    {
        public EmailsPageMenuItem()
        {
            TargetType = typeof(EmailsPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}