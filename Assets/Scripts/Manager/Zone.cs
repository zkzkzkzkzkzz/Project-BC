using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
    public int OwnerId { get; private set; }
    public BoardManager Board {  get; private set; }
    public BenchManager Bench {  get; private set; }
    public Transform UnitsRoot { get; private set; }
    public Transform ZoneRoot { get; private set; }

    public void Initialize(int ownerId, Transform zoneRoot)
    {
        OwnerId = ownerId;
        ZoneRoot = zoneRoot;

        UnitsRoot = new GameObject("Units").transform;
        UnitsRoot.SetParent(ZoneRoot);
        UnitsRoot.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 이 zone이 로컬 플레이어의 zone인지 확인
    /// </summary>
    public bool IsMyZone()
    {
        return OwnerId == PlayerSessionManager.Instance.LocalPlayerId;
    }

    /// <summary>
    /// 보드/벤치 매니저 설정 (초기화 용)
    /// </summary>
    public void SetBoardAndBench(BoardManager board, BenchManager bench)
    {
        Board = board;
        Bench = bench;
    }
}
