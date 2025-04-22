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

    [SerializeField] private float tileSpacing = 20f; // Zone �� ���� ������

    private readonly Vector2Int[] zoneGridPositions = new Vector2Int[]
    {
        new Vector2Int(0, 2), // 0��
        new Vector2Int(1, 2), // 1��
        new Vector2Int(2, 2), // 2��
        new Vector2Int(2, 1), // 3��
        new Vector2Int(2, 0), // 4��
        new Vector2Int(1, 0), // 5��
        new Vector2Int(0, 0), // 6��
        new Vector2Int(0, 1)  // 7��
    };

    public void GameStart()
    {
        Debug.Log("[TestManager] GameStart ȣ���.");

        ZoneManager.Instance.ClearZones(); // Ȥ�� ���� Zone�� �ִٸ� �ʱ�ȭ
        ZoneManager.Instance.InitializeZones(zoneGridPositions, tileSpacing);
    }


    /// <summary>
    /// ���� ���� ��ȯ
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
    /// ���� ����
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
