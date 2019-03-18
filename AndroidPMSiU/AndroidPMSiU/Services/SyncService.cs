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

                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
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

                    return true;
                });            
        }
    }
}
