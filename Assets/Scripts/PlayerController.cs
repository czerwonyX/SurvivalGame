using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] Button moveButtonForward;
    [SerializeField] Button moveButtonRight;
    [SerializeField] Button moveButtonLeft;
    [SerializeField] Button moveButtonBack;
    public Vector2 move;
    Rigidbody rb;
    bool moveForward;
    bool moveBack;
    bool moveRight;
    bool moveLeft;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Forward
        AddOnPointerUpToButton(moveButtonForward, PointerForwardUp);
        AddOnPointerDownToButton(moveButtonForward, PointerForwardDown);

        //Back
        AddOnPointerUpToButton(moveButtonBack,PointerBackUp);
        AddOnPointerDownToButton(moveButtonBack, PointerBackDown);

        // Right
        AddOnPointerUpToButton(moveButtonRight,PointerRightUp);
        AddOnPointerDownToButton(moveButtonRight, PointerRightDown);

        // Left
        AddOnPointerUpToButton(moveButtonLeft, PointerLeftUp);
        AddOnPointerDownToButton(moveButtonLeft,PointerLeftDown);
    }

    public void FixedUpdate()
    {
        move *= moveSpeed;
        rb.velocity = transform.forward * move.y;
        rb.velocity += transform.right * move.x;
    }
    public void Update()
    {
        //move.x = 0;
        //move.y = Input.GetAxis("Vertical");
        move.x = (moveRight ? 1 : 0) + (moveLeft ? -1 : 0);
        move.y = (moveForward ? 1 : 0) + (moveBack ? -1 : 0);
    }

    // Forward
    void PointerForwardDown() { moveForward = true; }
    void PointerForwardUp() { moveForward = false; }

    // Back
    void PointerBackDown() { moveBack = true; }
    void PointerBackUp() { moveBack = false; }

    // Right
    void PointerRightDown() { moveRight = true; }
    void PointerRightUp() { moveRight = false; }

    // Left
    void PointerLeftDown() { moveLeft = true; }
    void PointerLeftUp() { moveLeft = false; }

    void AddOnPointerDownToButton(Button button, UnityEngine.Events.UnityAction unityAction)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => unityAction());
        trigger.triggers.Add(pointerDown);
    }
    void AddOnPointerUpToButton(Button button, UnityEngine.Events.UnityAction unityAction)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => unityAction());
        trigger.triggers.Add(pointerUp);
    }

}
