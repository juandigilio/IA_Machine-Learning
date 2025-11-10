using System.Collections.Generic;
using UnityEngine;


public class Vector2IntGrapf : IGraph<Node<Vector2Int>>
{
    [SerializeField] private UrbanCenter urbanCenter;

    private List<Node<Vector2Int>> nodes = new List<Node<Vector2Int>>();
    private Dictionary<Vector2Int, Node<Vector2Int>> lookup = new Dictionary<Vector2Int, Node<Vector2Int>>();
    private int width;
    private int height;
    private int nodeDistance;

    public Vector2IntGrapf(int x, int y, int distance) 
    {
        width = x;
        height = y;
        nodeDistance = distance;

        nodes = new List<Node<Vector2Int>>(x * y);
        lookup = new Dictionary<UnityEngine.Vector2Int, Node<Vector2Int>>(x * y);

        int acumulatedY = 0;
        for (int i = 0; i < x; i++)
        {
            int acumulatedX = 0;
            for (int j = 0; j < y; j++)
            {
                Node<Vector2Int> node = new Node<Vector2Int>();
                Vector2Int coord = new Vector2Int(i, j);
                node.SetCoordinate(coord);
                node.SetTerrainCost(UnityEngine.Random.Range(1, 11));

                node.SetWorldPosition(new Vector2Int(i * nodeDistance, j * nodeDistance));

                nodes.Add(node);
                lookup[coord] = node;

                acumulatedX += nodeDistance;
            }
            acumulatedY += nodeDistance;
        }
    }

    public Node<Vector2Int> GetNodeAt(int x, int y)
    {
        lookup.TryGetValue(new Vector2Int(x, y), out Node<Vector2Int> node);
        return node;
    }

    public Node<Vector2Int> GetNodeAt(Vector2Int v)
    {
        return GetNodeAt(v.x, v.y);
    }

    public ICollection<Node<Vector2Int>> GetNeighbors(Node<Vector2Int> node)
    {
        List<Node<Vector2Int>> neighbors = new List<Node<Vector2Int>>();
        Vector2Int coord = node.GetCoordinate();

        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(0,1)
        };

        foreach (Vector2Int dir in dirs)
        {
            int newX = (coord.x + dir.x + width) % width;
            int newY = (coord.y + dir.y + height) % height;

            Vector2Int wrappedCoord = new Vector2Int(newX, newY);

            if (lookup.TryGetValue(wrappedCoord, out Node<Vector2Int> newNode))
            {
                neighbors.Add(newNode);
            }
        }
        return neighbors;
    }

    public bool IsBlocked(Node<Vector2Int> node)
    {
        return node.IsBlocked();
    }

    public ICollection<Node<Vector2Int>> GetAllNodes() => nodes;

    public int MoveToNeighborCost(Node<Vector2Int> neighbor)
    {
        return neighbor.GetTerrainCost();
    }

    public Vector2Int GetSize()
    {
        return new Vector2Int(width, height);
    }
}
