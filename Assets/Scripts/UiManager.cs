using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public Texture2D cursorTex;
    public Vector2 hotspot;
    private void Awake()
    {
        Cursor.SetCursor(cursorTex, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Cursor.SetCursor(cursorTex, hotspot, CursorMode.Auto);    
    }
}
