using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Prepare,    // 정비
    Battle,     // 전투
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState state = GameState.Prepare;
    public GameState CurState => state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetGameState(GameState _state)
    {
        state = _state;
    }

    public bool IsInPrepare() => state == GameState.Prepare;
    public bool IsInBattle() => state == GameState.Battle;
}
