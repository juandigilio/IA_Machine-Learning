using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstPathfinder<NodeType> : Pathfinder<NodeType> where NodeType : INode<Vector2Int>, INode
{
    public override List<NodeType> FindPath(NodeType startNode, NodeType destinationNode, IGraph<NodeType> graph)
    {
        Dictionary<NodeType, NodeType> parents = new Dictionary<NodeType, NodeType>();
        HashSet<NodeType> visited = new HashSet<NodeType>();
        Queue<NodeType> queue = new Queue<NodeType>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            NodeType current = queue.Dequeue();

            if (current.Equals(destinationNode))
            {
                return GeneratePath(startNode, destinationNode, parents);
            }

            foreach (NodeType neighbor in graph.GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && !neighbor.IsBlocked())
                {
                    visited.Add(neighbor);
                    parents[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return new List<NodeType>();
    }

    private List<NodeType> GeneratePath(NodeType startNode, NodeType goalNode, Dictionary<NodeType, NodeType> parents)
    {
        List<NodeType> path = new List<NodeType>();
        NodeType current = goalNode;

        while (!current.Equals(startNode))
        {
            path.Add(current);
            current = parents[current];
        }

        path.Add(startNode);
        path.Reverse();
        return path;
    }

    protected override int Distance(NodeType A, NodeType B, IGraph<NodeType> graph)
    {
        return 0;
    }
}
