using System.Collections;
using System.Collections.Generic;
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

    private void UpdateMove(Unit unit, Tile from, Tile to)
    {
        if (BenchManager.Instance.IsBenchTile(from))
            BenchManager.Instance.UnregisterUnitFromBench(unit);
        else if (BoardManager.Instance.IsBoardTile(from))
            BoardManager.Instance.UnregisterUnitFromBoard(unit);

        if (BenchManager.Instance.IsBenchTile(to))
            BenchManager.Instance.RegisterUnitToBench(unit, to);
        else if (BoardManager.Instance.IsBoardTile(to))
            BoardManager.Instance.RegisterUnitToBoard(unit, to);
    }

    private void UpdateSwap(Unit a, Unit b)
    {
        Tile tileA = a.curTile;
        Tile tileB = b.curTile;

        if (tileA == null || tileB == null) return;

        if (BenchManager.Instance.IsBenchTile(tileA))
            BenchManager.Instance.RegisterUnitToBench(a, tileA);
        else if (BoardManager.Instance.IsBoardTile(tileA))
            BoardManager.Instance.RegisterUnitToBoard(a, tileA);

        if (BenchManager.Instance.IsBenchTile(tileB))
            BenchManager.Instance.RegisterUnitToBench(b, tileB);
        else if (BoardManager.Instance.IsBoardTile(tileB))
            BoardManager.Instance.RegisterUnitToBoard(b, tileB);
    }

    /// <summary>
    /// 빈 타일로 유닛 배치 요청
    /// </summary>
    public void RequestMove(Unit a, Tile to)
    {
        Tile from = a.curTile;

        a.transform.position = to.transform.position + Vector3.up * a.unitYOffset;

        from.SetOccupyingUnit(null);
        to.SetOccupyingUnit(a);
        a.SetCurUnitTile(to);

        UpdateMove(a, from, to);
    }

    /// <summary>
    /// 유닛끼리 스왑 요청
    /// </summary>
    public void RequestSwap(Unit a, Unit b)
    {
        Tile tileA = a.curTile;
        Tile tileB = b.curTile;

        a.transform.position = tileB.transform.position + Vector3.up * a.unitYOffset;
        b.transform.position = tileA.transform.position + Vector3.up * b.unitYOffset;

        tileA.SetOccupyingUnit(b);
        tileB.SetOccupyingUnit(a);

        a.SetCurUnitTile(tileB);
        b.SetCurUnitTile(tileA);

        UpdateSwap(a, b);
    }
}
