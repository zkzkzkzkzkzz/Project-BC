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



    public void BattleStart()
    {

    }



    // @@@@ 디버그용 더미 적 생성
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

            enemy.SetOwnerId(1);
            enemy.SetCurUnitTile(tile);
            tile.SetOccupyingUnit(enemy);

            enemyObj.name = $"DummyEnemy_{i}";
        }
    }
}
