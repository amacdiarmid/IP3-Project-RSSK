using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientManager : MonoBehaviour
{
    string nickname = "";
    string server = "";

    //using legacy for now
    void OnGUI()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 3;
        float height = 25;
        GUI.Box(new Rect(x - 50, y, 100, height), "Nickname");
        y += height;

        nickname = GUI.TextField(new Rect(x - 50, y, 100, height), nickname);
        y += height;

        GUI.Box(new Rect(x - 50, y, 100, height), "Server");
        y += height;

        server = GUI.TextField(new Rect(x - 50, y, 100, height), server);
        y += height;

        if (GUI.Button(new Rect(x - 50, y, 100, height), "Connect"))
            Connect(server);
        y += height;

        if (GUI.Button(new Rect(x - 50, y, 100, height), "Host"))
            Host();
    }

    public void Connect(string target)
    {
        if (target.Length == 0)
            target = "localhost";
        string[] parts = target.Split(':');
        string ip = parts[0];
        int port = parts.Length == 2 ? int.Parse(parts[1]) : 7777;
        NetworkManager manager = GetComponent<NetworkManager>();
        manager.networkAddress = ip;
        manager.networkPort = port;
        manager.StartClient();
        ((GameManager)NetworkManager.singleton).localPlayerName = nickname;
    }

    public void Host()
    {
        NetworkManager manager = GetComponent<NetworkManager>();
        manager.networkPort = 7777;
        manager.StartHost();
        ((GameManager)NetworkManager.singleton).localPlayerName = nickname;
    }
}
