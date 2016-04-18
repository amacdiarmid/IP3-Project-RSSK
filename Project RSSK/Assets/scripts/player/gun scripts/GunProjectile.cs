using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunProjectile : NetworkBehaviour
{
	public float lifeSpan = 5;

	void Start ()
	{
		Destroy(gameObject, lifeSpan);
	}

	[ClientRpc]
	public void RpcSetUpLine(Vector3 start, Vector3 finish)
	{
		LineRenderer line = this.GetComponent<LineRenderer>();
		line.SetPosition(0, start);
		line.SetPosition(1, finish);
	}
}
