using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IPAddressFinder : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Connect at a give-my-public-ip website and get computer public ip

        string url = "http://checkip.dyndns.org/";

        WWW website = new WWW(url); // Go to website

        Text IPText = GetComponent<Text>();

        yield return website;

        // Print the IPadress
        IPText.text = website.text.Substring(64, website.text.Length - 64 - 16);
    }
}
