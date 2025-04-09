using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitLocationType { Bench, Board }

public class DraggableUnit : MonoBehaviour
{
    [SerializeField] private float dragHeight = 0.5f;

    private Vector3 offset;
    private float originYPos;
    private bool isDragging = false;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnMouseDown()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            offset = transform.position - hitPoint;
            originYPos = transform.position.y;
            isDragging = true;
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = new Vector3(hitPoint.x + offset.x, originYPos + dragHeight, hitPoint.z + offset.z);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        
        Vector3 curPos = transform.position;
        transform.position = new Vector3(curPos.x, originYPos, curPos.z);
    }
}
