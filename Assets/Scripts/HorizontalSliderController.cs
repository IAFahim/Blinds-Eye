using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HorizontalSliderController : MonoBehaviour
{
    public Slider slider;
    public UnityEvent onValueIsZero;
    public UnityEvent onValueIsOne;
    public UnityEvent onValueIsTwo;
    private void OnEnable()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }
    
    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float arg0)
    {
        if (arg0 <= 0.1)
        {
            onValueIsZero?.Invoke();
        }
        else if (arg0 <= 1)
        {
            onValueIsOne?.Invoke();
        }
        else if (arg0 <= 2)
        {
            onValueIsTwo?.Invoke();
        }
    }
}