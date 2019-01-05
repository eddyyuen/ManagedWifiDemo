using NativeWifi;
using System;
using System.Text;
using static NativeWifi.Wlan;

namespace WifiExample
{
    class Program
    {
        /// <summary>
        /// Converts a 802.11 SSID to a string.
        /// </summary>
        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString( ssid.SSID, 0, (int) ssid.SSIDLength );
        }

        static void Main( string[] args )
        {
            string SSID = "Krspace-Member";
            WlanClient client = new WlanClient();
            foreach ( WlanClient.WlanInterface wlanIface in client.Interfaces )
            {
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList( 0 );
                foreach ( Wlan.WlanAvailableNetwork network in networks )
                {
                   // if ( network.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.WPA_UseGroup )
                    {
                        //Console.WriteLine( "Found WEP network with SSID {0}.", GetStringForSSID(network.dot11Ssid));

                       // Console.WriteLine("dot11DefaultAuthAlgorithm {0}.", network.dot11DefaultAuthAlgorithm.ToString());
                        if(GetStringForSSID(network.dot11Ssid) == SSID && wlanIface.CurrentConnection.isState == WlanInterfaceState.Connected)
                        {
                            Console.WriteLine("{0} is connected",SSID);
                        }
                        
                    }
                }
                wlanIface.DeleteProfile("DM PHONE");
                // Retrieves XML configurations of existing profiles.
                // This can assist you in constructing your own XML configuration
                // (that is, it will give you an example to follow).
                foreach ( Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles() )
                {
                    string name = profileInfo.profileName; // this is typically the network's SSID
                    string xml = wlanIface.GetProfileXml( profileInfo.profileName );
                    // Console.WriteLine("{0} - {1}", name,xml);
                    if (name == "DM PHONE")
                    {
                        Console.WriteLine("{0} - {1}", name, xml);
                    }
                }

                // Connects to a known network with WEP security
                // string profileName = "Cheesecake"; // this is also the SSID
                //string mac = "52544131303235572D454137443638";
                // string key = "hello";
                //string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><MSM><security><authEncryption><authentication>open</authentication><encryption>WEP</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>networkKey</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey><keyIndex>0</keyIndex></security></MSM></WLANProfile>", profileName, mac, key);
                // wlanIface.SetProfile( Wlan.WlanProfileFlags.AllUser, profileXml, true );

                string xmls = string.Format(@"<?xml version=""1.0""?>
<WLANProfile xmlns = ""http://www.microsoft.com/networking/WLAN/profile/v1"">
        <name>DM PHONE</name>
        <SSIDConfig>
                <SSID>
                        
                        <name>DM PHONE</name>
                </SSID>
        </SSIDConfig>
        <connectionType>ESS</connectionType>
        <connectionMode>auto</connectionMode>
        <MSM>
                <security>
                        <authEncryption>
                                <authentication>WPA2PSK</authentication>
                                <encryption>AES</encryption>
                                <useOneX>false</useOneX>
                        </authEncryption>
                        <sharedKey>
                                <keyType>passPhrase</keyType>
                                <protected>false</protected>
                                <keyMaterial>{1}</keyMaterial>
                        </sharedKey>
                </security>
        </MSM>
</WLANProfile>", "DM PHONE","11223355");
                wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, xmls, true);
             
                wlanIface.Connect( Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, "DM PHONE" );

                


            }
            Console.ReadLine();
        }

        private static void WlanIface_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
        
        }
    }
}
