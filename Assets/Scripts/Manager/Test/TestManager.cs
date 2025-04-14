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

    /// <summary>
    /// 그리드 활성화 / 비활성화
    /// </summary>
    private bool bGrid = false;
    public void ToggleGrid()
    {
        bGrid = !bGrid;
        BoardManager.Instance.ShowHexGrid(bGrid);
        BenchManager.Instance.ShowBenchGrid(bGrid);
    }

    /// <summary>
    /// 유닛 구매
    /// 구매된 유닛은 벤치에 배치
    /// </summary>
    public void PurchaseUnit()
    {
        BenchManager.Instance.PlaceUnitOnBench();
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
            Debug.Log("전투 시작");
        }
        else
            GameManager.Instance.SetGameState(GameState.Prepare);
    }
}
