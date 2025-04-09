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

    [SerializeField] private float offsetY = 0.5f;      // 드래그 시 유닛이 들어올려지는 정도
    [SerializeField] private LayerMask tileLayerMask;   // 타일만 감지하도록 레이어 설정

    private Tile hoveredTile = null;            // 현재 마우스가 가리키고 있는 타일
    public Tile curTile { get; private set; }   // 유닛이 현재 점유하고 있는 타일

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

        // 유닛 드래그
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = new Vector3(hitPoint.x, hitPoint.y + offsetY, hitPoint.z);
        }

        // 타일 감지
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
                Debug.Log("대기열 타일 클릭");

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
                Debug.Log("보드 타일 클릭");
                // 추후 Board 관련 처리
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
