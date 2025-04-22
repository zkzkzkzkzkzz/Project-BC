using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

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
    /// ���� ����
    /// </summary>
    public void BattleStart()
    {
        Zone homeZone = ZoneManager.Instance.GetZoneByOwner(0);
        Zone awayZone = ZoneManager.Instance.GetZoneByOwner(1);

        BoardManager homeBoard = homeZone.Board;
        List<Unit> awayUnits = awayZone.Board.GetBoardUnits();

        List<Unit> battleUnits = new List<Unit>();

        foreach (var unit in awayUnits)
        {
            if (unit == null)
            {
                Debug.Log("������ null�Դϴ�.");
                continue;
            }

            battleUnits.Add(unit);
        }

        for (int i = 0; i < battleUnits.Count; ++i)
        {
            Tile fromTile = battleUnits[i].curTile;
            if (fromTile == null) continue;

            Vector3Int originCoord = fromTile.BoardCoord;
            Vector3Int targetCoord = GetOppositeCoordinates(originCoord);

            Tile targetTile = homeBoard.GetTileAt(targetCoord);
            if (targetTile == null)
            {
                Debug.Log($"�����Ǵ� Ÿ���� �����ϴ�: {targetTile}");
                continue;
            }

            UnitPlacementManager.Instance.RequestMove(battleUnits[i], targetTile);
        }
    }


    /// <summary>
    /// ���� ��ǥ 180�� ȸ�����Ѽ� �����Ǵ� Ÿ���� ��ǥ�� ����
    /// </summary>
    private Vector3Int GetOppositeCoordinates(Vector3Int origin)
    {
        int x = origin.x;
        int y = origin.y;
        int z = origin.z;

        return new Vector3Int(-x + 3, -y - 10, -z + 7);
    }
}
