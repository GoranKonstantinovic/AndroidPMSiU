using AndroidPMSiU.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AndroidPMSiU.Services
{
    class SettingsService
    {
        public static void SaveSyncTime(int time)
        {
            App.Current.Properties["time"] = time;
        }

        public static int GetSyncTime()
        {
            return App.Current.Properties.ContainsKey("time") ? (int)App.Current.Properties["time"] : 10 ;
        }
    }
}
