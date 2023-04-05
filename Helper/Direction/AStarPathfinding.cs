using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    private class Node
    {
        public Vector2 Position { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }
        public Node ParentNode { get; set; }

        public Node(Vector2 position)
        {
            Position = position;
            GCost = 0;
            HCost = 0;
            ParentNode = null;
        }
    }

    public static List<Vector2> FindPath(Vector2 startPosition, Vector2 endPosition)
    {
        List<Vector2> path = new List<Vector2>();

        // Create open and closed lists
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        // Create start and end nodes
        Node startNode = new Node(startPosition);
        Node endNode = new Node(endPosition);

        // Add start node to open list
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Find node with lowest F cost
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost ||
                    (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            // Remove current node from open list and add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If end node is found, return path
            if (currentNode == endNode)
            {
                while (currentNode != startNode)
                {
                    path.Add(currentNode.Position);
                    currentNode = currentNode.ParentNode;
                }
                path.Reverse();
                return path;
            }

            // Get adjacent nodes
            List<Node> adjacentNodes = GetAdjacentNodes(currentNode);
            foreach (Node adjacentNode in adjacentNodes)
            {
                // Skip if already in closed list
                if (closedList.Contains(adjacentNode))
                {
                    continue;
                }

                // Calculate new G cost
                float newGCost = currentNode.GCost + Vector2.Distance(currentNode.Position, adjacentNode.Position);

                // If adjacent node is not in open list, add it
                if (!openList.Contains(adjacentNode))
                {
                    openList.Add(adjacentNode);
                }
                else if (newGCost >= adjacentNode.GCost)
                {
                    // Skip if new G cost is not better
                    continue;
                }

                // Update adjacent node's values
                adjacentNode.GCost = newGCost;
                adjacentNode.HCost = Vector2.Distance(adjacentNode.Position, endNode.Position);
                adjacentNode.ParentNode = currentNode;
            }
        }

        // Path not found
        return null;
    }

    private static List<Node> GetAdjacentNodes(Node node)
    {
        List<Node> adjacentNodes = new List<Node>();
        Vector2[] directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        foreach (Vector2 direction in directions)
        {
            Vector2 position = node.Position + direction;
            Node adjacentNode = new Node(position);
            adjacentNodes.Add(adjacentNode);
        }

        return adjacentNodes;
    }

}
