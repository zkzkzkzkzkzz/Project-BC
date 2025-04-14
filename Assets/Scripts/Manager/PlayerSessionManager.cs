using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 로컬 플레이어의 ID를 저장하고,
// 전역에서 참조 가능하게 해주는 매니저
public class PlayerSessionManager : MonoBehaviour
{
    public static PlayerSessionManager Instance { get; private set; }

    public int LocalPlayerId { get; private set; }

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

    public void SetLocalPlayerId(int localPlayerId)
    {
        LocalPlayerId = localPlayerId;
    }
}
