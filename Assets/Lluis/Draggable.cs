using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{

    private bool isDragged = false;
    private Vector3 mouseDragStartPosition;

    private Vector3 spriteDragStartPosition;

    public Camera UICamera;

    private void OnMouseDown()
    {
        Debug.Log("On mouse down");
        isDragged = true;
        mouseDragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteDragStartPosition = transform.localPosition;
    }

    private void OnMouseDrag()
    {
        Debug.Log("On mouse drag");
        if (isDragged)
        {
            transform.localPosition = spriteDragStartPosition + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosition);
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("On mouse up");
        isDragged = false;
    }
}
