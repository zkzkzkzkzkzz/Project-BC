using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitLocationType { Bench, Board }

public class DraggableUnit : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private Plane dragPlane;
    private Vector3 originPos;

    [SerializeField] private float offsetY = 0.5f;      // �巡�� �� ������ ���÷����� ����
    [SerializeField] private LayerMask tileLayerMask;   // Ÿ�ϸ� �����ϵ��� ���̾� ����

    private GameObject hoveredTile = null;  // ���� ���콺�� ����Ű�� �ִ� Ÿ��

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnMouseDown()
    {
        originPos = transform.position;

        dragPlane = new Plane(Vector3.up, Vector3.zero);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        // ���� �巡��
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = new Vector3(hitPoint.x, hitPoint.y + offsetY, hitPoint.z);
        }

        // Ÿ�� ����
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayerMask))
            hoveredTile = hit.collider.gameObject;
        else
            hoveredTile = null;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (hoveredTile != null)
        {
            Debug.Log("Ÿ�� �߰�");
            Vector3 targetPos = hoveredTile.transform.position;
            transform.position = new Vector3(targetPos.x, originPos.y, targetPos.z);
            hoveredTile = null;
        }
        else
        {
            Debug.Log("Ÿ�� �߰� X");
            transform.position = originPos;
            hoveredTile = null;
        }
    }
}
