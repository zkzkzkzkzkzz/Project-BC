using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ġ �Ŵ����� ���� �Ŵ����� ���� �����ϴ� �Ŵ���
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

    /// <summary>
    /// �� Ÿ�Ϸ� ���� ��ġ ��û
    /// </summary>
    public void RequestMove(Unit a, Tile to)
    {
        Tile from = a.curTile;

        if (a.zone != to.zone)
        {
            Debug.LogWarning("�ٸ� Zone���� �̵��� �� �����ϴ�");
            a.transform.position = from.transform.position + Vector3.up * a.unitYOffset;
            return;
        }

        a.transform.position = to.transform.position + Vector3.up * a.unitYOffset;

        from.SetOccupyingUnit(null);
        to.SetOccupyingUnit(a);
        a.SetCurUnitTile(to);

        UpdateMove(a, from, to);
    }

    /// <summary>
    /// ���ֳ��� ���� ��û
    /// </summary>
    public void RequestSwap(Unit a, Unit b)
    {
        Tile tileA = a.curTile;
        Tile tileB = b.curTile;

        if (a.zone != b.zone)
        {
            Debug.LogWarning("�ٸ� Zone���ְ� ������ �� �����ϴ�");
            a.transform.position = tileA.transform.position + Vector3.up * a.unitYOffset;
            return;
        }

        a.transform.position = tileB.transform.position + Vector3.up * a.unitYOffset;
        b.transform.position = tileA.transform.position + Vector3.up * b.unitYOffset;

        tileA.SetOccupyingUnit(b);
        tileB.SetOccupyingUnit(a);

        a.SetCurUnitTile(tileB);
        b.SetCurUnitTile(tileA);

        UpdateSwap(a, b);
    }
}
