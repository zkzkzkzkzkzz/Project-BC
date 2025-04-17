using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �÷��̾��� ID�� �����ϰ�,
// �������� ���� �����ϰ� ���ִ� �Ŵ���
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
