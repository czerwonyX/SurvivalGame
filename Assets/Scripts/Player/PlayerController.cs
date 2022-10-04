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
        InvokeRepeating("PlayerLoop", 0f, 3f);
    }
    public void FixedUpdate()
    {
        if (!view.IsMine) return;

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

        Vector3 move;
        move.x = GameManager.playerMove.x * moveSpeed;
        move.y = velocity.y;
        move.z = GameManager.playerMove.y * moveSpeed;
        //characterController.Move(((transform.forward * GameManager.playerMove.y)
        //    + (transform.right * GameManager.playerMove.x))
        //    * Time.deltaTime);
        characterController.Move((move.z * transform.forward + move.x * transform.right)* Time.deltaTime);
        
        characterController.Move(velocity * Time.deltaTime);
    }
    public void Jump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2 * Physics.gravity.y);
    }

    public void setMoveSpeed(float value) => moveSpeed = value;
    public float getMoveSpeed() => moveSpeed;
    void PlayerLoop()
    {
        Vector3 pos = transform.position;
        if (pos.y <= 0)
        {
            characterController.enabled = false;
            Ray ray = new Ray(transform.position, transform.up + Vector3.up * 2);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                characterController.Move(-velocity);
                transform.position = new Vector3(pos.x, hit.point.y +10, pos.z);
            }
            else
                transform.position = new Vector3(pos.x, 130, pos.z);
            characterController.enabled = true;
        }
    }

}
