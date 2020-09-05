using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WifiScan.Model
{
    public class FormWifi : INotifyPropertyChanged
    {
        public string ssid { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string ipAdrs { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
