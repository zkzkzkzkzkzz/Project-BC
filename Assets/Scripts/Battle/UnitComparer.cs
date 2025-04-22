using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitComparer : IComparer<Unit>
{
    private readonly List<Unit> enemies;

    public UnitComparer(List<Unit> enemies)
    {
        this.enemies = enemies;
    }

    public int Compare(Unit a, Unit b)
    {
        Unit aTarget = FindClosestEnemy(a);
        Unit bTarget = FindClosestEnemy(b);

        float distA = aTarget != null ? PathFindingSystem.Heuristic(a.curTile, aTarget.curTile) : float.MaxValue;
        float distB = bTarget != null ? PathFindingSystem.Heuristic(b.curTile, bTarget.curTile) : float.MaxValue;

        if (distA != distB)
            return distA.CompareTo(distB);

        Vector3Int aCoord = a.curTile.BoardCoord;
        Vector3Int bCoord = b.curTile.BoardCoord;

        if (aCoord.z != bCoord.z)
            return aCoord.z.CompareTo(bCoord.z);

        return aCoord.x.CompareTo(bCoord.x);
    }

    private Unit FindClosestEnemy(Unit unit)
    {
        float minDist = float.MaxValue;
        Unit closestUnit = null;

        foreach (var enemy in enemies)
        {
            float dist = PathFindingSystem.Heuristic(unit.curTile, enemy.curTile);
            if (dist < minDist)
            {
                minDist = dist;
                closestUnit = enemy;
            }
        }

        return closestUnit;
    }
}
