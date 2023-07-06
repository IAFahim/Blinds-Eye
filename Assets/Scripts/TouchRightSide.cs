using System;
using Audio;
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
        if(AudioSourcePool.busy) return;

        if (Input.mousePosition.x > Screen.width * 0.5f)
        {
            onTouchRightSide?.Invoke();
        }
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width * 0.5f)
            {
                onTouchRightSide?.Invoke();
            }
        }
        
        interval = Time.time + delay;
    }
}