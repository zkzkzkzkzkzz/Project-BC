using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Board, Bench }

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType;           // 타일 타입
    [SerializeField] private Unit occupyingUnit = null;   // 타일 점유중인 유닛
    [SerializeField] private Unit reservedBy = null;      // 타일을 점유 예정인 유닛

    [SerializeField] private Vector3Int coord;     // 보드용 좌표
    [SerializeField] private int index;         // 벤치용 인덱스


    // 타일 예약 시스템
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
                Debug.LogError("타일의 타입이 Board가 아닙니다");
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
                Debug.LogError("타일의 타입이 Bench가 아닙니다");
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
    /// @@@디버그용.
    /// 유닛 타일 점유 상태 표기
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
