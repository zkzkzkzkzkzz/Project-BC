using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
    public int OwnerId { get; private set; }
    public BoardManager Board {  get; private set; }
    public BenchManager Bench {  get; private set; }
    public Transform UnitsRoot { get; private set; }

    public void Initialize(int ownerId, BoardManager board, BenchManager bench)
    {
        OwnerId = ownerId;
        Board = board;
        Bench = bench;

        UnitsRoot = new GameObject("Units").transform;
        UnitsRoot.SetParent(board.transform.parent);
        UnitsRoot.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 이 zone이 로컬 플레이어의 zone인지 확인
    /// </summary>
    public bool IsMyZone()
    {
        return OwnerId == PlayerSessionManager.Instance.LocalPlayerId;
    }
}
