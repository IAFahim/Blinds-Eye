using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CompassDirection : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform compassImage;
    public Transform camera;
    private bool isTextNotNull;
    public float trueHeading;

    void Start()
    {
        isTextNotNull = text != null;
        Input.compass.enabled = true;
        Input.location.Start();
        StartCoroutine(UpdateCompass());
    }

    IEnumerator UpdateCompass()
    {
        while (true)
        {
            trueHeading = Input.compass.trueHeading;
            // Calculate the direction based on the heading
            string direction = GetDirection(trueHeading);
            compassImage.DORotate(new Vector3(0, 0, trueHeading), .1f);
            camera.DORotate(new Vector3(0, trueHeading, 0), .1f);
            if (isTextNotNull) text.text = direction;
            yield return new WaitForSeconds(.1f);
        }
    }

    string GetDirection(float heading)
    {
        // Convert the heading to a positive value between 0 and 360
        if (heading < 0f)
        {
            heading += 360f;
        }

        switch (heading)
        {
            // Determine the direction based on the heading
            case >= 337.5f:
            case < 22.5f:
                return "North";
            case >= 22.5f and < 67.5f:
                return "NorthEast";
            case >= 67.5f and < 112.5f:
                return "East";
            case >= 112.5f and < 157.5f:
                return "SouthEast";
            case >= 157.5f and < 202.5f:
                return "South";
            case >= 202.5f and < 247.5f:
                return "SouthWest";
            case >= 247.5f and < 292.5f:
                return "West";
            case >= 292.5f and < 337.5f:
                return "NorthWest";
            default:
                return "Unknown";
        }
    }
}