using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class Utils
{
	static string masterServer = "http://5.9.251.204/server.php";

	public static PlayerTeam Enemy(this PlayerTeam team)
	{
		return team == PlayerTeam.TeamYellow ? PlayerTeam.TeamBlue : PlayerTeam.TeamYellow;
	}

	[System.Serializable]
	public class Servers
	{
		[System.Serializable]
		public class Server
		{
			public string target;
			public string serverInfo;

			public override string ToString ()
			{
				return target + " - " + serverInfo;
			}
		}

		public Server[] servers;
	}

	public static Servers GetServers()
	{
		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (masterServer);
		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		string data = "";
		using (StreamReader reader = new StreamReader (response.GetResponseStream ()))
			data = reader.ReadToEnd ();
		response.Close ();
		Debug.Log(data);
		return JsonUtility.FromJson<Servers>(data);
	}

	[System.Serializable]
	class Data
	{
		public string action;
		public string port;
		public string serverInfo;

		public Data(string action, string port, string serverInfo)
		{
			this.action = action;
			this.port = port;
			this.serverInfo = serverInfo;
		}
	}

	public static void SendServerUpdate(int port, int players, string map)
	{
		Data d = new Data("add", port.ToString(), map + " | " + players + "/6");
		string jsonData = JsonUtility.ToJson (d);
		byte[] data = Encoding.ASCII.GetBytes(jsonData);
		Debug.Log ("POST " + jsonData);

		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (masterServer);
		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = data.Length;
		Stream stream = request.GetRequestStream ();
		stream.Write (data, 0, data.Length);
		stream.Close ();

		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		using (StreamReader reader = new StreamReader (response.GetResponseStream ()))
			Debug.Log(reader.ReadToEnd ());
		response.Close ();
	}

	public static void SendServerStop(int port)
	{
		Data d = new Data ("delete", port.ToString(), null);
		string jsonData = JsonUtility.ToJson(d);
		byte[] data = Encoding.ASCII.GetBytes(jsonData);
		Debug.Log ("POST " + jsonData);

		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (masterServer);
		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = data.Length;
		Stream stream = request.GetRequestStream ();
		stream.Write (data, 0, data.Length);
		stream.Close ();

		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		using (StreamReader reader = new StreamReader (response.GetResponseStream ()))
			Debug.Log(reader.ReadToEnd ());
		response.Close ();
	}
}
