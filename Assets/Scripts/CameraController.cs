using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Vector2 lastTouchPosition;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        lastTouchPosition = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 touchMove = touch.position - lastTouchPosition;
                        transform.Rotate(new Vector3(0, touchMove.x, 0));
                        lastTouchPosition = touch.position;
                        break;
                    }
                }
            }
        }
    }
}
