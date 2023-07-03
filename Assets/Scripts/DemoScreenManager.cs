using UnityEngine;
using System.Collections.Generic;
using System;

public class DemoScreenManager : BaseScreenManager
{
    // Called when the script instance is being loaded.
    private void Start()
    {
        Initialize();
        UpdateDisplay();
        InitializeDropdown();
        cameraObject.transform.position = new Vector3(0, 0, 0);
        screenObject.transform.position = new Vector3(0, 0, 0);
    }

    // Initialize the GUI dropdown list
    private void InitializeDropdown()
    {
        GetWebcamNames();
        UpdateWebcamDeviceSelection();
    }

    // Get the names of all available webcams
    private List<string> GetWebcamNames()
    {
        List<string> webcamNames = new List<string>();
        foreach (WebCamDevice device in webcamDevices)
        {
            webcamNames.Add(device.name);
        }
        return webcamNames;
    }

    // Update the selected value of the webcam dropdown
    private void UpdateWebcamDeviceSelection()
    {
        int selectedIndex = Array.FindIndex(webcamDevices, device => device.name == currentWebcam);
    }
}
