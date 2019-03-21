using AndroidPMSiU.Services.Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidPMSiU.Services
{
    public class SyncService
    {
        public static void TryToSync()
        {

            int syncInterval = SettingsService.GetSyncTime();

                Device.StartTimer(TimeSpan.FromSeconds(syncInterval), () =>
                {
                    if (AuthenticationService.GetToken() == null)
                    {
                        return false;
                    }

                    Task.Run(async() => 
                    {
                        var data = await DataService.GetData(AuthenticationService.GetSyncTime());
                        if (data.Contacts.Any() || data.Draft.Any() || data.Received.Any() || data.Sent.Any())
                        {
                            AuthenticationService.InsertSyncTime();
                            RealmMessageService.InsertData(data, false);
                            
                        }
                        });


                    if (syncInterval != SettingsService.GetSyncTime())
                    {
                        TryToSync();
                        return false;
                    }
                    return true;
                });            
        }
    }
}
