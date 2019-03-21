using Realms;
using System.IO;

namespace AndroidPMSiU.Models
{
    public class AttachmentModel : RealmObject
    {
        public string Data { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}