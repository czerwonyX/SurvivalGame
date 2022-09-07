using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Vector2 lastTouchPosition;
    float cameraSensitivity = (30f / 100);
    Camera cam;
    float yRotation;
    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }
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
                        float posX = (touch.position.x - lastTouchPosition.x) * cameraSensitivity;
                        float posY = (touch.position.y - lastTouchPosition.y) * cameraSensitivity;
                        // camera X
                        transform.Rotate(0, posX, 0);

                        // Camera Y
                        yRotation -= posY;
                        yRotation = Mathf.Clamp(yRotation, -90, 90);
                        cam.transform.localRotation = Quaternion.Euler(yRotation,0, 0);
                        
                        lastTouchPosition = touch.position;
                        break;
                    }
                }
            }
        }
    }
    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
