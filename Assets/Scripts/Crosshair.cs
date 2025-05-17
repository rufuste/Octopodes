using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float mouseSensitivity;
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        // Sets transform position of crosshair to linearly interlope toward mouse position
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.Lerp(transform.position, cursorPos, mouseSensitivity);
    }

}
