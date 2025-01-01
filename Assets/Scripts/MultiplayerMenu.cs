using Unity.Netcode;
using UnityEngine;
using TMPro; // For TextMeshPro

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText; // Drag your StatusText here in the Inspector

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        UpdateStatus("Hosting...");
    }

    public void StartClient()
    {
        if (!NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StartClient();
            UpdateStatus("Connecting as Client...");
        }
        else
        {
            UpdateStatus("Already connected as Client.");
        }
    }


    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        UpdateStatus("Server started.");
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
