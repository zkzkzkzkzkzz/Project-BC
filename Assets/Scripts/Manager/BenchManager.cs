using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchManager : MonoBehaviour
{
    public static BenchManager Instance { get; private set; }

    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private GameObject benchPrefab;
    [SerializeField] private int benchSize = 8;
    [SerializeField] private float tileSize = 2f;
    [SerializeField] private Vector3 benchStartPos;

    private List<Tile> benchTiles = new List<Tile>();
    [SerializeField] private List<Unit> benchUnits = new List<Unit>();   // ��ġ�� ��ġ�� ����
    private int unitCount = 0;  // ����׿�

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

    private void Start()
    {
        GenerateBench();
    }

    private void GenerateBench()
    {
        for (int i = 0; i < benchSize; ++i)
        {
            Vector3 tilePos = benchStartPos + new Vector3(i * tileSize, 0, 0);
            GameObject tileObj = Instantiate(benchPrefab, tilePos, Quaternion.identity, transform);
            tileObj.name = $"BenchTile_{i}";

            Tile tile = tileObj.GetComponent<Tile>();
            if (tile == null)
                tile = tileObj.AddComponent<Tile>();

            tile.SetTileType(TileType.Bench);
            tile.BenchIndex = i;

            tileObj.SetActive(false);
            benchTiles.Add(tile);
            benchUnits.Add(null);
        }
    }

    public bool PlaceUnitOnBench()
    {
        for (int i = 0; i < benchUnits.Count; ++i)
        {
            if (benchUnits[i] == null)
            {
                Vector3 unitPos = benchTiles[i].transform.position + Vector3.up;
                GameObject unitObj = Instantiate(unitPrefab, unitPos, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();

                unit.OwnerId = PlayerSessionManager.Instance.LocalPlayerId;
                unit.SetCurUnitTile(benchTiles[i]);
                benchTiles[i].SetOccupyingUnit(unit);

                benchUnits[i] = unit;

                // @@����׿�
                unit.name = $"MyUnit_{unitCount}";
                
                var renderer = unit.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material = new Material(renderer.material);
                    Color color = Color.HSVToRGB((unitCount * 0.1f) % 1f, 0.8f, 1f);
                    renderer.material.color = color;
                }
                ++unitCount;
                // ==========

                return true;
            }
        }

        Debug.Log("��⿭�� ���� á���ϴ�.");
        return false;
    }

    /// <summary>
    /// ��ġ ����Ʈ�� ���� ���
    /// </summary>
    public void RegisterUnitToBench(Unit unit, Tile to)
    {
        int idx = benchTiles.IndexOf(to);
        if (idx >= 0)
            benchUnits[idx] = unit;
    }

    /// <summary>
    /// ��ġ ����Ʈ���� ���� ����
    /// </summary>
    public void UnregisterUnitFromBench(Unit unit)
    {
        int idx = benchUnits.IndexOf(unit);
        if (idx >= 0)
            benchUnits[idx] = null;
    }



    public void ShowBenchGrid(bool show)
    {
        foreach (var tile in benchTiles)
        {
            tile.gameObject.SetActive(show);
        }
    }

    public bool IsBenchTile(Tile tile)
    {
        return tile.GetTileType() == TileType.Bench;
    }

    public List<Tile> GetBenchTiles()
    {
        return benchTiles;
    }

    public List<Unit> GetBenchUnits()
    {
        return benchUnits;
    }
}
