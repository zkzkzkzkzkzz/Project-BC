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

    [SerializeField] private float offsetY = 0.5f;      // 드래그 시 유닛이 들어올려지는 정도
    [SerializeField] private LayerMask tileLayerMask;   // 타일만 감지하도록 레이어 설정

    private GameObject hoveredTile = null;  // 현재 마우스가 가리키고 있는 타일

    private GameObject curTile; // 유닛이 현재 점유하고 있는 타일

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
            hoveredTile = hit.collider.gameObject;
        else
            hoveredTile = null;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (hoveredTile != null)
        {
            if (BenchManager.Instance.GetBenchTileIndex(hoveredTile) != -1)
            {
                int myIdx = BenchManager.Instance.GetBenchTileIndex(curTile);
                int targetIdx = BenchManager.Instance.GetBenchTileIndex(hoveredTile);

                if (myIdx == -1 || targetIdx == -1)
                    return;

                GameObject otherUnit = BenchManager.Instance.GetBenchUnitAt(targetIdx);

                if (otherUnit != null && otherUnit != this.gameObject)
                {
                    otherUnit.transform.position = curTile.transform.position;
                    transform.position = hoveredTile.transform.position;

                    DraggableUnit other = otherUnit.GetComponent<DraggableUnit>();
                    other.curTile = curTile;
                    curTile = hoveredTile;

                    BenchManager.Instance.SetBenchUnitAt(myIdx, otherUnit);
                    BenchManager.Instance.SetBenchUnitAt(targetIdx, this.gameObject);
                }
                else
                {
                    transform.position = hoveredTile.transform.position;
                    BenchManager.Instance.SetBenchUnitAt(myIdx, null);
                    BenchManager.Instance.SetBenchUnitAt(targetIdx, this.gameObject);
                    curTile = hoveredTile;
                }

            }
            else
                transform.position = originPos;
            //Debug.Log("타일 발견");
            //Vector3 targetPos = hoveredTile.transform.position;
            //transform.position = new Vector3(targetPos.x, originPos.y, targetPos.z);
            //hoveredTile = null;
        }
        else
        {
            Debug.Log("타일 발견 X");
            transform.position = originPos;
            hoveredTile = null;
        }
    }


    public GameObject GetCurUnitTile()
    {
        return curTile;
    }

    public void SetCurUnitTile(GameObject tile)
    {
        curTile = tile;
    }
}
