using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float radius;  // 타일 크기

    // 보드 크기
    [SerializeField] private int rows = 8;
    [SerializeField] private int cols = 7;
    [SerializeField] private float gap = 1.1f;

    private List<Tile> boardTiles = new List<Tile>();

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

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
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

                tile.SetTileType(TileType.Board);
                tile.BoardCoord = new Vector3Int(x, y, z);

                tileObj.SetActive(false);
                boardTiles.Add(tile);
            }
        }
    }

    private Vector3 CubeToWorldPointy(int x, int y, int z, float r, float gapFactor)
    {
        float effectiveR = r * gapFactor;
        float worldX = Mathf.Sqrt(3f) * (x + (z * 0.5f)) * effectiveR;
        float worldZ = 3f / 2f * z * effectiveR;
        return new Vector3(worldX, 0f, worldZ);
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
}
