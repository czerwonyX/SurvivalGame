using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    Rigidbody rb;
    PhotonView view;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    public void FixedUpdate()
    {
        if (!view.IsMine) return;

        GameManager.playerMove *= moveSpeed;
        rb.velocity = transform.forward * GameManager.playerMove.y;
        rb.velocity += transform.right * GameManager.playerMove.x;
    }
    public void Update()
    {
        //move.x = 0;
        //move.y = Input.GetAxis("Vertical");
    }

}
