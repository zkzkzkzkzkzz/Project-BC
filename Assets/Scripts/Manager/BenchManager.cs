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

    private List<GameObject> benchList = new List<GameObject>();
    private List<GameObject> unitsOnBench = new List<GameObject>();
    private int unitCount = 0;  // 디버그용
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
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
            GameObject bench = Instantiate(benchPrefab, tilePos, Quaternion.identity, transform);
            bench.name = $"BenchTile_{i}";
            benchList.Add(bench);
            bench.SetActive(false);
            unitsOnBench.Add(null);
        }
    }

    public bool PlaceUnitOnBench()
    {
        for (int i = 0; i < benchSize; ++i)
        {
            if (unitsOnBench[i] == null)
            {
                Vector3 unitPos = benchList[i].transform.position + Vector3.up;
                GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);

                var renderer = unit.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material = new Material(renderer.material);
                    Color color = Color.HSVToRGB((unitCount * 0.1f) % 1f, 0.8f, 1f);
                    renderer.material.color = color;
                }

                unitsOnBench[i] = unit;

                var dragUnit = unit.GetComponent<DraggableUnit>();
                dragUnit.SetCurUnitTile(benchList[i]);
                ++unitCount;
                return true;

                //Vector3 unitPos = benchList[i].transform.position + Vector3.up;
                //GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
                //unitsOnBench[i] = unit;
                //return true;
            }
        }

        Debug.Log("대기열이 가득 찼습니다.");
        return false;
    }

    public void ShowBenchGrid(bool show)
    {
        foreach (var tile in benchList)
        {
            tile.SetActive(show);
        }
    }

    public int GetBenchTileIndex(GameObject tile)
    {
        return benchList.IndexOf(tile);
    }

    public GameObject GetBenchTileAt(int idx)
    {
        if (idx >= 0 && idx < benchList.Count)
            return benchList[idx];
        return null;
    }

    public GameObject GetBenchUnitAt(int idx)
    {
        if (idx >= 0 && idx < unitsOnBench.Count)
            return unitsOnBench[idx];
        return null;
    }

    public void SetBenchUnitAt(int idx, GameObject unit)
    {
        if (idx >= 0 && idx < unitsOnBench.Count)
            unitsOnBench[idx] = unit;
    }

    public int GetBenchUnitIndex(GameObject unit)
    {
        return unitsOnBench.IndexOf(unit);
    }
}
