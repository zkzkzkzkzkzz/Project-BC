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
        var Units = FindObjectsOfType<Unit>();
        float minDist = float.MaxValue;
        Unit closestEnemy = null;
        foreach (var unit in Units)
        {
            if (unit.ownerId == ownerId || unit.curTile == null) continue;

            float dist = PathFindingSystem.Heuristic(curTile, unit.curTile);
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = unit;
            }
        }

        return closestEnemy;
    }

    // 그리디 방식
    private Unit moveTargetUnit = null;       // 타겟 유닛

    public void BattleStartAI()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(GreedyMoveRoutine());
    }

    private IEnumerator GreedyMoveRoutine()
    {
        while (GameManager.Instance.IsInBattle())
        {
            moveTargetUnit = FindClosestEnemy();
            if (moveTargetUnit == null || moveTargetUnit.curTile == null)
                break;

            if (IsAdjacentTo(moveTargetUnit)) break;

            Tile targetTile = moveTargetUnit.curTile;

            List<Tile> path = PathFindingSystem.FindPath(curTile, targetTile);
            if (path == null || path.Count < 2)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            Tile nextTile = path[1];

            if (!nextTile.IsAvailable())
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            curTile.ClearReservation();
            curTile.ClearOccupyingUnit();

            nextTile.Reserve(this);
            nextTile.SetOccupyingUnit(this);
            SetCurUnitTile(nextTile);

            //transform.position = nextTile.transform.position + Vector3.up * unitYOffset;

            yield return StartCoroutine(MoveSmooth(nextTile));
        }

        moveRoutine = null;
        curTile?.ClearReservation();
    }

    private bool IsAdjacentTo(Unit target)
    {
        var neighbors = PathFindingSystem.GetNeighbors(curTile);
        return neighbors.Contains(target.curTile);
    }

    public void BattleEndAI()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
    }

    /// <summary>
    /// 유닛 이동 보간 (임시)
    /// </summary>
    private IEnumerator MoveSmooth(Tile nextTile)
    {
        Vector3 start = transform.position;
        Vector3 end = nextTile.transform.position + Vector3.up * unitYOffset;

        float dist = Vector3.Distance(start, end);
        float duration = dist / 2f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }
}
