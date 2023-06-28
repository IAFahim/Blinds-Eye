using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    public RawImage rawImageBackground;
    public WebCamTexture _cameraTexture;
    public TMP_Text text;
    public bool _isCamAvaible;
    public AspectRatioFitter aspectRatioFitter;
    public Button RefetchButton;
    public Slider width;
    public Slider height;
    public Slider oriantation;
    public Vector2 dimensions;
    public Button flip;
    public bool flipped = false;

    private void Start()
    {
        SetUpCam();
        RefetchButton.onClick.AddListener(SetUpCam);
        flip.onClick.AddListener(flipIt);
    }

    private void SetUpCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            text.text = "No Camera Detected";
            Debug.Log("No Camera Detected");
            _isCamAvaible = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)width.value, (int)height.value, 30);
                Debug.Log($"x: {width.value} y: {(int)height.value}");
                break;
            }
        }

        _isCamAvaible = true;
        _cameraTexture.Play();

        rawImageBackground.texture = _cameraTexture;
    }

    private void Update()
    {
        UpdateCameraRender();
    }

    private void flipIt()
    {
        flipped = !flipped;
    }

    private void UpdateCameraRender()
    {
        if (_isCamAvaible == false)
        {
            return;
        }

        float ratio = 1;
        if (flipped)
        {
             ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        }
        else
        {
             ratio = (float)_cameraTexture.height / (float)_cameraTexture.width;
        }

        // aspectRatioFitter.aspectRatio = ratio;

        // int orientation = _cameraTexture.videoRotationAngle;
        text.text =
        $"width: {_cameraTexture.width} height: {_cameraTexture.height} orientation: {_cameraTexture.videoRotationAngle}";
        // orientation = (int)(orientation * oriantation.value);
        // rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }
}