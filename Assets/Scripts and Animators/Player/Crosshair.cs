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
        float posX = cam.pixelWidth / 2 - size / 1.5f;
        float posY = cam.pixelHeight / 2 - size / 0.75f;

        GUIStyle style = new GUIStyle();
        //for crosshair size
        style.fontSize = 30;

        //displays "+" as crosshair
        GUI.Label(new Rect(posX, posY, size, size), "+", style);
    }
}
