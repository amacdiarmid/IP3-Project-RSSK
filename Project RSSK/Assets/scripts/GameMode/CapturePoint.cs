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

    [SyncVar]
    bool team1Captured = false;

    List<PlayerController> captors = new List<PlayerController>();
    float accTime = 0;
    Material indicatorMat;
    
    void Start ()
    {
        indicatorMat = indicator.GetComponent<MeshRenderer>().material;
        indicatorMat.color = team1Captured ? teamColors[0] : teamColors[1];
	}

    [ClientRpc]
    void RpcChangeColor(Vector3 from, Vector3 to, float progress)
    {
        indicatorMat.color = Color.Lerp(new Color(from.x, from.y, from.z), new Color(to.x, to.y, to.z), progress);
    }

    void FixedUpdate()
    {
        float xRot = Random.Range(0, maxRotSpeed) * Time.deltaTime;
        float yRot = Random.Range(0, maxRotSpeed) * Time.deltaTime;
        float zRot = Random.Range(0, maxRotSpeed) * Time.deltaTime;
        indicator.Rotate(xRot, yRot, zRot);

        if (!isServer)
            return;

        if(captors.Count > 0)
        {
            int[] teamCaptors = new int[] { 0, 0 };
            foreach (PlayerController contr in captors)
                teamCaptors[contr.team]++;
            if (!team1Captured && teamCaptors[0] > teamCaptors[1])
                accTime += Time.deltaTime;
            else if (team1Captured && teamCaptors[0] < teamCaptors[1])
                accTime += Time.deltaTime;

            float progr = accTime / captureTime;
            Color from = team1Captured ? teamColors[0] : teamColors[1];
            Color to = team1Captured ? teamColors[1] : teamColors[0];
            RpcChangeColor(new Vector3(from.r, from.g, from.b), new Vector3(to.r, to.g, to.b), progr);

            if (accTime > captureTime)
            {
                accTime = 0;
                team1Captured = !team1Captured;
            }
        }
        else if(accTime > 0)
        {
            accTime -= Time.deltaTime;
            float progr = accTime / captureTime;
            Color from = team1Captured ? teamColors[0] : teamColors[1];
            Color to = team1Captured ? teamColors[1] : teamColors[0];
            RpcChangeColor(new Vector3(from.r, from.g, from.b), new Vector3(to.r, to.g, to.b), progr);
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
