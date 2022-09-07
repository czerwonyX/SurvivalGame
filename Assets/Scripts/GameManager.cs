using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // TODO: Create a game
    // TOOD: Create terrain
    // TODO: Create health system
    // TODO: Create inventory and containers
    // TODO: Create tree
    // TODO: 

    public static Vector2 playerMove;
    [SerializeField] Button moveButtonForward;
    [SerializeField] Button moveButtonRight;
    [SerializeField] Button moveButtonLeft;
    [SerializeField] Button moveButtonBack; // 472.6099  91.77875  473.6458   
    Vector2 posMin = new Vector2(60f, 43);
    Vector2 posMax = new Vector2(472,473);
    bool moveForward;
    bool moveBack;
    bool moveRight;
    bool moveLeft;
    private void Awake()
    {
        float x = Random.Range(posMin.x, posMax.x);
        float y = Random.Range(posMin.y, posMax.y);
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(x, 93, y), Quaternion.identity);

        Camera cam = FindObjectOfType<Camera>();
        print(cam);
        print(player);
        cam.transform.parent = player.transform;
        cam.transform.localPosition = new Vector3(0, 0.47f, 0.6f);


        //Forward
        AddOnPointerUpToButton(moveButtonForward, PointerForwardUp);
        AddOnPointerDownToButton(moveButtonForward, PointerForwardDown);

        //Back
        AddOnPointerUpToButton(moveButtonBack, PointerBackUp);
        AddOnPointerDownToButton(moveButtonBack, PointerBackDown);

        // Right
        AddOnPointerUpToButton(moveButtonRight, PointerRightUp);
        AddOnPointerDownToButton(moveButtonRight, PointerRightDown);

        // Left
        AddOnPointerUpToButton(moveButtonLeft, PointerLeftUp);
        AddOnPointerDownToButton(moveButtonLeft, PointerLeftDown);

    }
    private void Update()
    {
        playerMove.x = (moveRight ? 1 : 0) + (moveLeft ? -1 : 0);
        playerMove.y = (moveForward ? 1 : 0) + (moveBack ? -1 : 0);
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
