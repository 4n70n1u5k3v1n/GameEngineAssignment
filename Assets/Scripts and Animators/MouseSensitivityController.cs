using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityController : MonoBehaviour
{
    public MouseLook playerMouseLook; // Reference to the MouseLook script on the player (X-axis)
    public MouseLook cameraMouseLook; // Reference to the MouseLook script on the camera (Y-axis)
    public Slider sensitivitySliderHor; // Reference to the UI Slider for horizontal sensitivity (player)
    public Slider sensitivitySliderVert; // Reference to the UI Slider for vertical sensitivity (camera)

    void Start()
    {
        // Initialize slider values based on current sensitivity
        if (sensitivitySliderHor != null && playerMouseLook != null)
        {
            sensitivitySliderHor.value = playerMouseLook.sensitivityHor;
            sensitivitySliderHor.onValueChanged.AddListener(UpdateHorizontalSensitivity);
        }

        if (sensitivitySliderVert != null && cameraMouseLook != null)
        {
            sensitivitySliderVert.value = cameraMouseLook.sensitivityVert;
            sensitivitySliderVert.onValueChanged.AddListener(UpdateVerticalSensitivity);
        }
    }

    // Method to update horizontal sensitivity for the player
    void UpdateHorizontalSensitivity(float value)
    {
        if (playerMouseLook != null)
        {
            playerMouseLook.sensitivityHor = value;
        }
    }

    // Method to update vertical sensitivity for the camera
    void UpdateVerticalSensitivity(float value)
    {
        if (cameraMouseLook != null)
        {
            cameraMouseLook.sensitivityVert = value;
        }
    }
}
