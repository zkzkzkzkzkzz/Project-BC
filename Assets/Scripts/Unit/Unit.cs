using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private Plane dragPlane;
    private Vector3 originPos;
    [SerializeField] private float unitYOffset = 1f;

    [SerializeField] private float offsetY = 0.5f;      // �巡�� �� ������ ���÷����� ����
    [SerializeField] private LayerMask tileLayerMask;   // Ÿ�ϸ� �����ϵ��� ���̾� ����

    private Tile hoveredTile = null;            // ���� ���콺�� ����Ű�� �ִ� Ÿ��
    public Tile curTile { get; private set; }   // ������ ���� �����ϰ� �ִ� Ÿ��

    public TileType curTileType => curTile?.GetTileType() ?? TileType.Bench;

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
            hoveredTile = hit.collider.GetComponent<Tile>();
        else
            hoveredTile = null;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        
        if (hoveredTile != null)
        {
            if (hoveredTile.GetTileType() == TileType.Bench)
            {
                Debug.Log("��⿭ Ÿ�� Ŭ��");

                Tile myTile = curTile;
                Tile targetTile = hoveredTile;

                if (myTile == null || targetTile == null) return;

                Unit otherUnit = targetTile.GetOccupyingUnit();

                if (otherUnit != null && otherUnit != this)
                {
                    otherUnit.transform.position = myTile.transform.position + Vector3.up * unitYOffset;
                    transform.position = targetTile.transform.position + Vector3.up * unitYOffset;

                    otherUnit.SetCurUnitTile(myTile);
                    SetCurUnitTile(targetTile);

                    myTile.SetOccupyingUnit(otherUnit);
                    targetTile.SetOccupyingUnit(this);
                }
                else
                {
                    transform.position = targetTile.transform.position + Vector3.up * unitYOffset;
                    myTile.SetOccupyingUnit(null);
                    targetTile.SetOccupyingUnit(this);
                    SetCurUnitTile(targetTile);
                }
            }
            else if (hoveredTile.GetTileType() == TileType.Board)
            {
                Debug.Log("���� Ÿ�� Ŭ��");
                // ���� Board ���� ó��
            }
            else
            {
                transform.position = originPos;
            }
        }
        else
        {
            transform.position = originPos;
        }
    }

    public void SetCurUnitTile(Tile tile)
    {
        curTile = tile;
    }
}
