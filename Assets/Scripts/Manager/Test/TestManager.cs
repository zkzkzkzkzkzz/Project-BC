using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance { get; private set; }

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

    [SerializeField] private float tileSpacing = 20f; // Zone 간 간격 조정용

    private readonly Vector2Int[] zoneGridPositions = new Vector2Int[]
    {
        new Vector2Int(0, 2), // 0번
        new Vector2Int(1, 2), // 1번
        new Vector2Int(2, 2), // 2번
        new Vector2Int(2, 1), // 3번
        new Vector2Int(2, 0), // 4번
        new Vector2Int(1, 0), // 5번
        new Vector2Int(0, 0), // 6번
        new Vector2Int(0, 1)  // 7번
    };

    public void GameStart()
    {
        Debug.Log("[TestManager] GameStart 호출됨.");

        ZoneManager.Instance.ClearZones(); // 혹시 기존 Zone이 있다면 초기화
        ZoneManager.Instance.InitializeZones(zoneGridPositions, tileSpacing);
    }


    /// <summary>
    /// 게임 상태 전환
    /// </summary>
    public void GameStateToggle()
    {
        GameState state = GameManager.Instance.CurState;

        if (state == GameState.Prepare)
        {
            GameManager.Instance.SetGameState(GameState.Battle);
            BattleManager.Instance.BattleStart();
        }
        else
            GameManager.Instance.SetGameState(GameState.Prepare);
    }


    [SerializeField] private int playerId = 0;
    /// <summary>
    /// 유닛 구매
    /// </summary>
    public void PurchaseUnit()
    {
        foreach (var zone in ZoneManager.Instance.GetAllZones())
        {
            if (zone.OwnerId == playerId)
                zone.Bench.PlaceUnitOnBench();
        }
    }
}
