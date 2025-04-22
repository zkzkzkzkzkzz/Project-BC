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
    /// �� zone�� ���� �÷��̾��� zone���� Ȯ��
    /// </summary>
    public bool IsMyZone()
    {
        return OwnerId == PlayerSessionManager.Instance.LocalPlayerId;
    }

    /// <summary>
    /// ����/��ġ �Ŵ��� ���� (�ʱ�ȭ ��)
    /// </summary>
    public void SetBoardAndBench(BoardManager board, BenchManager bench)
    {
        Board = board;
        Bench = bench;
    }
}
