using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WifiScan.Droid.Helper;
using WifiScan.Interface;
using WifiScan.Model;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(WifiManagerDroid))]
namespace WifiScan.Droid.Helper
{
   public class WifiManagerDroid : IPlatformWifiManager
    {
        private Context context = null;
        private static WifiManager wifi;
        private WifiNetworkReceiver wifiReceiver;
        WifiManager wifiManager;
        LocationManager LocationManager;

        public WifiManagerDroid()
        {
            this.context = Android.App.Application.Context;
            wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            LocationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);
        }

        /// <summary>
		/// Get the list of wifi in droid
        /// Start a wifi scan and register the Broadcast receiver to get the list of available Wifi Networks
		/// </summary>
        public void RequestWifiNetworks()
        {
            wifiReceiver = new WifiNetworkReceiver();
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            wifi.StartScan();
        }
        public async Task<bool> ConnectWifi(string _ssid, string _pwd)
        {
            var ssid = $"\"{_ssid}\"";
            var pwd = $"\"{_pwd}\"";
            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = ssid;
            wifiConfig.PreSharedKey = pwd;

            int netId = wifiManager.AddNetwork(wifiConfig);
            wifiManager.Disconnect();
            wifiManager.EnableNetwork(netId, true);
            wifiManager.Reconnect();
            await Task.Delay(2 * 1000);

            if (wifiManager.ConnectionInfo?.SSID != ssid)
            {
                Console.WriteLine($"Cannot connect to network: {ssid}");
                return false;
            }
            return true;
        }

        public bool IsWifiEnabled()
        {
            return wifi.IsWifiEnabled;
        }

        public bool EnableWifi()
        {
            return wifi.SetWifiEnabled(true);
        }
        public bool DisableWifi()
        {
            return wifi.SetWifiEnabled(false);
        }

        /// <summary>
        /// To get Available wifi network , the location should be enabled in mobile.
        /// </summary>
        public void NavigateLocationSetting()
        {
            Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));
        }

        public bool IsLocationEnabled()
        {
            return LocationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        class WifiNetworkReceiver : BroadcastReceiver
        {
            public List<FormWifi> WiFiNetworks;

            /// <summary>
            /// Once the scan is completed, the OnReceive method will receive available WiFiNetworks
            /// </summary>
            /// <param name="context"></param>
            /// <param name="intent"></param>
            public override void OnReceive(Context context, Intent intent)
            {
                WiFiNetworks = new List<FormWifi>();
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    FormWifi wifi = new FormWifi();
                    wifi.name = wifinetwork.Ssid;
                    wifi.ipAdrs = wifinetwork.Bssid;
                    WiFiNetworks.Add(wifi);
                }
                var instance = WifiAdapter.Instance;
                if (instance != null)
                {
                    instance.OnReceiveAvailableNetworks(WiFiNetworks);
                }
            }
        }
    }
}