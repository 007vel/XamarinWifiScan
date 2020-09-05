using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace WifiScan
{
    public class PermissionUtil
    {
        private PermissionUtil() { }
        static PermissionUtil permissionUtil = null;
        public static PermissionUtil Instance
        {
            get
            {
                if (permissionUtil == null)
                {
                    permissionUtil = new PermissionUtil();
                }
                return permissionUtil;
            }
        }
        public async Task<PermissionStatus> CheckPermissionAndRequestPermissionAsync<T>(T permissionReq)
             where T : BasePermission
        {
            var status = await permissionReq.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permissionReq.RequestAsync();
            }
            return status;
        }
    }
}
