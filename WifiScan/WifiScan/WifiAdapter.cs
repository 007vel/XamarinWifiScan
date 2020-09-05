using System;
using System.Collections.Generic;
using System.Text;
using WifiScan.Interface;
using WifiScan.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WifiScan
{
   public class WifiAdapter
    {
        public delegate void ActionResult(List<FormWifi> wifi);

        public event ActionResult InvokeResult;

        static WifiAdapter instacne = null;
        public static WifiAdapter Instance
        {
            get
            {
                if(instacne==null)
                {
                    instacne = new WifiAdapter();
                }
                return instacne;
            }
        }
        private WifiAdapter()
        {
        }
       public async void Init()
        {
            var locationPermission = await PermissionUtil.Instance.CheckPermissionAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
            if (locationPermission == PermissionStatus.Granted)
            {
                if (!FormWifiManager.IsWifiEnabled())
                {
                    var popupres = await App.Current.MainPage.DisplayAlert(title: "Use Wifi?", message: "The app wants to turn on your device wifi", "YES", "NO");
                    if (popupres)
                    {
                        FormWifiManager.EnableWifi();
                        
                        if(!FormWifiManager.IsLocationEnabled())
                        {
                            var res=await App.Current.MainPage.DisplayAlert(title: "Use Location?", message: "The app wants to turn on your device GPS", "YES", "NO");
                            if(res)
                            {
                                FormWifiManager.NavigateLocationSetting();
                            }                            
                        }                        
                        FormWifiManager.RequestWifiNetworks();
                    }
                }
            }
        }
        public void OnReceiveAvailableNetworks(List<FormWifi> wifiList)
        {
            InvokeResult.Invoke(wifiList);
        }

        IPlatformWifiManager formWifiManager;
        public IPlatformWifiManager FormWifiManager
        {
            get
            {
                if (formWifiManager == null)
                {
                    formWifiManager = DependencyService.Get<IPlatformWifiManager>();
                }
                return formWifiManager;
            }
        }

        public void OnRequestAvailableNetworks()
        {
            FormWifiManager.RequestWifiNetworks();
        }
    }
}
