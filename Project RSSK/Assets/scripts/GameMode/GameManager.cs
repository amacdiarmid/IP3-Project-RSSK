using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkManager
{
    public struct Player
    {
        public int lives;
        public PlayerController controller;
        public Transform spawnPos;
        public NetworkConnection conn;
    }

    List<Player> players = new List<Player>();
    Transform debugRoot = null;

    public override void OnStartServer()
    {
        base.OnStartServer();
        DebugMsg("Spawn pos count: " + startPositions.Count);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        DebugMsg("OnClientConnect: " + conn.address);
        ClientScene.AddPlayer(0);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        DebugMsg("OnClientDisconnect: " + conn.address);
        players.RemoveAll(x => x.conn == conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        DebugMsg("OnServerAddPlayer: " + playerControllerId);

        bool team1 = players.Count % 2 == 0;
        string spawnTag = team1 ? "Spawn0" : "Spawn1";

        foreach (Transform spawn in startPositions)
        {
            if (spawn.tag == spawnTag && IsSpawnFree(spawn))
            {
                GameObject player = (GameObject)Instantiate(playerPrefab, spawn.position, Quaternion.identity);
                NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

                Player p = new Player() { controller = player.GetComponent<PlayerController>(), lives = 5, spawnPos = spawn, conn = conn };
                p.controller.RpcSpawned(spawn.position, team1 ? 0 : 1);
                players.Add(p);

                DebugMsg("Spawned for " + conn.address);
                DebugMsg("Pos: " + player.transform.position);
                return;
            }
        }
    }

    bool IsSpawnFree(Transform spawn)
    {
        return true;
    }

    void DebugMsg(string msg)
    {
        Debug.Log(msg);
        if(debugRoot == null)
        {
            GameObject root = new GameObject("DebugMsgs");
            debugRoot = root.transform;
        }

        GameObject go = new GameObject(msg);
        go.transform.SetParent(debugRoot);
        go.transform.SetAsFirstSibling();
    }
}
