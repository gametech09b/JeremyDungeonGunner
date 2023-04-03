using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    private void Awake()
    {
        // set hadware cursor to invisible
        Cursor.visible = false;
    }

    private void Update()
    {
        // set cursor position to mouse position
        transform.position = Input.mousePosition;
    }
}
