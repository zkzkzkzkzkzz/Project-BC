using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Board, Bench }

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType;           // Ÿ�� Ÿ��
    [SerializeField] private Unit occupyingUnit = null;   // Ÿ�� �������� ����

    [SerializeField] private Vector3 coord;     // ����� ��ǥ
    [SerializeField] private int index;         // ��ġ�� �ε���


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
