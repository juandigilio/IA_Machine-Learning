using System.Collections.Generic;
using UnityEngine;

public class DijstraPathfinder<NodeType> : Pathfinder<NodeType> where NodeType : INode<Vector2Int>, INode
{
    public override List<NodeType> FindPath(NodeType startNode, NodeType destinationNode, IGraph<NodeType> graph)
    {
        Dictionary<NodeType, (NodeType Parent, int Cost)> nodes = new Dictionary<NodeType, (NodeType, int)>();
        List<NodeType> openList = new List<NodeType>();
        HashSet<NodeType> closedList = new HashSet<NodeType>();

        foreach (NodeType node in graph.GetAllNodes())
        {
            nodes[node] = (default, int.MaxValue);
        }

        nodes[startNode] = (default, 0);  

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            NodeType currentNode = openList[0];
            int currentIndex = 0;

            for (int i = 1; i < openList.Count; i++)
            {
                if (nodes[openList[i]].Cost < nodes[currentNode].Cost)
                {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }
            openList.RemoveAt(currentIndex);

            if (currentNode.Equals(destinationNode))
            {
                return GeneratePath(startNode, destinationNode, nodes);
            }

            closedList.Add(currentNode);

            foreach (NodeType neighbor in graph.GetNeighbors(currentNode))
            {
                if (neighbor.IsBlocked() || closedList.Contains(neighbor))
                    continue;

                int newCost = nodes[currentNode].Cost + neighbor.GetTerrainCost();
                int otherCost = nodes[neighbor].Cost;

                if (newCost < otherCost)
                {
                    nodes[neighbor] = (currentNode, newCost);

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return new List<NodeType>();
    }

    private List<NodeType> GeneratePath(NodeType startNode, NodeType goalNode, Dictionary<NodeType, (NodeType Parent, int Cost)> nodes)
    {
        List<NodeType> path = new List<NodeType>();
        NodeType current = goalNode;

        while (!current.Equals(startNode))
        {
            if (!nodes.ContainsKey(current) || nodes[current].Parent == null)
            {
                Debug.LogError("No se pudo reconstruir el camino.");
                return new List<NodeType>();
            }

            path.Add(current);
            current = nodes[current].Parent;
        }

        path.Add(startNode);
        path.Reverse();

        return path;
    }

    protected override int Distance(NodeType a, NodeType b, IGraph<NodeType> graph)
    {
        return 0;
    }
}
