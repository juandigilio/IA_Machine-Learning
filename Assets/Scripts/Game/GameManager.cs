using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //mines
    [SerializeField] private AgentsManager agentManager;

    //grapf
    [SerializeField] private Vector2Int startCoordinates = new Vector2Int(0, 0);
    [SerializeField] private Vector2Int destinationCoordinates = new Vector2Int(9, 9);   
    [SerializeField] private bool resetPathfinder = true;
    [SerializeField] private bool resetGrapf = true;
    [SerializeField] private bool drawGizmos = true;

    //Testing
    [SerializeField] private Transform testTransform;
    private Vector2Int testNodeCoordinates;

    private Node<Vector2Int> startNode;
    private Node<Vector2Int> destinationNode;
    private Vector2IntGrapf grapf;
    private GrapfView grapfView = new GrapfView();
    private Traveler traveler;

    private Voronoi2D voronoi;

    private void Start()
    {
        resetPathfinder = true;
        resetGrapf = true;

        ResetGapf();
        ResetPathFinder();
    }

    private void Update()
    {
        ResetGapf();
        ResetPathFinder(); 
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        { 
            return;
        }

        grapfView.DrawGizmos();
        //voronoi.DrawGizmos(traveler.GetTransform());
        //voronoi.DrawGizmos(testTransform);

        if (grapf != null)
        {
            foreach (Voronoi2DPoint point in voronoi.GetAllPoints())
            {
                if (point.drawGizmos != drawGizmos)
                {
                    point.drawGizmos = drawGizmos;
                }
                
                point.DrawGizmos();
            }
        }
    }

    private void ResetGapf()
    {
        if (resetGrapf)
        {
            resetGrapf = false;

            grapf = new Vector2IntGrapf(GameData.width, GameData.height, GameData.nodeDistance);

            agentManager.SetGrapf(grapf);
            agentManager.InstantiateAgents();

            startNode = grapf.GetNodeAt(startCoordinates);
            destinationNode = grapf.GetNodeAt(destinationCoordinates);

            grapfView.SetGraph(grapf, startNode, destinationNode);

            //voronoi

            List<Node<Vector2Int>> carnivorousNodes = new List<Node<Vector2Int>>();

            foreach(Carnivorous agent in agentManager.GetCarnivorous())
            {
                carnivorousNodes.Add(agent.GetNode());
            }        
            voronoi = new Voronoi2D(carnivorousNodes, grapf);


            //Testing
            testNodeCoordinates = grapf.GetNodeAt(0, 0).GetCoordinate();
            testTransform.position = WorldPositionAt(testNodeCoordinates);
        }
    }

    private void ResetPathFinder()
    {
        if (resetPathfinder)
        {
            resetPathfinder = false;

            //traveler = new Traveler(grapf, GameData.pathfinderType, minerPrefab, startNode, destinationNode);
            //StopAllCoroutines();
            //StartCoroutine(traveler.Move());
        }      
    }

    //Testing

    public void Move(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            testNodeCoordinates.y += 1;

            if (testNodeCoordinates.y >= GameData.height)
            {
                testNodeCoordinates.y -= GameData.height;
            }
        }
        else if(direction == Vector2.down)
        {
            testNodeCoordinates.y -= 1;

            if (testNodeCoordinates.y < 0)
            {
                testNodeCoordinates.y += GameData.height;
            }
        }
        else if (direction == Vector2.right)
        {
            testNodeCoordinates.x += 1;

            if (testNodeCoordinates.x >= GameData.width)
            {
                testNodeCoordinates.x -= GameData.width;
            }
        }
        else if (direction == Vector2.left)
        {
            testNodeCoordinates.x -= 1;

            if (testNodeCoordinates.x < 0)
            {
                testNodeCoordinates.x += GameData.width;
            }
        }

        testTransform.position = WorldPositionAt(testNodeCoordinates);
    }

    private Vector3 WorldPositionAt(Vector2Int coordinates)
    {
        Vector3 worldPosition = new Vector3(grapf.GetNodeAt(coordinates).GetWorldPosition().x, grapf.GetNodeAt(coordinates).GetWorldPosition().y);
        return worldPosition;
    }
}
