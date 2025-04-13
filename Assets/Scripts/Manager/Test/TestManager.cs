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
    [SerializeField] private Button GridBtn;
    private bool bGrid = false;
    public void ToggleGrid()
    {
        bGrid = !bGrid;
        BoardManager.Instance.ShowHexGrid(bGrid);
        BenchManager.Instance.ShowBenchGrid(bGrid);
    }

    [SerializeField] private Button PurchaseBtn;
    public void PurchaseUnit()
    {
        BenchManager.Instance.PlaceUnitOnBench();
    }
}
