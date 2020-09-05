using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WifiScan
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            init();
        }
        async void init()
        {
            WifiAdapter wifiAdapter = WifiAdapter.Instance;
            wifiAdapter.Init();
            wifiAdapter.InvokeResult += WifiAdapter_InvokeResult;

            
        }

        private void WifiAdapter_InvokeResult(List<Model.FormWifi> wifi)
        {
          //  throw new NotImplementedException();
        }
    }
}
