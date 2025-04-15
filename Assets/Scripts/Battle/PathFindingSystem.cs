using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PathFindingSystem
{
    private static readonly Vector3Int[] CubeDirections = new Vector3Int[]
    {
        new Vector3Int(1, -1, 0), new Vector3Int(1, 0, -1),
        new Vector3Int(0, 1, -1), new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 0, 1), new Vector3Int(0, -1, 1)
    };

    // ��� �̿� Ÿ���� Ž���ϸ鼭 �ִ� ��ΰ� Ȯ���Ǹ� �ش� ��θ� ��ȯ
    public static List<Tile> FindPath(Tile start, Tile end)
    {
        var openSet = new List<Tile> { start };                 // ���� Ž������ ���� �ĺ� Ÿ�� ���
        var cameFrom = new Dictionary<Tile, Tile>();            // ��� �������� ���� ����
        var gScore = new Dictionary<Tile, int> { [start] = 0 }; // ���������� �� �������� ���� �Ÿ�

        // ���� �ִ� �Ÿ� (f = g + h)
        var fScore = new Dictionary<Tile, float> { [start] = Heuristic(start, end) };

        while (openSet.Count > 0)
        {
            Tile curTile = GetLowestFScore(openSet, fScore);
            if (curTile == end)
                return ReconstructPath(cameFrom, curTile);

            openSet.Remove(curTile);

            foreach (var neighbor in GetNeighbors(curTile))
            {
                if (neighbor.IsOccupied() && neighbor != end) continue;

                int tentativeG = gScore.ContainsKey(curTile) ? gScore[curTile] + 1 : int.MaxValue;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = curTile;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }


    private static Tile GetLowestFScore(List<Tile> openSet, Dictionary<Tile, float> fScore)
    {
        Tile lowest = openSet[0];
        float lowestScore = fScore.ContainsKey(lowest) ? fScore[lowest] : float.MaxValue;

        foreach (var tile in openSet)
        {
            float score = fScore.ContainsKey(tile) ? fScore[tile] : float.MaxValue;
            if (score < lowestScore)
            {
                lowest = tile;
                lowestScore = score;
            }
        }

        return lowest;
    }


    private static List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile curTile)
    {
        var path = new List<Tile> { curTile };
        while (cameFrom.ContainsKey(curTile))
        {
            curTile = cameFrom[curTile];
            path.Insert(0, curTile);
        }

        return path;
    }


    public static List<Tile> GetNeighbors(Tile tile)
    {
        var neighbors = new List<Tile>();

        for (int i = 0; i < CubeDirections.Length; ++i)
        {
            InfiniteLoopDetector.Run();
            Vector3Int neighborCoord = tile.BoardCoord + CubeDirections[i];
            Tile neighbor = BoardManager.Instance.GetTileAt(neighborCoord);

            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    public static float Heuristic(Tile a, Tile b)
    {
        Vector3Int ac = a.BoardCoord;
        Vector3Int bc = b.BoardCoord;

        return (Mathf.Abs(ac.x - bc.x) + Mathf.Abs(ac.y - bc.y) + Mathf.Abs(ac.z - bc.z)) / 2f;
    }
}
