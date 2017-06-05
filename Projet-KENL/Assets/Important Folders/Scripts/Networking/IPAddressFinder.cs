using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class IPAddressFinder : MonoBehaviour
{
    private void Start()
    {
        Text IPText = GetComponent<Text>();
        IPText.text = "Local IP Address: " + GetLocalIPAddress();
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }

        return "Not found";
    }
}
