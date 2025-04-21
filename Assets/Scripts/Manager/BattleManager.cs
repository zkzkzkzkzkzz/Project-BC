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
    /// 전투 시작
    /// </summary>
    public void BattleStart()
    {
        //List<Unit> allUnits = BoardManager.Instance.GetBoardUnits();

        //List<Unit> myUnits = new List<Unit>();
        //List<Unit> enemyUnits = new List<Unit>();

        //foreach (Unit unit in allUnits)
        //{
        //    if (unit == null) continue;

        //    if (unit.OwnerId == PlayerSessionManager.Instance.LocalPlayerId)
        //        myUnits.Add(unit);
        //    else
        //        enemyUnits.Add(unit);
        //}

        //myUnits.Sort(new UnitComparer(enemyUnits));

        //foreach (var unit in myUnits)
        //    unit.BattleStartAI();
    }


    //// @@@@ 디버그용 더미 적 생성
    //[SerializeField] private GameObject enemyPrefab;
    //[SerializeField] private float unitYoffset = 1f;
    //public void SpawnDummyEnemy(int count)
    //{
    //    var tiles = BoardManager.Instance.GetBoardTiles();

    //    List<Tile> tileList = tiles.Where(t => !t.IsOccupied() && t.BoardCoord.z > 3f).ToList();

    //    for (int i = 0; i < count; ++i)
    //    { 
    //        int randIdx = Random.Range(0, tileList.Count);
    //        Tile tile = tileList[randIdx];
    //        tileList.RemoveAt(randIdx);
         
    //        Vector3 spawnPos = tile.transform.position + Vector3.up * unitYoffset;
    //        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

    //        Unit enemy = enemyObj.GetComponent<Unit>();
    //        enemy.OwnerId = 1;
    //        enemy.SetCurUnitTile(tile);
    //        tile.SetOccupyingUnit(enemy);

    //        BoardManager.Instance.RegisterUnitToBoard(enemy, tile);

    //        enemyObj.name = $"DummyEnemy_{i}";
    //    }
    //}
}
