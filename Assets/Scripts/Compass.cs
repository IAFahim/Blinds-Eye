using TMPro;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public ARCompass arCompass;
    public RectTransform compassImage;
    public TMP_Text text;
    void Update()
    {
        Quaternion quaternion = arCompass.TrueHeadingRotation;
        quaternion = Quaternion.Euler(0, 0, -quaternion.eulerAngles.y);
        compassImage.rotation = quaternion;
        Vector3 eulerAngles = quaternion.eulerAngles;
        text.text = $"x: {eulerAngles.x} y: {eulerAngles.y} z: {eulerAngles.z}";
    }
}