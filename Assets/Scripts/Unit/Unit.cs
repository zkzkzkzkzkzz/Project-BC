using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private int ownerId;            // 유닛 오너 고유 id
    public int OwnerId => ownerId;  // 외부 읽기 전용 프로퍼티 제공

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

    private Coroutine moveRoutine;

    private void Awake()
    {
        mainCam = Camera.main;
    }    

    public void SetOwnerId(int id)
    {
        ownerId = id;
    }

    private void OnMouseDown()
    {
        if (!IsDragEnable()) return;

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
            Tile myTile = curTile;
            Tile targetTile = hoveredTile;

            if (myTile == null || targetTile == null ||
                myTile.GetTileType() == TileType.Board && GameManager.Instance.IsInBattle()) return;

            if (targetTile.GetTileType() == TileType.Board && GameManager.Instance.IsInBattle())
            {
                transform.position = originPos;
                return;
            }

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
        else
        {
            transform.position = originPos;
        }
    }

    public void SetCurUnitTile(Tile tile)
    {
        curTile = tile;
    }

    public bool IsDragEnable()
    {
        return GameManager.Instance.IsInPrepare() || curTileType == TileType.Bench;
    }


    /// <summary>
    /// 자신으로부터 가장 가까운 적 찾기
    /// </summary>
    private Unit FindClosestEnemy()
    {
        var allUnits = FindObjectsOfType<Unit>();
        float minDist = float.MaxValue;
        Unit closestEnemy = null;

        foreach (var unit in allUnits)
        {
            if (unit.ownerId == this.ownerId) continue;

            float dist = PathFindingSystem.Heuristic(this.curTile, unit.curTile);
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = unit;
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// target의 인접 타일 중 가장 짧은 경로를 가지는 타일 반환
    /// </summary>
    private Tile FindBestTileToMove(Unit target)
    {
        var neighbors = PathFindingSystem.GetNeighbors(target.curTile);

        Tile bestTile = null;
        int minDist = int.MaxValue;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.IsOccupied()) continue;

            var path = PathFindingSystem.FindPath(this.curTile, neighbor);
            if (path != null && path.Count < minDist)
            {
                bestTile = neighbor;
                minDist = path.Count;
            }
        }

        return bestTile;
    }

    /// <summary>
    /// 전투 시작 후 타겟 인접 타일 중 가장 가까운 타일로 이동
    /// </summary>
    public void MoveToTarget()
    {
        Unit target = FindClosestEnemy();
        if (target == null) return;

        Tile dest = FindBestTileToMove(target);
        if (dest == null || dest == curTile)
            return;

        var path = PathFindingSystem.FindPath(curTile, dest);
        if (path == null || path.Count < 2)
            return; // 현재 위치하고 있는 타일이 가장 가까운 타일

        for (int i = 1; i < path.Count; i++)
        {
            Tile nextTile = path[i];

            transform.position = nextTile.transform.position + Vector3.up * unitYOffset;

            curTile.SetOccupyingUnit(null);
            nextTile.SetOccupyingUnit(this);
            curTile = nextTile;
        }
    }

    private IEnumerator MoveRoutine()
    {
        if (curTile == null)
            yield break;

        Unit target = FindClosestEnemy();
        if (target == null)
            yield break;

        Tile dest = FindBestTileToMove(target);
        if (dest == null || dest == curTile)
            yield break;

        List<Tile> path = PathFindingSystem.FindPath(curTile, dest);
        if (path == null || path.Count < 2)
            yield break;

        for (int i = 1; i < path.Count; i++)
        {
            Tile nextTile = path[i];

            // 이동
            transform.position = nextTile.transform.position + Vector3.up * unitYOffset;

            // 점유 정보 갱신
            curTile.SetOccupyingUnit(null);
            nextTile.SetOccupyingUnit(this);
            curTile = nextTile;

            yield return new WaitForSeconds(1f);
        }

        moveRoutine = null;
    }

    public void BattleStartAI()
    {
        if (moveRoutine == null)
            moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void BattleEndAI()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
    }
}
