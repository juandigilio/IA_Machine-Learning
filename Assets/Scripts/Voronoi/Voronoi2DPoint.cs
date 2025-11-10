using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Voronoi2DPoint
{
    private Vector2IntGrapf grapf;
    private Vector2 grapfSize = new Vector2();
    public Node<Vector2Int> node;
    private List<Vector2> normalsA = new List<Vector2>();
    private List<Vector2> normalsB = new List<Vector2>();
    private List<Vector2> midPointsA = new List<Vector2>();
    private List<Vector2> midPointsB = new List<Vector2>();
    private List<Vector2> normalizedPoints = new List<Vector2>();
    public List<Plane> nearestPlanesA = new List<Plane>();
    public List<Plane> nearestPlanesB = new List<Plane>();
    private int colorID;
    public bool drawGizmos = true;

    public Voronoi2DPoint(Node<Vector2Int> node, int colorID, Vector2IntGrapf grapf)
    {
        this.node = node;
        this.colorID = colorID;
        this.grapf = grapf;
        grapfSize = grapf.GetSize();
    }

    public void SetPlanes(List<Voronoi2DPoint> neighbors)
    {
        normalsA.Clear();
        normalsB.Clear();
        midPointsA.Clear();
        midPointsB.Clear();
        nearestPlanesA.Clear();
        nearestPlanesB.Clear();

        foreach (Voronoi2DPoint point in neighbors)
        {
            CreatePlane(point);
        }
    }

    private void CreatePlane(Voronoi2DPoint otherPoint)
    {
        Vector2Int a = node.GetCoordinate();
        Vector2Int b = otherPoint.node.GetCoordinate();

        CreateForwardPlane(a, b);
        CreateBackwardsPlanes(a, b);
    }

    private void CreateForwardPlane(Vector2Int a, Vector2 b)
    {
        Vector2 midPointA = (Vector2)(a + b) * 0.5f;

        Vector2 normalA = (Vector2)(a - b); 
        normalA.Normalize(); 
        normalsA.Add(normalA); 
        
        midPointsA.Add(midPointA); 

        Plane planeA = new Plane(normalA, midPointA); 
        nearestPlanesA.Add(planeA); 

        //Debug.Log($"midpoint A" + midPointA);
    }

    private void CreateBackwardsPlanes(Vector2Int a, Vector2 b)
    {
        Vector2 size = grapf.GetSize();

        Vector2 distanceBetween = new Vector2();
        distanceBetween.x =  Mathf.Abs(a.x - b.x);
        distanceBetween.y =  Mathf.Abs(a.y - b.y);

        Vector2 outerDistance = (grapfSize - distanceBetween);

        Vector2Int[] directions = GetDirections(a, b);
        Vector2 virtualPoint = new Vector2();


        for (int i = 0;  i < directions.Length; i++)
        {
            virtualPoint.x = b.x;
            virtualPoint.y = b.y;
            virtualPoint += (directions[i] * size);

            Vector2 midPointB = (a + virtualPoint) * 0.5f;
            midPointsB.Add(midPointB);

            Vector2 normalizedPoint = NormalizePositions(midPointB);
            normalizedPoints.Add(normalizedPoint);

            Vector2 normalB = a - virtualPoint;
            normalB.Normalize();
            normalsB.Add(normalB);

            Plane planeB = new Plane(normalB, normalizedPoint);
            nearestPlanesB.Add(planeB);
        }  
    }

    private Vector2 NormalizePositions(Vector2 midPointB)
    {
        Vector2 normalized = midPointB;

        if (normalized.x >= grapfSize.x)
        {
            normalized.x -= grapfSize.x;
        }
        else if (normalized.x < 0)
        {
            normalized.x += grapfSize.x;
        }

        if (normalized.y >= grapfSize.y)
        {
            normalized.y -= grapfSize.y;
        }
        else if (normalized.y < 0)
        {
            normalized.y += grapfSize.y;
        }

        return normalized;
    }

    private Vector2Int[] GetDirections(Vector2Int a, Vector2 b)
    {
        Vector2Int[] directions = new Vector2Int[3];

        if (a.x == b.x)
        {
            if (a.y < b.y)
            {
                directions[0] = new Vector2Int(-1, -1);
                directions[1] = new Vector2Int(0, -1);
                directions[2] = new Vector2Int(1, -1);
            }
            else
            {
                directions[0] = new Vector2Int(-1, 1);
                directions[1] = new Vector2Int(0, 1);
                directions[2] = new Vector2Int(1, 1);
            }
        }
        else if (a.y == b.y)
        {
            if (a.x < b.x)
            {
                directions[0] = new Vector2Int(-1, -1);
                directions[1] = new Vector2Int(-1, 0);
                directions[2] = new Vector2Int(-1, 1);
            }
            else
            {
                directions[0] = new Vector2Int(1, -1);
                directions[1] = new Vector2Int(1, 0);
                directions[2] = new Vector2Int(1, 1);
            }
        }
        else
        {
            if (a.x < b.x)
            {
                if (a.y < b.y)
                {
                    directions[0] = new Vector2Int(0, -1);
                    directions[1] = new Vector2Int(-1, -1);
                    directions[2] = new Vector2Int(-1, 0);
                }
                else
                {
                    directions[0] = new Vector2Int(-1, 0);
                    directions[1] = new Vector2Int(-1, 1);
                    directions[2] = new Vector2Int(0, 1);
                }
            }
            else //ax mayor que bx
            {
                if (a.y < b.y)
                {
                    directions[0] = new Vector2Int(1, 0);
                    directions[1] = new Vector2Int(1, -1);
                    directions[2] = new Vector2Int(0, -1);
                }
                else
                {
                    directions[0] = new Vector2Int(0, 1);
                    directions[1] = new Vector2Int(1, 1);
                    directions[2] = new Vector2Int(1, 0);
                }
            }
        }

        return directions;
    }

    /// Gizmos
    private Color GetColor()
    {
        int id = colorID;

        while (id > 4)
        {
            id -= 5;
        }

        switch (id)
        {
            case 0:
                {
                    return Color.green;
                }
            case 1:
                {
                    return Color.blue;
                }
            case 2:
                {
                    return Color.cyan;
                }
            case 3:
                {
                    return Color.magenta;
                }
            case 4:
                {
                    return Color.yellow;
                }
            default:
                {
                    return Color.black;
                }
        }
    }

    private void DrawNormals()
    {
        int i = 0;

        foreach (Vector3 midpoint in midPointsA)
        {
            Gizmos.color = GetColor();
            Gizmos.DrawLine(midpoint, (midpoint + nearestPlanesA[i].normal));

            i++;
        }
    }

    private void DrawNormalsB()
    {
        int i = 0;

        if (!drawGizmos)
        {
            i = 0;

            foreach (Vector3 midpoint in midPointsB)
            {
                Gizmos.color = GetColor();
                Gizmos.DrawLine(midpoint, (midpoint + nearestPlanesB[i].normal));
                Gizmos.DrawSphere(midpoint, 0.4f);

                i++;
            }
        }
    }

    private void DrawNormalizedPoints()
    {
        int i = 0;

        if (drawGizmos)
        {
            i = 0;
            foreach (Vector3 normalizedPoint in normalizedPoints)
            {
                Gizmos.color = GetColor();
                Gizmos.DrawLine(normalizedPoint, (normalizedPoint + nearestPlanesB[i].normal));
                Gizmos.DrawSphere(normalizedPoint, 0.4f);

                i++;
            }
        }
    }

    public void DrawGizmos()
    {
        Gizmos.color = GetColor();
        Gizmos.DrawSphere(new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y), 0.6f);

        //DrawNormals();
        DrawNormalsB();
        DrawNormalizedPoints();
    }
}
