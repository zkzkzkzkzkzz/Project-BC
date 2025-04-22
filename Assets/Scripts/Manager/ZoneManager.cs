using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [SerializeField] private int zoneCount = 1;
    [SerializeField] private GameObject boardPrefab;
    [SerializeField] private GameObject benchPrefab;
    [SerializeField] private Vector3 startPosition = Vector3.zero;
    [SerializeField] private Vector3 zoneOffset = new Vector3(20f, 0f, 0f); // Zone 간 거리

    private List<Zone> zones = new List<Zone>();

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
    /// zoneCount만큼 Zone을 생성
    /// </summary>
    public void InitializeZones()
    {
        zones.Clear();

        for (int i = 0; i < zoneCount; ++i)
        {
            GameObject zoneRoot = new GameObject($"Zone_{i}");

            Vector3 offset = startPosition + zoneOffset * i;

            GameObject boardObj = Instantiate(boardPrefab, offset, Quaternion.identity);
            GameObject benchObj = Instantiate(benchPrefab, offset + new Vector3(0f, 0f, -10f), Quaternion.identity);

            BoardManager board = boardObj.GetComponent<BoardManager>();
            BenchManager bench = benchObj.GetComponent<BenchManager>();

            if (board != null) board.GenerateGrid(i);
            if (bench != null) bench.GenerateBench(i);

            Zone zone = new Zone();
            zone.Initialize(i, zoneRoot.transform);

            zones.Add(zone);
        }
    }

    /// <summary>
    /// 특정 OwnerId를 가진 Zone을 가져온다
    /// </summary>
    public Zone GetZoneByOwner(int ownerId)
    {
        return zones.FirstOrDefault(z => z.OwnerId == ownerId);
    }

    /// <summary>
    /// 로컬 플레이어가 소유한 Zone을 가져온다
    /// </summary>
    public Zone GetMyZone()
    {
        return GetZoneByOwner(PlayerSessionManager.Instance.LocalPlayerId);
    }

    /// <summary>
    /// 전체 Zone 리스트를 반환
    /// </summary>
    public List<Zone> GetAllZones()
    {
        return zones;
    }

    public void InitializeZones(Vector2Int[] layout, float spacing)
    {
        zones.Clear();

        for (int i = 0; i < zoneCount; ++i)
        {
            GameObject zoneRoot = new GameObject($"Zone_{i}");
            
            Vector2Int gridPos = layout[i];
            Vector3 worldPos = new Vector3(gridPos.x * spacing, 0f, gridPos.y * spacing);

            Zone zone = new Zone();
            zone.Initialize(i, zoneRoot.transform);

            zones.Add(zone);

            GameObject boardObj = Instantiate(boardPrefab, worldPos, Quaternion.identity, zoneRoot.transform);
            GameObject benchObj = Instantiate(benchPrefab, worldPos + new Vector3(0f, 0f, -2f), Quaternion.identity, zoneRoot.transform);

            BoardManager board = boardObj.GetComponent<BoardManager>();
            BenchManager bench = benchObj.GetComponent<BenchManager>();

            if (board != null) board.GenerateGrid(i);
            if (bench != null) bench.GenerateBench(i);

            zone.SetBoardAndBench(board, bench);
        }
    }

    public void ClearZones()
    {
        foreach (var zone in zones)
        {
            if (zone.Board != null)
                Destroy(zone.Board.gameObject);
            if (zone.Bench != null)
                Destroy(zone.Bench.gameObject);
        }
        zones.Clear();
    }
}
