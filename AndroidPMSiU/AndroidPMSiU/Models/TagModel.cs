using Realms;
namespace AndroidPMSiU.Models
{
    public class TagModel : RealmObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}