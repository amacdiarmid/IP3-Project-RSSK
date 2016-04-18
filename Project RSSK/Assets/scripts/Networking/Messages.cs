using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public enum MsgTypes
{
	ChangeName = MsgType.Highest + 1,
    PickedCharacter,
    PickedTeam,
    GameState,
    PrepTimeStart,
    RoundStart
}

public class ChangeNameMsg : MessageBase
{
    public static short type = (short)MsgTypes.ChangeName;

    public short playerId;
    public string playerName;
}

public class PickedTeamMsg : MessageBase
{
    public static short type = (short)MsgTypes.PickedTeam;

    public short playerId;
    public byte character;
    public byte team;
}

public class GameStateMsg : MessageBase
{
    public static short type = (short)MsgTypes.GameState;

    public int playerCount;
    public bool yellowAttacking;
    public byte[] score;
    public string sceneName;
    public string[] playerNames;
    public byte[] playerTeams;
    public byte[] playerLives;
    public short[] playerIds;
	public byte[] playerChars;
}