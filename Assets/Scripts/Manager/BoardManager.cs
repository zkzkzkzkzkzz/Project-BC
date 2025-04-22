using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float radius;  // 타일 크기

    // 보드 크기
    [SerializeField] private int rows = 8;
    [SerializeField] private int cols = 7;
    [SerializeField] private float gap = 1.1f;

    private List<Tile> boardTiles = new List<Tile>();
    [SerializeField] private List<Unit> boardUnits = new List<Unit>();

    private void Start()
    {
        //GenerateGrid();
    }

    public void GenerateGrid(int ownerId)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // odd-r offset → axial → cube 좌표 변환
                int q = col - (row - (row & 1)) / 2;
                int rCoord = row;

                int x = q;
                int z = rCoord;
                int y = -x - z;

                Vector3 pos = CubeToWorldPointy(x, y, z, radius, gap);

                GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tileObj.name = $"Tile_{row}_{col} (x:{x}, y:{y}, z:{z})";

                Tile tile = tileObj.GetComponent<Tile>();
                if (tile == null)
                    tile = tileObj.AddComponent<Tile>();

                Zone zone = ZoneManager.Instance.GetZoneByOwner(ownerId);
                if (zone == null)
                    Debug.Log("해당 id에 맞는 zone이 없습니다.");

                tile.zone = ZoneManager.Instance.GetZoneByOwner(ownerId);
                tile.SetTileType(TileType.Board);
                tile.BoardCoord = new Vector3Int(x, y, z);

                //tileObj.SetActive(false);
                boardTiles.Add(tile);
                boardUnits.Add(null);
            }
        }
    }

    private Vector3 CubeToWorldPointy(int x, int y, int z, float r, float gapFactor)
    {
        float effectiveR = r * gapFactor;
        float localX = Mathf.Sqrt(3f) * (x + (z * 0.5f)) * effectiveR;
        float localZ = 3f / 2f * z * effectiveR;    

        Vector3 localPos = new Vector3(localX, 0f, localZ);
        return transform.position + localPos;
    }

    /// <summary>
    /// 현재 좌표 180도 회전시켜서 대응되는 타일의 좌표를 리턴
    /// </summary>
    private Vector3Int GetOppositeCoordinates(Vector3Int origin)
    {
        int x = origin.x;
        int y = origin.y;
        int z = origin.z;

        return new Vector3Int(-x + 3, -y - 10, -z + 7);
    }


    /// <summary>
    /// 보드 리스트에 유닛 등록
    /// </summary>
    public void RegisterUnitToBoard(Unit unit, Tile to)
    {
        int idx = boardTiles.IndexOf(to);
        if (idx >= 0)
            boardUnits[idx] = unit;
    }

    /// <summary>
    /// 보드 리스트에서 유닛 해제
    /// </summary>
    public void UnregisterUnitFromBoard(Unit unit)
    {
        int idx = boardUnits.IndexOf(unit);
        if (idx >= 0)
            boardUnits[idx] = null;
    }



    public void ShowHexGrid(bool show)
    {
        foreach (var tile in boardTiles)
        {
            tile.gameObject.SetActive(show);
        }
    }

    public bool IsBoardTile(Tile tile)
    {
        return tile.GetTileType() == TileType.Board;
    }

    public List<Tile> GetBoardTiles()
    {
        return boardTiles;
    }

    public Tile GetTileAt(Vector3Int cubeCoord)
    {
        foreach (var tile in boardTiles)
            if (tile.BoardCoord == cubeCoord)
                return tile;

        return null;
    }

    public List<Unit> GetBoardUnits()
    {
        return boardUnits;
    }
}
