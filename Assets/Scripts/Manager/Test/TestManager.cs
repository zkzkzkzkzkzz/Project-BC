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
    /// ���� ����. ���� �� ��ġ ����
    /// </summary>
    public void GameStart()
    {

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
}
