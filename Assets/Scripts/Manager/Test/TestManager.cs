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
    /// �׸��� Ȱ��ȭ / ��Ȱ��ȭ
    /// </summary>
    private bool bGrid = false;
    public void ToggleGrid()
    {
        bGrid = !bGrid;
        BoardManager.Instance.ShowHexGrid(bGrid);
        BenchManager.Instance.ShowBenchGrid(bGrid);
    }

    /// <summary>
    /// ���� ����
    /// ���ŵ� ������ ��ġ�� ��ġ
    /// </summary>
    public void PurchaseUnit()
    {
        BenchManager.Instance.PlaceUnitOnBench();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void BattleStart()
    {
        GameManager.Instance.SetGameState(GameState.Battle);
        BattleManager.Instance.BattleStart();
        Debug.Log("���� ����");
    }
}
