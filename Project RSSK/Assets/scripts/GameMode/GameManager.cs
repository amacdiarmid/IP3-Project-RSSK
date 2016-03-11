using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : NetworkManager
{
    public ClientManager clManager;

    [HideInInspector]
    public string localPlayerName = "";
    [HideInInspector]
    public bool pickedTeam = true;

    short playerIdsGen = 0;

    public class Player
    {
        public string name;
        public int lives;
        public PlayerController controller;
        public Transform spawnPos;
        public NetworkConnection conn;
    }

    List<Player> players = new List<Player>();
    
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler(ChangeNameMsg.type, OnNameChanged);
        NetworkServer.RegisterHandler(PickedTeamMsg.type, OnClientPickedTeam);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Client Connected " + localPlayerName);
        pickedTeam = false;
        clManager.enabled = false;

        StringMessage msg = new StringMessage();
        msg.value = localPlayerName;

        ClientScene.AddPlayer(conn, 0, msg);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log("OnClientDisconnect: " + conn.address);
        players.RemoveAll(x => x.conn == conn);

        SendGameState();
    }

    void OnNameChanged(NetworkMessage netMsg)
    {
        Debug.Log("OnNameChanged: " + netMsg.conn.address);
        ChangeNameMsg msg = netMsg.ReadMessage<ChangeNameMsg>();

        /*Player p = new Player();
        p.name = msg.playerName;
        p.conn = netMsg.conn;
        p.lives = 5;
        players.Add(p);*/
        Debug.Log("Name: " + netMsg.conn);
        Player p = players.Find(x => x.controller.playerControllerId == msg.playerId);
        p.name = msg.playerName;

        SendGameState();
    }

    void OnClientPickedTeam(NetworkMessage netMsg)
    {
        PickedTeamMsg msg = netMsg.ReadMessage<PickedTeamMsg>();
        Player p = players.Find(x => x.controller.playerId == msg.playerId);
        p.controller.team = (PlayerTeam)msg.team;

        SendGameState();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader reader)
    {
        string playerName = reader.ReadString();
        bool team1 = players.Count % 2 == 0;
        string spawnTag = team1 ? "Spawn0" : "Spawn1";

        foreach (Transform spawn in startPositions)
        {
            if (spawn.tag == spawnTag && IsSpawnFree(spawn))
            {
                GameObject player = (GameObject)Instantiate(playerPrefab, spawn.position, Quaternion.identity);
                NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

                Player p = new Player();// players.Find(x => x.conn == conn);
                p.controller = player.GetComponent<PlayerController>();
                p.controller.playerId = playerIdsGen;
                p.spawnPos = spawn;
                p.conn = conn;
                Debug.Log("Add: " + conn);
                p.lives = 5;
                p.name = playerName;
                players.Add(p);

                playerIdsGen++;
                SendGameState();

                return;
            }
        }
    }

    bool IsSpawnFree(Transform spawn)
    {
        return true;
    }

    void SendGameState()
    {
        GameStateMsg stateMsg = new GameStateMsg();
        stateMsg.playerCount = players.Count;
        stateMsg.sceneName = networkSceneName;
        stateMsg.playerIds = new short[players.Count];
        stateMsg.playerNames = new string[players.Count];
        stateMsg.playerTeams = new byte[players.Count];
        for(int i=0; i< players.Count; i++)
        {
            stateMsg.playerIds[i] = players[i].controller.playerId;
            stateMsg.playerNames[i] = players[i].name;
            stateMsg.playerTeams[i] = (byte)players[i].controller.team;
        }
        NetworkServer.SendToAll(GameStateMsg.type, stateMsg);
    }
}
