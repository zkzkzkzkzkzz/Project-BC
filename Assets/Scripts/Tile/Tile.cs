using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Board, Bench }

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType;           // Ÿ�� Ÿ��
    [SerializeField] private Unit occupyingUnit = null;   // Ÿ�� �������� ����
    [SerializeField] private Unit reservedBy = null;      // Ÿ���� ���� ������ ����

    [SerializeField] private Vector3Int coord;     // ����� ��ǥ
    [SerializeField] private int index;         // ��ġ�� �ε���


    // Ÿ�� ���� �ý���
    public bool IsOccupied() => occupyingUnit != null;
    public void ClearOccupyingUnit() => occupyingUnit = null;

    public bool IsReserved() => reservedBy != null;
    public bool IsReservedBy(Unit unit) => reservedBy == unit;
    public void Reserve(Unit unit) => reservedBy = unit;
    public void ClearReservation() => reservedBy = null;

    public bool IsAvailable() => !IsOccupied() && !IsReserved();

    public Zone zone { get; set; }

    public TileType GetTileType()
    {
        return tileType;
    }
    public void SetTileType(TileType _tileType)
    {
        tileType = _tileType;
    }

    public Unit GetOccupyingUnit()
    {
        return occupyingUnit;
    }
    public void SetOccupyingUnit(Unit _occupyingUnit)
    {
        occupyingUnit = _occupyingUnit;
    }


    public Vector3Int BoardCoord
    {
        get
        {
            if (tileType != TileType.Board)
                Debug.LogError("Ÿ���� Ÿ���� Board�� �ƴմϴ�");
            return coord;
        }
        set
        {
            coord = value;
        }
    }

    public int BenchIndex
    {
        get
        {
            if (tileType != TileType.Bench)
                Debug.LogError("Ÿ���� Ÿ���� Bench�� �ƴմϴ�");
            return index;
        }
        set
        {
            index = value;
        }
    }


    private void Update()
    {
        DebugColor();
    }

    /// <summary>
    /// @@@����׿�.
    /// ���� Ÿ�� ���� ���� ǥ��
    /// </summary>
    private void DebugColor()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        if (!IsOccupied() && !IsReserved())
            renderer.material.color = Color.white;
        else if (!IsOccupied() && IsReserved())
            renderer.material.color = Color.green;
        else
            renderer.material.color = tileType == TileType.Bench ? Color.yellow : Color.cyan;
    }
}
