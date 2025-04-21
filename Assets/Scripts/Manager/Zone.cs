using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public int OwnerId { get; private set; }
    public BoardManager Board {  get; private set; }
    public BenchManager Bench {  get; private set; }

    public void Initialize(int ownerId, BoardManager board, BenchManager bench)
    {
        OwnerId = ownerId;
        Board = board;
        Bench = bench;
    }

    /// <summary>
    /// �� zone�� ���� �÷��̾��� zone���� Ȯ��
    /// </summary>
    public bool IsMyZone()
    {
        return OwnerId == PlayerSessionManager.Instance.LocalPlayerId;
    }
}
