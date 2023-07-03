using System;
using UnityEngine;
using UnityEngine.Events;

public class TouchRightSide : MonoBehaviour
{
    public UnityEvent onTouchRightSide;
    public float interval = 0.5f;
    public float delay = 0.5f;

    private void Start()
    {
        interval += Time.time;
    }

    private void Update()
    {
        if (Time.time < interval)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width * 0.5f)
            {
                onTouchRightSide.Invoke();
            }
        }
        else if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > Screen.width * 0.5f)
        {
            onTouchRightSide.Invoke();
        }
        
        interval = Time.time + delay;
    }
}