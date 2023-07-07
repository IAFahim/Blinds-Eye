using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;


    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        if (value <= 0.1)
        {
            Stage1(value);
        }
        else if (value <= 1)
        {
            Stage2(value);
        }
        else if (value <= 2)
        {
            Stage3(value);
        }
        else if (value <= 3)
        {
            Stage4(value);
        }
        else if (value <= 4)
        {
            Stage5(value);
        }
        else if (value <= 4.9)
        {
            Stage6(value);
        }
        else
        {
            Stage7(value);
        }
    }

    public GameObject screen;

    public void Stage7(float value)
    {
        screen.SetActive(true);
    }

    public GameObject bBox2DVisualizer;

    private void Stage6(float value)
    {
        screen.SetActive(false);
        bBox2DVisualizer.SetActive(true);
    }

    public GameObject paddingStage5;

    private void Stage5(float value)
    {
        bBox2DVisualizer.SetActive(false);
        paddingStage5.SetActive(true);
    }

    private void Stage4(float value)
    {
        paddingStage5.SetActive(false);
    }

    public Image[] sliderImageObjects;

    private void Stage3(float value)
    {
        foreach (var obj in sliderImageObjects)
        {
            var color = obj.color;
            color.a = value - 1;
            obj.color = color;
        }
    }

    public Image[] compassRadar;

    public void Stage2(float value)
    {
        foreach (var image in compassRadar)
        {
            var color = image.color;
            if (value > 0.2)
            {
                color.a = value;
            }
            else
            {
                color.a = 0;
            }

            image.color = color;
        }

        compassText.SetActive(true);
    }

    public GameObject compassText;

    public void Stage1(float value)
    {
        compassText.SetActive(false);
    }
}