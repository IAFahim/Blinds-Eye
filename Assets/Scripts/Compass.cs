using UnityEngine;

public class Compass : MonoBehaviour
{
    public RectTransform compass_image;

    void Start()
    {
        if (Input.compass.enabled)
        {
            Input.compass.enabled = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.compass.enabled)
        {
            // Get the current heading from the compass
            float heading = Input.compass.trueHeading;

            // Rotate the minimap in the opposite direction of the heading
            compass_image.transform.rotation = Quaternion.Euler(0f, 0f, -heading);
        }
    }
}