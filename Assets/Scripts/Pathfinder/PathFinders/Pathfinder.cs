using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder<NodeType> where NodeType : INode<Vector2Int>, INode
{
    public abstract List<NodeType> FindPath(NodeType startNode, NodeType destinationNode, IGraph<NodeType> graph);

    protected virtual int Distance(NodeType a, NodeType b, IGraph<NodeType> graph)
    {
        Vector2Int ac = a.GetCoordinate();
        Vector2Int bc = b.GetCoordinate();

        int dx = Mathf.Abs(ac.x - bc.x);
        int dy = Mathf.Abs(ac.y - bc.y);

        dx = Mathf.Min(dx, graph.GetSize().x - dx);
        dy = Mathf.Min(dy, graph.GetSize().y - dy);

        return dx + dy;
    }
}