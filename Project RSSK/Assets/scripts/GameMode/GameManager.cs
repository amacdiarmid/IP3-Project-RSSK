using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : NetworkManager
{
    public ClientManager clManager;
    public List<GameObject> characters;

    [HideInInspector]
    public string localPlayerName = "";
    [HideInInspector]
    public bool pickedTeam = true;

    bool canPickT1 = false;
    bool canPickT2 = false;
    bool drawMenu = false;

    short playerIdsGen = 0;
    string playersActive = "";

    public class Player
    {
        public string name;
        public int lives = 5;
        public byte character = 6;
        public PlayerTeam team = PlayerTeam.NotPicked;
        public PlayerController controller = null;
        public Vector3 spawnPos;
        public NetworkConnection conn;
    }

    List<Player> players = new List<Player>();
    byte pickCharacter = 6;

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 100, 0, 200, 100), playersActive);

        if (drawMenu)
        {
            float indWidth = 75;
            for (int i = 0; i < 6; i++)
                if (GUI.Button(new Rect(Screen.width / 2 - indWidth * 3 + indWidth * i, 100, indWidth, 25), i.ToString()))
                    pickCharacter = (byte)i;

            if ((pickCharacter != 6) && !pickedTeam)
            {
                if (canPickT1 && GUI.Button(new Rect(Screen.width / 2 - 50, 125, 50, 25), "Team Yellow"))
                    PickedTeam(PlayerTeam.TeamYellow);
                if (canPickT2 && GUI.Button(new Rect(Screen.width / 2, 125, 50, 25), "Team Blue"))
                    PickedTeam(PlayerTeam.TeamBlue);
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler(ChangeNameMsg.type, OnNameChanged);
        NetworkServer.RegisterHandler(PickedTeamMsg.type, OnPickedTeam);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        conn.RegisterHandler(GameStateMsg.type, GameStateResponse);

        Debug.Log("Client Connected " + localPlayerName);
        pickedTeam = false;
        clManager.enabled = false;

        StringMessage msg = new StringMessage();
        msg.value = localPlayerName;

        drawMenu = true;
        ClientScene.AddPlayer(conn, 0, msg);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log("OnClientDisconnect: " + conn.address);
        drawMenu = false;
    }

    void OnNameChanged(NetworkMessage netMsg)
    {
        Debug.Log("OnNameChanged: " + netMsg.conn.address);
        ChangeNameMsg msg = netMsg.ReadMessage<ChangeNameMsg>();

        Player p = players.Find(x => x.controller.playerControllerId == msg.playerId);
        p.name = msg.playerName;

        SendGameState();
    }

    void OnPickedTeam(NetworkMessage netMsg)
    {
        PickedTeamMsg msg = netMsg.ReadMessage<PickedTeamMsg>();
        Player p = players.Find(x => x.controller.id == msg.playerId);

        NetworkServer.Destroy(p.controller.gameObject);

        Vector3 spawn = PickSpawnPoint(msg.team == 1 ? "Spawn0" : "Spawn1");
        GameObject newPlayer = (GameObject)Instantiate(characters[msg.character], spawn, Quaternion.identity);
        p.controller = newPlayer.GetComponent<PlayerController>();
        p.team = p.controller.team = (PlayerTeam)msg.team;
        p.controller.id = msg.playerId;
        p.character = msg.character;
        NetworkServer.ReplacePlayerForConnection(netMsg.conn, newPlayer, 0);

        SendGameState();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader reader)
    {
        string playerName = reader.ReadString();
        bool team1 = players.Count % 2 == 0;

        Vector3 spawnPos = PickSpawnPoint(team1 ? "Spawn0" : "Spawn1");
        GameObject player = (GameObject)Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        Player p = new Player();
        p.controller = player.GetComponent<PlayerController>();
        p.controller.id = playerIdsGen;
        p.spawnPos = spawnPos;
        p.conn = conn;
        p.name = playerName;
        players.Add(p);

        playerIdsGen++;
        SendGameState();
    }

    Vector3 PickSpawnPoint(string tag)
    {
        foreach (Transform spawn in startPositions)
            if (spawn.tag == tag && IsSpawnFree(spawn))
                return spawn.position;
        return Vector3.zero;
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
            stateMsg.playerIds[i] = players[i].controller.id;
            stateMsg.playerNames[i] = players[i].name;
            stateMsg.playerTeams[i] = (byte)players[i].team;
        }
        NetworkServer.SendToAll(GameStateMsg.type, stateMsg);
    }

    void GameStateResponse(NetworkMessage inMsg)
    {
        Debug.Log("Received state");
        playersActive = "Players ingame:\n";
        GameStateMsg msg = inMsg.ReadMessage<GameStateMsg>();
        int playerCount = msg.playerCount;
        int[] teams = { 0, 0 };
        for (int i = 0; i < playerCount; i++)
        {
            playersActive += msg.playerNames[i] + " - " + (PlayerTeam)msg.playerTeams[i] + "\n";

            if ((PlayerTeam)msg.playerTeams[i] != PlayerTeam.NotPicked)
                teams[msg.playerTeams[i] - 1]++;
        }
        canPickT1 = teams[0] <= teams[1];
        canPickT2 = teams[0] >= teams[1];
    }

    void PickedTeam(PlayerTeam team)
    {
        pickedTeam = true;
        PickedTeamMsg msg = new PickedTeamMsg();
        msg.playerId = PlayerController.localInstance.id;
        msg.character = pickCharacter;
        msg.team = (byte)team;
        client.Send(PickedTeamMsg.type, msg);
    }
}
