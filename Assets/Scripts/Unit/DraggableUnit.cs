using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitLocationType { Bench, Board }

public class DraggableUnit : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private Plane dragPlane;
    private float originY;

    [SerializeField] float offsetY = 0.5f;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnMouseDown()
    {
        originY = transform.position.y;

        dragPlane = new Plane(Vector3.up, Vector3.zero);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = new Vector3(hitPoint.x, hitPoint.y + offsetY, hitPoint.z);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, originY, pos.z);
    }
}
