using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchManager : MonoBehaviour
{
    public static BenchManager Instance { get; private set; }

    [SerializeField] private GameObject benchPrefab;
    [SerializeField] private int benchSize = 8;
    [SerializeField] private float tileSize = 2f;
    [SerializeField] private Vector3 benchStartPos;

    private List<GameObject> benchList = new List<GameObject>();

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
        }
    }

    public void ShowBenchGrid(bool show)
    {
        foreach (var tile in benchList)
        {
            tile.SetActive(show);
        }
    }
}
