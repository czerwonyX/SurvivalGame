using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpForce = 1;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    CharacterController characterController;
    PhotonView view;
    Vector3 velocity;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();
    }

    public void FixedUpdate()
    {
        if (!view.IsMine) return;

        GameManager.playerMove *= moveSpeed;
        print($"{GameManager.playerMove} move");
        print(moveSpeed + "speed");
        //   rb.velocity = transform.forward * GameManager.playerMove.y;
        //  rb.velocity += transform.right * GameManager.playerMove.x;
        characterController.Move((transform.forward * GameManager.playerMove.y) + (transform.right * GameManager.playerMove.x));
    }
    public void Update()
    {
        if (!view.IsMine) { print(view.IsMine); return; }
        //move.x = 0;
        //move.y = Input.GetAxis("Vertical");

        //isGrounded = Physics.OverlapSphere(groundChecker.position, 0.1f, 7).Length > 0;
        isGrounded = Physics.Raycast(groundChecker.position,Vector3.down, 0.1f);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        //if (Input.GetKey(KeyCode.Space) && isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpForce * -2 * Physics.gravity.y);
        //}
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    public void Jump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2 * Physics.gravity.y);
    }

}
