using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PickTeam : MonoBehaviour
{
    bool canPickT1 = false;
    bool canPickT2 = false;
    string playersActive = "";
    NetworkClient client;
    GameManager manager;

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 100), playersActive);
        if (!manager.pickedTeam)
        {
            if (canPickT1 && GUI.Button(new Rect(0, 100, 50, 25), "Team Yellow"))
                PickedTeam(PlayerTeam.TeamYellow);
            if (canPickT2 && GUI.Button(new Rect(50, 100, 50, 25), "Team Blue"))
                PickedTeam(PlayerTeam.TeamBlue);
        }
    }

    public void Init(string playerName, NetworkClient client)
    {
        manager = (GameManager)NetworkManager.singleton;
        manager.localPlayerName = playerName;
        this.client = client;
        client.RegisterHandler(GameStateMsg.type, GameStateResponse);
    }

    void GameStateResponse(NetworkMessage inMsg)
    {
        Debug.Log("Received state");
        playersActive = "";
        GameStateMsg msg = inMsg.ReadMessage<GameStateMsg>();
        int playerCount = msg.playerCount;
        int[] teams = { 0, 0 };
        for(int i=0; i<playerCount; i++)
        {
            playersActive += msg.playerNames[i] + "\n";
            if((PlayerTeam)msg.playerTeams[i] != PlayerTeam.NotPicked)
            {
                teams[(PlayerTeam)msg.playerTeams[i] == PlayerTeam.TeamYellow ? 0 : 1]++;
                string ids = "";
                foreach (PlayerController p in PlayerController.players)
                    ids += p.playerId + " ";
                PlayerController player= PlayerController.players.Find(x => x.playerId == msg.playerIds[i]);
                player.team = (PlayerTeam)msg.playerTeams[i];
                player.UpdateTeam();
                //Debug.Log("Players ids: " + ids + ", player " + player.playerId + " has " + (PlayerTeam)msg.playerTeams[i]);
            }
                
        }
        canPickT1 = teams[0] <= teams[1];
        canPickT2 = teams[0] >= teams[1];
    }

    void PickedTeam(PlayerTeam team)
    {
        manager.pickedTeam = true;
        PickedTeamMsg msg = new PickedTeamMsg();
        msg.playerId = PlayerController.localInstance.playerId;
        msg.team = (byte)team;
        client.Send(PickedTeamMsg.type, msg);
        PlayerController.localInstance.team = team;
        //PlayerController.localInstance.CmdUpdateTeam();
    }
}
