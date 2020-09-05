using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WifiScan.Model;

namespace WifiScan.Interface
{
    public interface IPlatformWifiManager
    {
        void RequestWifiNetworks();
        Task<bool> ConnectWifi(string ssid, string pwd);

        bool IsWifiEnabled();

        bool EnableWifi();

        bool DisableWifi();

        void NavigateLocationSetting();

        bool IsLocationEnabled();
    }
}
