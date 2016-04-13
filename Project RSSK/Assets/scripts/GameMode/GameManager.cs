using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class GameManager : NetworkManager
{
	#region ClientVars
	public ClientManager clManager;
	public CapturePoint capturePoint;
	public int prepSeconds = 10;
	public int roundSeconds = 180;
	private int CurTime = 300;

	[HideInInspector]
	public string localPlayerName = "";

	public GameObject canvas;
	public Button yellowBtn, blueBtn;
	string status = "";
	byte pickCharacter = 6;
	byte pickedTeam = 0;
	string timeoutText;
	#endregion

	#region ServerVars
	public class Player
	{
		public bool picked;
		public bool waitingForRound;
		public string name;
		public byte lives = 5;
		public byte character = 6;
		public PlayerTeam team = PlayerTeam.NotPicked;
		public PlayerController controller = null;
		public Vector3 spawnPos;
		public NetworkConnection conn;
	}

	PlayerTeam attackingTeam = PlayerTeam.TeamYellow;
	byte roundsToPlay = 5;
	bool runningRound = false;
	byte[] score = { 0, 0 };
	public List<GameObject> characters;
	List<Player> players = new List<Player>();
	short playerIdsGen = 0;
	#endregion

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Cursor.visible = !Cursor.visible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;

			Transform status = canvas.transform.Find("GameStatus");
			foreach(Transform child in status)
				child.gameObject.SetActive(!child.gameObject.activeSelf);

			//hide/show game status info 
			Text text = canvas.transform.Find("GameStatus").GetComponent<Text>();
			text.enabled = !text.enabled;
		}
	}

	public override void OnStartServer()
	{
		base.OnStartServer();

		NetworkServer.RegisterHandler(ChangeNameMsg.type, OnNameChanged);
		NetworkServer.RegisterHandler(PickedTeamMsg.type, OnPickedTeam);
		Utils.SendServerUpdate (networkPort, 0, networkSceneName);

		StartCoroutine(PrepTimer(prepSeconds));
	}

	public override void OnStopServer ()
	{
		base.OnStopServer ();

		Utils.SendServerStop (networkPort);
	}

	void OnApplicationQuit()
	{
		Utils.SendServerStop (networkPort);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);

		conn.RegisterHandler(GameStateMsg.type, GameStateResponse);
		conn.RegisterHandler((short)MsgTypes.PrepTimeStart, StartPrepTimer);
		conn.RegisterHandler((short)MsgTypes.RoundStart, StartRoundTimer);

		Debug.Log("Client Connected " + localPlayerName);
		clManager.enabled = false;

		StringMessage msg = new StringMessage();
		msg.value = localPlayerName;

		ClientScene.AddPlayer(conn, 0, msg);
		canvas.SetActive(true);
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect(conn);
		Debug.Log("OnServerDisconnect: " + conn.address);
		players.RemoveAll(x => x.conn == conn);
		NetworkServer.DestroyPlayersForConnection(conn);
		SendGameState();
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

		Vector3 spawn = PickSpawnPoint((PlayerTeam)msg.team);
		GameObject newPlayer = (GameObject)Instantiate(characters[msg.character], spawn, Quaternion.identity);
		p.controller = newPlayer.GetComponent<PlayerController>();
		p.controller.controllable = runningRound;
		p.team = p.controller.team = (PlayerTeam)msg.team;
		p.controller.id = msg.playerId;
		p.character = msg.character;
		p.picked = true;
		NetworkServer.ReplacePlayerForConnection(netMsg.conn, newPlayer, msg.playerId);
		p.controller.RpcLockCursor(true);

		SendGameState();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader reader)
	{
		playerControllerId = playerIdsGen++;
		string playerName = reader.ReadString();

		Vector3 spawnPos = transform.position;
		GameObject player = (GameObject)Instantiate(playerPrefab, spawnPos, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		Player p = new Player();
		p.controller = player.GetComponent<PlayerController>();
		p.controller.id = playerControllerId;
		p.controller.RpcLockCursor(false);
		p.spawnPos = spawnPos;
		p.conn = conn;
		p.name = playerName;
		players.Add(p);

		Utils.SendServerUpdate (networkPort, players.Count, networkSceneName);
		
		SendGameState();
	}

	int[] oldSpawns = { 0, 0 };
	Vector3 PickSpawnPoint(PlayerTeam team)
	{
		if (team == PlayerTeam.NotPicked)
			return transform.position;

		string spawnTag = team == attackingTeam ? "SpawnAttack" : "SpawnDefend";
		List<Transform> spawns = startPositions.FindAll(x => x.tag == spawnTag);
		int spawnToUse = oldSpawns[(int)team - 1] + 1;
		if (spawnToUse == spawns.Count)
			spawnToUse = 0;
		while (spawnToUse != oldSpawns[(int)team - 1])
		{
			Transform spawn = spawns[spawnToUse];
			if (IsSpawnFree(spawn))
			{
				oldSpawns[(int)team - 1] = spawnToUse;
				return spawn.position;
			}
			else if (++spawnToUse == spawns.Count)
			   spawnToUse = 0;
		}

		return transform.position;
	}

	bool IsSpawnFree(Transform spawn)
	{
		return true;
	}

	void SendGameState()
	{
		GameStateMsg stateMsg = new GameStateMsg();
		stateMsg.playerCount = players.Count;
		stateMsg.yellowAttacking = attackingTeam == PlayerTeam.TeamYellow;
		stateMsg.score = score;
		stateMsg.sceneName = networkSceneName;
		stateMsg.playerIds = new short[players.Count];
		stateMsg.playerNames = new string[players.Count];
		stateMsg.playerLives = new byte[players.Count];
		stateMsg.playerTeams = new byte[players.Count];
		for(int i=0; i< players.Count; i++)
		{
			stateMsg.playerIds[i] = players[i].controller.id;
			stateMsg.playerNames[i] = players[i].name;
			stateMsg.playerLives[i] = players[i].lives;
			stateMsg.playerTeams[i] = (byte)players[i].team;
		}
		NetworkServer.SendToAll(GameStateMsg.type, stateMsg);
	}

	void GameStateResponse(NetworkMessage inMsg)
	{
		Debug.Log("Received state");
		GameStateMsg msg = inMsg.ReadMessage<GameStateMsg>();
		status = string.Format("Attackers {0}-{1} Defenders\nPlayers:\n", msg.score[0], msg.score[1]);
		int playerCount = msg.playerCount;
		int[] teams = { 0, 0 };
		for (int i = 0; i < playerCount; i++)
		{
			status += msg.playerNames[i] + " - " + (PlayerTeam)msg.playerTeams[i] + " (" + msg.playerLives[i] + ")\n";

			//count only players which picked a team and not us (so that we don't our current choice doesn't let pick a team in case it's a N-N case)
			if ((PlayerTeam)msg.playerTeams[i] != PlayerTeam.NotPicked && !msg.playerNames[i].Equals(localPlayerName))
				teams[msg.playerTeams[i] - 1]++;
		}
		yellowBtn.interactable = teams[0] <= teams[1];
		blueBtn.interactable = teams[0] >= teams[1];
	}

	public void PickedCharacter(int character)
	{
		pickCharacter = (byte)character;
		canvas.transform.Find("GameStatus/TeamHolder").gameObject.SetActive(true);

		if (pickedTeam != 0)
			PickedTeam (pickedTeam);
	}

	public void PickedTeam(int team)
	{
		PickedTeamMsg msg = new PickedTeamMsg();
		msg.playerId = PlayerController.localInstance.id;
		msg.character = pickCharacter;
		msg.team = pickedTeam = (byte)team;
		client.Send(PickedTeamMsg.type, msg);
		Transform status = canvas.transform.Find("GameStatus");
		foreach(Transform child in status)
			child.gameObject.SetActive(false);
	}

	void StartPrepTimer(NetworkMessage netMsg)
	{
		IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
		int secs = msg.value;
		CurTime = secs;
		timeoutText = string.Format("\nPrep Left: {0}s", secs);
		if (PlayerController.localInstance)
			PlayerController.localInstance.SetGameInfo (status + timeoutText);
	}

	void StartRoundTimer(NetworkMessage netMsg)
	{
		IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
		int secs = msg.value;
		CurTime = secs;
		timeoutText = string.Format("\nTime Left: {0}s", secs);
		if (PlayerController.localInstance)
			PlayerController.localInstance.SetGameInfo (status + timeoutText);
	}

	#region GameModeProgression
	public void OnPlayerDied(GameObject go)
	{
		short id = go.GetComponent<PlayerController>().id;
		Player p = players.Find(x => x.controller.id == id);
		NetworkServer.Destroy(go);

		Vector3 spawn = PickSpawnPoint(p.team);
		GameObject newPlayer = (GameObject)Instantiate(characters[p.character], spawn, Quaternion.identity);
		p.controller = newPlayer.GetComponent<PlayerController>();
		p.controller.team = p.team;
		p.controller.id = id;
		p.waitingForRound = --p.lives == 0;
		p.controller.controllable = !p.waitingForRound;
		NetworkServer.ReplacePlayerForConnection(p.conn, newPlayer, 0);

		SendGameState();

		//check if all spent their lives
		List<Player> activeTeammates = players.FindAll(x => x.team == p.team && !x.waitingForRound);
		if (activeTeammates.Count == 0)
			Win(p.team.Enemy());
	}

	public void OnPointCaptured()
	{
		Win(attackingTeam);
	}

	IEnumerator PrepTimer(int seconds)
	{
		runningRound = false;
		while(--seconds > 0)
		{
			IntegerMessage msg = new IntegerMessage();
			msg.value = seconds;
			NetworkServer.SendToAll((short)MsgTypes.PrepTimeStart, msg);
			yield return new WaitForSeconds(1);
		}
		foreach (Player p in players)
		{
			p.waitingForRound = false;
			p.controller.controllable = true;
		}
		
		StartCoroutine(RoundTimer(roundSeconds));
	}

	IEnumerator RoundTimer(int seconds)
	{
		int currRoundsLeft = roundsToPlay;
		runningRound = true;
		while(--seconds > 0)
		{
			IntegerMessage msg = new IntegerMessage();
			msg.value = seconds;
			NetworkServer.SendToAll((short)MsgTypes.RoundStart, msg);
			if (currRoundsLeft == roundsToPlay)
				yield return new WaitForSeconds(1);
			else
				yield return null;
		}
		
		if (currRoundsLeft == roundsToPlay) //defenders won
			Win(attackingTeam.Enemy());
	}

	public void Win(PlayerTeam winners)
	{
		//updating the score and switching the teams
		score[(byte)winners - 1]++;
		capturePoint.owner = attackingTeam;
		attackingTeam = attackingTeam.Enemy();
		roundsToPlay--;

		//respawning everyone
		foreach (Player p in players)
		{
			short id = p.controller.id;
			NetworkServer.Destroy(p.controller.gameObject);

			Vector3 spawn = PickSpawnPoint(p.team);
			GameObject newPlayer = (GameObject)Instantiate(characters[p.character], spawn, Quaternion.identity);
			p.controller = newPlayer.GetComponent<PlayerController>();
			p.controller.team = p.team;
			p.controller.id = id;
			p.waitingForRound = true;
			p.lives = 5;
			p.controller.controllable = !p.waitingForRound;
			NetworkServer.ReplacePlayerForConnection(p.conn, newPlayer, 0);
		}

		SendGameState();

		if(roundsToPlay > 0)
			StartCoroutine(PrepTimer(prepSeconds));
	}
	#endregion

	public byte getScore(int i)
	{
		return score[i];
	}

	public int getCurTime()
	{
		//Debug.Log("grab time " + CurTime);
		return CurTime;
	}
}
