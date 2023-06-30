using System.Globalization;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class Compass : MonoBehaviour
{
    public ARCompass arCompass;
    public RectTransform compassImage;
    public TMP_Text text;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateCompass), 0, 0.5f);
    }

    private void UpdateCompass()
    {
        var euler = arCompass.TrueHeadingRotation.eulerAngles;
        text.text = euler.y.ToString(CultureInfo.InvariantCulture);
        compassImage.DORotate(new Vector3(0, 0, euler.y), 0.5f);
    }
}