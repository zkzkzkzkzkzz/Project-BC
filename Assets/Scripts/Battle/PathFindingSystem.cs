using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PathFindingSystem
{
    private static Vector3[] CubeDirections = new Vector3[]
    {
        new Vector3(1, -1, 0), new Vector3(-1, 1, 0),
        new Vector3(1, 0, -1), new Vector3(-1, 0, 1),
        new Vector3(0, 1, -1), new Vector3(0, -1, 1)
    };

    // ��� �̿� Ÿ���� Ž���ϸ鼭 �ִ� ��ΰ� Ȯ���Ǹ� �ش� ��θ� ��ȯ
    public static List<Tile> FindPath(Tile start, Tile end)
    {
        var openSet = new List<Tile> { start };                 // ���� Ž������ ���� �ĺ� Ÿ�� ���
        var cameFrom = new Dictionary<Tile, Tile>();            // ��� �������� ���� ����
        var gScore = new Dictionary<Tile, int> { [start] = 0 }; // ���������� �� �������� ���� �Ÿ�

        // ���� �ִ� �Ÿ� (f = g + h)
        var fScore = new Dictionary<Tile, int> { [start] = Heuristic(start, end) };

        while (openSet.Count > 0)
        {
            Tile curTile = GetLowestFScore(openSet, fScore);
            if (curTile == end)
                return ReconstructPath(cameFrom, curTile);

            openSet.Add(curTile);

            foreach (var neighbor in GetNeighbors(curTile))
            {
                if (neighbor.IsOccupied()) continue;

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


    private static Tile GetLowestFScore(List<Tile> openSet, Dictionary<Tile, int> fScore)
    {
        Tile lowest = openSet[0];
        int lowestScore = fScore.ContainsKey(lowest) ? fScore[lowest] : int.MaxValue;

        foreach (var tile in openSet)
        {
            int score = fScore.ContainsKey(tile) ? fScore[tile] : int.MaxValue;
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


    private static List<Tile> GetNeighbors(Tile tile)
    {
        var neighbors = new List<Tile>();
        foreach (var dir in CubeDirections)
        {
            Vector3 neighborCoord = Vector3Int.RoundToInt(tile.BoardCoord) + dir;
            Tile neighbor = BoardManager.Instance.GetTileAt(neighborCoord);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    private static int Heuristic(Tile a, Tile b)
    {
        Vector3Int ac = Vector3Int.RoundToInt(a.BoardCoord);
        Vector3Int bc = Vector3Int.RoundToInt(b.BoardCoord);

        return (Mathf.Abs(ac.x - bc.x) + Mathf.Abs(ac.y - bc.y) + Mathf.Abs(ac.z - bc.z)) / 2;
    }
}
