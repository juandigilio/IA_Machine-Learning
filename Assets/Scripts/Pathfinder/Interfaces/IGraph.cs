using System.Collections.Generic;
using UnityEngine;

public interface IGraph<NodeType>
{
    ICollection<NodeType> GetNeighbors(NodeType node);
    bool IsBlocked(NodeType node);
    int MoveToNeighborCost(NodeType neighbor);
    ICollection<NodeType> GetAllNodes();
    public Vector2Int GetSize();
}