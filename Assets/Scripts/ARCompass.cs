using UnityEngine;

public class ARCompass : MonoBehaviour
{
    private Camera _mainCamera;
    private double _lastCompassTimestamp;

    public Quaternion TrueHeadingRotation { get; private set; } = Quaternion.identity;

    #region Unity Callback

    private void Start()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!(Input.compass.timestamp > _lastCompassTimestamp)) return;
        _lastCompassTimestamp = Input.compass.timestamp;

        var declination = -(Input.compass.trueHeading - Input.compass.magneticHeading);

        
        UpdateRotation(
            new Vector3(Input.compass.rawVector.x, Input.compass.rawVector.y, -Input.compass.rawVector.z),
            declination);
    }

    #endregion

    private void UpdateRotation(Vector3 rawVector, float declination)
    {
        // compensate camera pose
        rawVector = _mainCamera.transform.rotation * rawVector;

        // while phone is held landscape, the compass is rotated 90 degrees clockwise
        // compensate for that
        rawVector = Quaternion.AngleAxis(90, Vector3.up) * rawVector;
        
        // calculate rotation
        var rotation = Quaternion.Euler(0, Input.compass.trueHeading, 0);
        
        // update global rotation
        TrueHeadingRotation = rotation;
        
    }
}