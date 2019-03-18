using System.Collections.Generic;

namespace AndroidPMSiU.Models
{
    public class DataModel
    {
        public List<MessageModel> Sent { get; set; }
        public List<MessageModel> Received { get; set; }
        public List<MessageModel> Draft { get; set; }
        public List<ContactModel> Contacts { get; set; }
    }
}