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

	float accTime = 0;
	Material indicatorMat;
	Vector3 colliderExtents;

	void Start ()
	{
		indicatorMat = indicator.GetComponent<MeshRenderer>().material;
		indicatorMat.color = owner == PlayerTeam.TeamYellow ? teamColors[0] : teamColors[1];
		colliderExtents = GetComponent<BoxCollider> ().size / 2;
	}

	[ClientRpc]
	void RpcChangeColor(Vector3 color)
	{
		indicatorMat.color = new Color(color.x, color.y, color.z);
	}
	
	void OnOwnerChanged(PlayerTeam newOwner)
	{
		owner = newOwner;
		indicatorMat.color = owner == PlayerTeam.TeamYellow ? teamColors[0] : teamColors[1];
	}

	void FixedUpdate()
	{
		float xRot = Random.Range(0, maxRotSpeed) * Time.deltaTime;
		float yRot = Random.Range(0, maxRotSpeed) * Time.deltaTime;
		float zRot = Random.Range(0, maxRotSpeed) * Time.deltaTime;
		indicator.Rotate(xRot, yRot, zRot);

		if (!isServer)
			return;

		Collider[] hits = Physics.OverlapBox (transform.position, colliderExtents);

		PlayerTeam attackers = owner.Enemy();
		int attInd = (int)attackers - 1;
		int defInd = (int)owner - 1;
		int[] teamCaptors = new int[] { 0, 0 };
		foreach (Collider col in hits) 
		{
			if(col.tag == "Player")
				teamCaptors [(int)col.GetComponent<PlayerController>().team - 1]++;
		}

		Debug.LogWarning (teamCaptors [0] + " - " + teamCaptors [1]);

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
			Color from = teamColors[defInd];
			Color to = teamColors[attInd];
			Color res = Color.Lerp(from, to, progr);
			RpcChangeColor(new Vector3(res.r, res.g, res.b));
		}
	}
}
