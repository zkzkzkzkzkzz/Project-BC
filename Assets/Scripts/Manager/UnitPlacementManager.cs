using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 벤치 매니저와 보드 매니저를 통합 관리하는 매니저
/// </summary>
public class UnitPlacementManager : MonoBehaviour
{
    public static UnitPlacementManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 빈 타일로 유닛 배치 요청
    /// </summary>
    public void RequestMove(Unit a, Tile to)
    {
        Tile from = a.curTile;
        if (CanMove(a, to))
            UpdateMove(a, from, to);
        else
        {
            Debug.LogWarning("다른 Zone으로 이동할 수 없습니다");
            a.transform.position = from.transform.position + Vector3.up * a.unitYOffset;
            return;
        }
    }

    /// <summary>
    /// 유닛끼리 스왑 요청
    /// </summary>
    public void RequestSwap(Unit a, Unit b)
    {
        Tile tileA = a.curTile;
        Tile tileB = b.curTile;

        if (CanSwap(a, b))
            UpdateSwap(a, b);
        else
        {
            Debug.LogWarning("다른 Zone유닛과 스왑할 수 없습니다");
            a.transform.position = tileA.transform.position + Vector3.up * a.unitYOffset;
            b.transform.position = tileB.transform.position + Vector3.up * b.unitYOffset;
            return;
        }
    }


    private void UpdateMove(Unit unit, Tile from, Tile to)
    {
        unit.transform.position = to.transform.position + Vector3.up * unit.unitYOffset;

        from.SetOccupyingUnit(null);
        to.SetOccupyingUnit(unit);
        unit.SetCurUnitTile(to);

        Zone zone = unit.zone;
        BoardManager board = zone.Board;
        BenchManager bench = zone.Bench;

        if (bench.IsBenchTile(from))
            bench.UnregisterUnitFromBench(unit);
        else if (board.IsBoardTile(from))
            board.UnregisterUnitFromBoard(unit);

        if (bench.IsBenchTile(to))
            bench.RegisterUnitToBench(unit, to);
        else if (board.IsBoardTile(to))
            board.RegisterUnitToBoard(unit, to);
    }

    private void UpdateSwap(Unit a, Unit b)
    {
        Zone zoneA = a.zone;

        BoardManager board = zoneA.Board;
        BenchManager bench = zoneA.Bench;

        Tile tileA = a.curTile;
        Tile tileB = b.curTile;

        a.transform.position = tileB.transform.position + Vector3.up * a.unitYOffset;
        b.transform.position = tileA.transform.position + Vector3.up * b.unitYOffset;

        tileA.SetOccupyingUnit(b);
        tileB.SetOccupyingUnit(a);

        a.SetCurUnitTile(tileB);
        b.SetCurUnitTile(tileA);

        if (tileA == null || tileB == null) return;

        if (bench.IsBenchTile(tileA))
            bench.RegisterUnitToBench(a, tileA);
        else if (board.IsBoardTile(tileA))
            board.RegisterUnitToBoard(a, tileA);

        if (bench.IsBenchTile(tileB))
            bench.RegisterUnitToBench(b, tileB);
        else if (board.IsBoardTile(tileB))
            board.RegisterUnitToBoard(b, tileB);
    }


    // 이동 가능 검증 함수들
    private bool CanMove(Unit a, Tile to)
    {
        if (a == null || to == null) return false;

        // Battle 상태일 때는 zone 무시
        if (GameManager.Instance.IsInBattle())
            return to.IsAvailable();
        else
            return a.zone == to.zone && to.IsAvailable();
    }

    private bool CanSwap(Unit a, Unit b)
    {
        return a != null && b != null && a.zone == b.zone;
    }
}
