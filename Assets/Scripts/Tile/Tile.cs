using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Board, Bench }

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType;           // 타일 타입
    [SerializeField] private Unit occupyingUnit = null;   // 타일 점유중인 유닛

    [SerializeField] private Vector3 coord;     // 보드용 좌표
    [SerializeField] private int index;         // 벤치용 인덱스


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


    public Vector3 BoardCoord
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


    public bool IsOccupied()
    {
        return occupyingUnit != null;
    }

    public void ClearOccupyingUnit()
    {
        occupyingUnit = null;
    }

    private void Update()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = occupyingUnit != null ? Color.yellow : Color.white;
        }
    }
}
