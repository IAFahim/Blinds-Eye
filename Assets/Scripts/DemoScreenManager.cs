using UnityEngine;
using CJM.MediaDisplay;
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
        cameraObject.transform.position = new Vector3(0, 0, -10);
        screenObject.transform.position = new Vector3(0, 0, 11);
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

    // Update the useWebcam option and the display when the webcam toggle changes
    public void UpdateWebcamToggle(bool useWebcam)
    {
        this.useWebcam = useWebcam;
        UpdateDisplay();
    }

    // Update the current webcam device and the display when the webcam dropdown selection changes
    public void UpdateWebcamDevice()
    {
        currentWebcam = webcamDevices[0].name;
        useWebcam = webcamDevices.Length > 0 && useWebcam;
        UpdateDisplay();
    }
}
