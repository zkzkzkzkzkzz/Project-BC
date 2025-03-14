using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private float radius;  // 타일 크기

    // 보드 크기
    [SerializeField] private int Rows = 7;
    [SerializeField] private int Cols = 8;

    [SerializeField] private float gap = 1.1f;

    private Dictionary<Vector3, GameObject> hexGrid = new Dictionary<Vector3, GameObject>();

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

    void GenerateGrid()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                // odd-r 변환: Axial 좌표로 변경
                // 홀수번째 타일 밀기
                int q = col - (row - (row & 1)) / 2;
                int rCoord = row;

                // Axial -> Cube 변환
                int x = q;
                int z = rCoord;
                int y = -x - z;

                // Cube -> 월드 좌표 (Pointy-topped)
                Vector3 pos = CubeToWorldPointy(x, y, z, radius, gap);

                // 헥스 타일 생성
                GameObject tile = Instantiate(TilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile_{row}_{col} (x:{x}, y:{y}, z:{z})";
                hexGrid[pos] = tile;
                tile.SetActive(false);
            }
        }
    }

    Vector3 CubeToWorldPointy(int x, int y, int z, float r, float gapFactor)
    {
        float effectiveR = r * gapFactor;
        float worldX = Mathf.Sqrt(3f) * (x + (z * 0.5f)) * effectiveR;
        float worldZ = 3f / 2f * z * effectiveR;
        return new Vector3(worldX, 0f, worldZ);
    }

    public void ShowHexGrid(bool show)
    {
        foreach (var tile in hexGrid.Values)
        {
            tile.SetActive(show);
        }
    }
}
