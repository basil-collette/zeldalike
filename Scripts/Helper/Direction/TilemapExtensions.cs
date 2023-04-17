using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public static class TilemapExtensions
{
    public static Vector3Int[] GetPath(this Tilemap tilemap, Vector3Int start, Vector3Int end)
    {
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
        Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

        List<Vector3Int> openSet = new List<Vector3Int>();
        openSet.Add(start);

        gScore[start] = 0;
        fScore[start] = HeuristicCostEstimate(start, end);

        while (openSet.Count > 0)
        {
            Vector3Int current = LowestFScore(openSet, fScore);
            if (current == end)
            {
                return ReconstructPath(cameFrom, end);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector3Int neighbor in GetNeighbors(tilemap, current))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + DistanceBetween(current, neighbor);
                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, end);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private static Vector3Int[] ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path.ToArray();
    }

    private static float HeuristicCostEstimate(Vector3Int start, Vector3Int end)
    {
        return DistanceBetween(start, end);
    }

    private static float DistanceBetween(Vector3Int start, Vector3Int end)
    {
        return Vector3Int.Distance(start, end);
    }

    private static Vector3Int LowestFScore(List<Vector3Int> set, Dictionary<Vector3Int, float> fScore)
    {
        float lowest = float.MaxValue;
        Vector3Int lowestNode = set[0];

        foreach (Vector3Int node in set)
        {
            if (fScore.ContainsKey(node) && fScore[node] < lowest)
            {
                lowest = fScore[node];
                lowestNode = node;
            }
        }

        return lowestNode;
    }

    private static List<Vector3Int> GetNeighbors(Tilemap tilemap, Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        if (IsTileWalkable(tilemap, position + Vector3Int.up))
        {
            neighbors.Add(position + Vector3Int.up);
        }
        if (IsTileWalkable(tilemap, position + Vector3Int.down))
        {
            neighbors.Add(position + Vector3Int.down);
        }
        if (IsTileWalkable(tilemap, position + Vector3Int.left))
        {
            neighbors.Add(position + Vector3Int.left);
        }
        if (IsTileWalkable(tilemap, position + Vector3Int.right))
        {
            neighbors.Add(position + Vector3Int.right);
        }

        return neighbors;
    }

    private static bool IsTileWalkable(Tilemap tilemap, Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        return (tile == null || tilemap.GetTileFlags(position) != TileFlags.None);
    }

}
