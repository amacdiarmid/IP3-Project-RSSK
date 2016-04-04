using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ClientManager : MonoBehaviour
{
    string nickname = "";
    string server = "";
	Utils.Servers info = null;

	void Start()
	{
		Refresh ();
	}

    //using legacy for now
    void OnGUI()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 5;
        float height = 25;

		//client gui for connecting or hosting
        GUI.Box(new Rect(x - 100, y, 100, height), "Nickname");
        nickname = GUI.TextField(new Rect(x, y, 100, height), nickname);
        y += height;

        GUI.Box(new Rect(x - 100, y, 100, height), "Server");
        server = GUI.TextField(new Rect(x, y, 100, height), server);
        y += height;

        if (GUI.Button(new Rect(x - 150, y, 100, height), "Connect"))
            Connect(server);

        if (GUI.Button(new Rect(x - 50, y, 100, height), "Host"))
            Host();

		if (GUI.Button (new Rect (x + 50, y, 100, height), "Refresh"))
			Refresh ();
		y += height;
		
		//now to draw the server browser stuff
		if (info.servers != null) 
		{
			for (int i = 0; i < info.servers.Length; i++) 
			{
				if(GUI.Button(new Rect(x - 150, y, 300, height), info.servers[i].ToString()))
					Connect(info.servers[i].target);
				y += height;
			}
		} 
		else
			GUI.Box (new Rect (x - 150, y, 300, height), "No servers available");
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

	public void Refresh()
	{
		info = Utils.GetServers ();
	}
}
