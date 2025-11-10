using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathfinderType { DepthFirst, BreadthFirst, Dijkstra, AStar }

public class Traveler
{
    private PathfinderType pathfinderType;
    private Vector2IntGrapf grapf;
    private Pathfinder<Node<Vector2Int>> Pathfinder;
    private Transform transform;
    private Node<Vector2Int> startNode;
    private Node<Vector2Int> destinationNode;

    private List<Node<Vector2Int>> currentPath;


    public Traveler(Vector2IntGrapf grapf, PathfinderType pathfinderType, Transform transform, Node<Vector2Int> startNode, Node<Vector2Int> destinationNode)
    {
        this.grapf = grapf;
        this.transform = transform;
        this.startNode = startNode;
        this.destinationNode = destinationNode;
        this.pathfinderType = pathfinderType;

        SetPathFinderType();
    }

    private void SetPathFinderType()
    {
        switch (pathfinderType)
        {
            case PathfinderType.DepthFirst:
                Pathfinder = new DepthFirstPathfinder<Node<Vector2Int>>();
                break;
            case PathfinderType.BreadthFirst:
                Pathfinder = new BreadthFirstPathfinder<Node<Vector2Int>>();
                break;
            case PathfinderType.Dijkstra:
                Pathfinder = new DijstraPathfinder<Node<Vector2Int>>();
                break;
            case PathfinderType.AStar:
                Pathfinder = new AStarPathfinder<Node<Vector2Int>>();
                break;
            default:
                Pathfinder = new AStarPathfinder<Node<Vector2Int>>();
                break;
        }

        currentPath = new List<Node<Vector2Int>>();
        currentPath = Pathfinder.FindPath(startNode, destinationNode, grapf);
    }
    
    public List<Node<Vector2Int>> GetPath()
    {
        return currentPath;
    }

    public IEnumerator Move()
    {
        foreach (Node<Vector2Int> node in currentPath)
        {
            transform.position = new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public Transform GetTransform() 
    { 
        return transform;
    }
}
