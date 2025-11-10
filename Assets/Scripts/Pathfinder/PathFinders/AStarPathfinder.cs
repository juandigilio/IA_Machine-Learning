using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder<NodeType> : Pathfinder<NodeType> where NodeType : INode<Vector2Int>, INode
{
    public override List<NodeType> FindPath(NodeType startNode, NodeType destinationNode, IGraph<NodeType> graph)
    {
        Dictionary<NodeType, (NodeType Parent, int AcumulativeCost, int Heuristic)> nodes =
            new Dictionary<NodeType, (NodeType Parent, int AcumulativeCost, int Heuristic)>();

        foreach (NodeType node in graph.GetAllNodes())
        {
            nodes.Add(node, (default, int.MaxValue, 0));
        }

        nodes[startNode] = (default, 0, Distance(startNode, destinationNode, graph));

        List<NodeType> openList = new List<NodeType>();
        List<NodeType> closedList = new List<NodeType>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            NodeType currentNode = openList[0];
            int currentIndex = 0;

            for (int i = 1; i < openList.Count; i++)
            {
                if (nodes[openList[i]].AcumulativeCost + nodes[openList[i]].Heuristic <
                nodes[currentNode].AcumulativeCost + nodes[currentNode].Heuristic)
                {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }

            openList.RemoveAt(currentIndex);
            closedList.Add(currentNode);

            if (currentNode.Equals(destinationNode))
            {
                return GeneratePath(startNode, destinationNode);
            }

            foreach (NodeType neighbor in graph.GetNeighbors(currentNode))
            {
                if (!nodes.ContainsKey(neighbor) ||
                neighbor.IsBlocked() ||
                closedList.Contains(neighbor))
                {
                    continue;
                }

                int tentativeNewAcumulatedCost = 0;
                tentativeNewAcumulatedCost += nodes[currentNode].AcumulativeCost;
                tentativeNewAcumulatedCost += graph.MoveToNeighborCost(neighbor);

                if (!openList.Contains(neighbor) || tentativeNewAcumulatedCost < nodes[neighbor].AcumulativeCost)
                {
                    nodes[neighbor] = (currentNode, tentativeNewAcumulatedCost, Distance(neighbor, destinationNode, graph));

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return new List<NodeType>();

        List<NodeType> GeneratePath(NodeType startNode, NodeType goalNode)
        {
            List<NodeType> path = new List<NodeType>();
            NodeType currentNode = goalNode;

            while (!currentNode.Equals(startNode))
            {
                path.Add(currentNode);
                currentNode = nodes[currentNode].Parent;
            }

            path.Add(startNode);
            path.Reverse();
            return path;
        }
    }
}
