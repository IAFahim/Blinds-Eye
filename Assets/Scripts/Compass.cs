using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class Compass : MonoBehaviour
{
    public ARCompass arCompass;
    public RectTransform compassImage;
    public TMP_Text text;

    private bool compassEnabled;
    private Quaternion initialRotation;

    void Start()
    {
        // Check if the device has a compass
        compassEnabled = Input.compass.enabled;

        // Check if the device has an accelerometer
        bool accelerometerEnabled = SystemInfo.supportsAccelerometer;

        if (!compassEnabled || !accelerometerEnabled)
        {
            Debug.LogError("Device compass or accelerometer is not available.");
            return;
        }

        // Enable the compass
        Input.compass.enabled = true;

        // Store the initial rotation of the object
        initialRotation = compassImage.rotation;

        // Start the rotation update coroutine
        StartCoroutine(UpdateRotation());
    }

    private IEnumerator UpdateRotation()
    {
        while (true)
        {
            // Get the current accelerometer and compass data
            Vector3 accelerometerData = Input.acceleration;
            Vector3 compassData = Input.compass.rawVector;

            // Calculate the target rotation value using accelerometer and compass data
            float rotation = Mathf.Atan2(accelerometerData.x, accelerometerData.y) * Mathf.Rad2Deg;
            rotation += Input.compass.trueHeading;

            // Smoothly animate the rotation using DoTween
            compassImage.DORotateQuaternion(initialRotation * Quaternion.Euler(0f, 0f, -rotation), 0.5f);

            text.text = $"rotation: {Input.compass.trueHeading}";

            yield return null;
        }
    }
}