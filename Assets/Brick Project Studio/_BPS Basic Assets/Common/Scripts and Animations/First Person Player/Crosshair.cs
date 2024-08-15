using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();

        //hide the mouse cursor at the centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnGUI()
    {
        int size = 12;

        //centre of screen and caters for font size
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;

        GUIStyle style = new GUIStyle();
        //for crosshair size
        style.fontSize = 50;

        //displays "*" as crosshair
        GUI.Label(new Rect(posX, posY, size, size), "*", style);
    }
}
