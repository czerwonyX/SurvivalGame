using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO: Create a game
    // TOOD: Create terrain
    // TODO: Create health system
    // TODO: Create inventory and containers
    // TODO: Create tree
    // TODO: 


    Vector2 posMin = new Vector2(260,15);
    Vector2 posMax = new Vector2(300,50);
    private void Awake()
    {
        float x = Random.Range(posMin.x, posMax.x);
        float y = Random.Range(posMin.y, posMax.y);
        GameObject player = PhotonNetwork.InstantiateRoomObject("Player", new Vector3(x, 90, y), Quaternion.identity);

        Camera cam = FindObjectOfType<Camera>();
        cam.transform.parent = player.transform;
        cam.transform.position = new Vector3(0, 0.47f, 0.6f);
    }
}
