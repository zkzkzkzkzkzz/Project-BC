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

    // �׸��� Ȱ��ȭ / ��Ȱ��ȭ
    [SerializeField] private Button GridBtn;
    private bool bHexGrid = false;

    public void ToggleHexGrid()
    {
        bHexGrid = !bHexGrid;
        BoardManager.Instance.ShowHexGrid(bHexGrid);
    }
}
