using AndroidPMSiU.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidPMSiU.Services.Realms
{
    class RealmMessageService
    {

        public static int RECIVE_MESSAGE_TYPE = 1;
        public static int SENT_MESSAGE_TYPE = 2;
        public static int DRAFT_MESSAGE_TYPE = 3;

        public static void ClearData()
        {
            var realm = Realm.GetInstance();

            realm.Write(() =>
            {

                realm.RemoveAll();
            });
        }

        public static bool InsertData(DataModel dataModel, bool clearDataDatabase = true)
        {
            try
            {
    
                var realm = Realm.GetInstance();

                realm.Write(() =>
                {
                    if (clearDataDatabase)
                    {
                        realm.RemoveAll();
                    }

                    var allMessages = new List<MessageModel>();


                    foreach (var item in dataModel.Received)
                    {
                        item.Type = RECIVE_MESSAGE_TYPE;
                        allMessages.Add(item);
                    }

                    foreach (var item in dataModel.Sent)
                    {
                        item.Type = SENT_MESSAGE_TYPE;
                        allMessages.Add(item);
                    }

                    foreach (var item in dataModel.Draft)
                    {
                        item.Type = DRAFT_MESSAGE_TYPE;
                        allMessages.Add(item);
                    }

                    foreach (var item in allMessages)
                    {
                        realm.Add(item);
                    }

                    foreach (var item in dataModel.Contacts)
                    {
                        realm.Add(item);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<MessageModel> GetMessagesByType(int messageType)
        {
            try
            {
                var realm = Realm.GetInstance();
                
                var messages = realm.All<MessageModel>().Where(x => x.Type == messageType).ToList().OrderByDescending(x => x.DateTime).ToList();
                foreach (var item in messages)
                {
                    if (item.ContactsTo == null)
                    {
                        item.ContactsTo = new List<ContactModel>();
                    }
                    item.ContactsTo.AddRange(GetContactsById(item.ContactsToIds.ToList()));
                    item.StringContactsTo = string.Join("; ", item.ContactsTo.Select(x => x.DisplayName));

                    if (item.ContactsCC == null)
                    {
                        item.ContactsCC = new List<ContactModel>();
                    }
                    item.ContactsCC.AddRange(GetContactsById(item.ContactsCCIds.ToList()));                   

                    if (item.ContactsBCC == null)
                    {
                        item.ContactsBCC = new List<ContactModel>();
                    }
                    item.ContactsBCC.AddRange(GetContactsById(item.ContactsBCCIds.ToList()));

                    if (item.Tags == null)
                    {
                        item.Tags = new List<TagModel>();
                    }

                    item.Tags.AddRange(GetTagsById(item.TagIds.ToList()));

                }
                return messages;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static IDisposable OnNewMessageInterface(Action doOnNewMessage)
        {
            var realm = Realm.GetInstance();
            var realmToken = realm.All<MessageModel>().Where(x => x.Type == RECIVE_MESSAGE_TYPE).SubscribeForNotifications((sender, change, error) => 
            {
                doOnNewMessage();
            });
            return realmToken;
        }

        public static bool InsertMessages (MessageModel messageModel)
        {
            try
            {
                var realm = Realm.GetInstance();

                realm.Write(() =>
                {
                    realm.Add(messageModel, true);
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CheckReadedMesssages(MessageModel messageModel)
        {
            var realm = Realm.GetInstance();
            try
            {
                using (var trans = realm.BeginWrite())
                {
                    messageModel.IsRead = true;
                    trans.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void DeleteMessage(long deleteById)
        {
            var realm = Realm.GetInstance();
            var deleteMessage = realm.All<MessageModel>().First(x => x.Id == deleteById);
            try
            {
                using (var trans = realm.BeginWrite())
                {
                    realm.Remove(deleteMessage);
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<AccountModel> GetAllAccounts()
        {
            try
            {
                var realm = Realm.GetInstance();
                var contacts = realm.All<AccountModel>()
                    .ToList();
                return contacts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContactModel> GetAllContacts()
        {
            try
            {
                var realm = Realm.GetInstance();
                var contacts = realm.All<ContactModel>()
                    .ToList();
                return contacts;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ContactModel GetContactById(long id)
        {
            try
            {
                var realm = Realm.GetInstance();
                var contacts = realm.All<ContactModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                return contacts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContactModel> GetContactsById(List<long> ids)
        {
            try
            {
                var realm = Realm.GetInstance();
                var contacts = realm.All<ContactModel>()
                    .ToList()
                    .Where(x => ids.Contains(x.Id))                  
                    .ToList();
                return contacts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool InsertContact(ContactModel contactModel)
        {
            try
            {
                var realm = Realm.GetInstance();

                realm.Write(() =>
                {

                    realm.Add(contactModel, true);

                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static List<TagModel> GetTagsById(List<long> ids)
        {
            try
            {
                var realm = Realm.GetInstance();
                var tags = realm.All<TagModel>()
                    .ToList()
                    .Where(x => ids.Contains(x.Id))
                    .ToList();
                return tags;



            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
