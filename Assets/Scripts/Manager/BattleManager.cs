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

    [SerializeField] private int myOwnerId = 0;
    private List<Unit> myUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

    /// <summary>
    /// ���� ���
    /// </summary>
    public void RegisterUnit(Unit unit)
    {
        if (unit.OwnerId == myOwnerId)
            myUnits.Add(unit);
        else
            enemyUnits.Add(unit);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void UnregisterUnit(Unit unit)
    {
        if (unit.OwnerId == myOwnerId)
            myUnits.Remove(unit);
        else
            enemyUnits.Remove(unit);
    }




    /// <summary>
    /// ���� ����
    /// </summary>
    public void BattleStart()
    {
        //List<Unit> sortedUnits = SortedMyUnits(0);
        //foreach (Unit unit in sortedUnits)
        //    unit.BattleStartAI();

        //List<Unit> sortedEnemies = SortedMyUnits(1);
        //foreach (Unit unit in sortedEnemies)
        //    unit.BattleStartAI();

        myUnits.Sort(new UnitComparer(enemyUnits));

        foreach (var unit in myUnits)
            unit.BattleStartAI();
    }


    // @@@@ ����׿� ���� �� ����
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float unitYoffset = 1f;
    public void SpawnDummyEnemy(int count)
    {
        var tiles = BoardManager.Instance.GetBoardTiles();

        List<Tile> tileList = tiles.Where(t => !t.IsOccupied() && t.BoardCoord.z > 3f).ToList();

        for (int i = 0; i < count; ++i)
        { 
            int randIdx = Random.Range(0, tileList.Count);
            Tile tile = tileList[randIdx];
            tileList.RemoveAt(randIdx);
         
            Vector3 spawnPos = tile.transform.position + Vector3.up * unitYoffset;
            GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Unit enemy = enemyObj.GetComponent<Unit>();
            enemy.SetOwnerId(GetEnemyOwnerId());
            enemy.SetCurUnitTile(tile);
            tile.SetOccupyingUnit(enemy);

            RegisterUnit(enemy);

            enemyObj.name = $"DummyEnemy_{i}";
        }
    }

    /// <summary>
    /// ���� �÷��̾��� �� OwnerId ��ȯ (�׽�Ʈ��)
    /// </summary>
    private int GetEnemyOwnerId()
    {
        return (myOwnerId == 0) ? 1 : 0;
    }
}
