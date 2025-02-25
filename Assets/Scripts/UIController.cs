using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI Components")] [SerializeField]
    private TextMeshProUGUI objectsDetectedText;

    [SerializeField] private TextMeshProUGUI fpsText;

    [Header("Settings")] [SerializeField, Tooltip("Option to display number of objects detected")]
    public bool displayObjectCount = true;

    [SerializeField, Tooltip("Option to display fps")]
    private bool displayFPS = true;

    [SerializeField, Tooltip("Time in seconds between refreshing fps value"), Range(0.01f, 1.0f)]
    private float fpsRefreshRate = 0.1f;

    private float fpsTimer = 0f;

    public void Update()
    {
        // Update FPS text
        if (displayFPS)
        {
            UpdateFPS();
        }
        else
        {
            fpsText.gameObject.SetActive(false);
        }
    }

    public void UpdateUI(int objectCount)
    {
        // Update object count text
        if (displayObjectCount)
        {
            objectsDetectedText.gameObject.SetActive(true);
            if (objectCount > 0)
            {
                objectsDetectedText.text = objectCount.ToString();
            }
            else
            {
                objectsDetectedText.text = "";
            }
        }
        else
        {
            objectsDetectedText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the displayed FPS value.
    /// </summary>
    private void UpdateFPS()
    {
        if (Time.unscaledTime > fpsTimer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = $"FPS: {fps}";

            fpsTimer = Time.unscaledTime + fpsRefreshRate;
        }
    }
}