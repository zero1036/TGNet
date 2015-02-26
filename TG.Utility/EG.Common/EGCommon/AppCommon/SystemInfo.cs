using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace EG.Utility.AppCommon
{
    public class SystemInfo
    {       
        /// <summary>
        /// Get localhost IP address(IPv4)
        /// </summary>
        public static string GetIPAddress()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] ips = Dns.GetHostEntry(hostName).AddressList;
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    continue;
                return ip.ToString();
            }
            return string.Empty;
        }
    }
}
