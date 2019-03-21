using Realms;
using System;
using System.Collections.Generic;
using System.IO;

namespace AndroidPMSiU.Models
{
    public class MessageModel : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string UID { get; set; }
        public DateTimeOffset DateTime { get; set; }



        public string Subject { get; set; }
        public string MessageContent { get; set; }

        public long FolderId { get; set; }


        public string From { get; set; }

        [Ignored]
        public string StringContactsTo { get; set; }

        public IList<long> ContactsToIds { get; }
        [Ignored]
        public List<ContactModel> ContactsTo { get; set; }

        [Ignored]
        public string StringContactsFrom { get; set; }//


        public IList<long> ContactsFromIds { get;}
        [Ignored]
        public List<ContactModel> ContactsFrom { get; set; } //

        public IList<long> ContactsCCIds { get; }
        [Ignored]
        public List<ContactModel> ContactsCC { get; set; }

        [Ignored]
        public string StringContactsCC { get; set; } //

        public IList<long> ContactsBCCIds { get; }
        [Ignored]
        public List<ContactModel> ContactsBCC { get; set; }

        [Ignored]
        public string StringContactsBCC { get; set; }

        public IList<long> TagIds { get; }
        [Ignored]
        public List<TagModel> Tags { get; set; }

        public IList<AttachmentModel> Attachments { get;}

        //[Ignored]
        //public List<AttachmentModel> AttachmentString { get; set; }


        public int Type { get; set; }

        public bool IsCreatedInMobileApp { get; set; }

        public bool IsRead { get; set; }

        public bool IsDelete { get; set; }

        private string backgroundColor;

        [Ignored]
        public string BackgroundColor
        {
            get
            {
                if (IsRead)
                {
                    return "#fff0d3";
                }
                else
                {
                    return "#ffdd7e";
                }
            }
            set { backgroundColor = value; }
        }


        private string stringDateTime;
        [Ignored]
        public string StringDateTime
        {
            get
            {
                return DateTime.ToString("dd.MM.yyyy HH:mm");
            }
            set { stringDateTime = value; }
        }
    }

    public class SendMessageModel
    {

        public SendMessageModel()
        {
            ContactsTo = new List<long>();
            ContactsCC = new List<long>();
            ContactsBCC = new List<long>();
            Attachments = new List<AttachmentModel>();
        }
        public bool IsDraft { get; set; }
        public string Subject { get; set; }
        public string MessageContent { get; set; }
        public DateTime DateTime { get; set; }
        public List<AttachmentModel> Attachments { get; set; }
        public List<long> ContactsTo { get; set; }
        public List<long> ContactsCC { get; set; }
        public List<long> ContactsBCC { get; set; }
    }
}