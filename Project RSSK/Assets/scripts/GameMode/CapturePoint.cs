using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CapturePoint : NetworkBehaviour
{
    public Transform indicator;
    public float captureTime = 15f;
    public Color[] teamColors = new Color[] { Color.yellow, Color.red };
    public float maxRotSpeed = 2;

    [SyncVar(hook="OnOwnerChanged")]
    public PlayerTeam owner = PlayerTeam.TeamYellow;

    List<PlayerController> captors = new List<PlayerController>();
    float accTime = 0;
    Material indicatorMat;
    
    void Start ()
    {
        indicatorMat = indicator.GetComponent<MeshRenderer>().material;
        indicatorMat.color = owner == PlayerTeam.TeamYellow ? teamColors[0] : teamColors[1];
	}

    [ClientRpc]
    void RpcChangeColor(Vector3 color)
    {
        indicatorMat.color = new Color(color.x, color.y, color.z);
    }
    
    void OnOwnerChanged(PlayerTeam newOwner)
    {
        Debug.Log("New Owner: " + newOwner);
        owner = newOwner;
        indicatorMat.color = owner == PlayerTeam.TeamYellow ? teamColors[0] : teamColors[1];
    }

    void FixedUpdate()
    {
        float xRot = Random.Range(0, maxRotSpeed) * Time.fixedDeltaTime;
        float yRot = Random.Range(0, maxRotSpeed) * Time.fixedDeltaTime;
        float zRot = Random.Range(0, maxRotSpeed) * Time.fixedDeltaTime;
        indicator.Rotate(xRot, yRot, zRot);

        if (!isServer)
            return;

        PlayerTeam attackers = owner == PlayerTeam.TeamYellow ? PlayerTeam.TeamBlue : PlayerTeam.TeamYellow;
        int attInd = (int)attackers - 1;
        int defInd = (int)owner - 1;
        int[] teamCaptors = new int[] { 0, 0 };
        foreach (PlayerController contr in captors)
            teamCaptors[(byte)contr.team - 1]++;

        if (teamCaptors[attInd] > 0 && teamCaptors[defInd] == 0) //capturing and not defending
        {
            accTime += Time.deltaTime;

            float progr = accTime / captureTime;
            Color from = teamColors[defInd];
            Color to = teamColors[attInd];
            Color res = Color.Lerp(from, to, progr);
            RpcChangeColor(new Vector3(res.r, res.g, res.b));

            if (accTime > captureTime)
            {
                accTime = 0;
                ((GameManager)NetworkManager.singleton).OnPointCaptured();
            }
        }
        else if(teamCaptors[(byte)attackers - 1] == 0 && accTime > 0) //not capturing
        {
            accTime -= Time.deltaTime;
            float progr = accTime / captureTime;
            Color from = teamColors[attInd];
            Color to = teamColors[defInd];
            Color res = Color.Lerp(from, to, progr);
            RpcChangeColor(new Vector3(res.r, res.g, res.b));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer && other.tag == "Player")
            captors.Add(other.GetComponent<PlayerController>());
    }

    void OnTriggerExit(Collider other)
    {
        if (isServer && other.tag == "Player")
            captors.Remove(other.GetComponent<PlayerController>());
    }
}
