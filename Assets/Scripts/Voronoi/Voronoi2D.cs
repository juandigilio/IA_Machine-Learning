using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voronoi2D : Voronoi<Node<Vector2Int>>
{
    private List<Voronoi2DPoint> voronoiPoints;
    private Vector2IntGrapf grapf;

    public Voronoi2D(List<Node<Vector2Int>> seccionsToCull, Vector2IntGrapf grapf)
    {
        this.grapf = grapf;
        AddVoroniPoints(seccionsToCull);
        OrderByDistance();
    }

    public Voronoi2DPoint GetSeccion(Vector2 characterPosition)
    {
        foreach (Voronoi2DPoint seccion in voronoiPoints)
        {
            bool isInside = true;

            foreach (Plane plane in seccion.nearestPlanesA)
            {
                float distance = plane.GetDistanceToPoint(characterPosition);

                if (distance < 0)
                {
                    isInside = false;
                    break;
                }
            }

            //cirunavegable

            //if (isInside)
            //{
            //    foreach (Plane plane in seccion.nearestPlanesB)
            //    {
            //        float distance = plane.GetDistanceToPoint(characterPosition);

            //        if (distance < 0)
            //        {
            //            isInside = false;
            //            break;
            //        }
            //    }
            //}

            if (isInside)
            {
                return seccion;
            }
        }

        Debug.Log("No nearest point found");
        return null;
    }

    public void DrawGizmos(Transform testingTransform)
    {
        Vector2 gizmoPosition = GetSeccion(new Vector2(testingTransform.position.x, testingTransform.position.y)).node.GetWorldPosition();

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(gizmoPosition.x, gizmoPosition.y), 1f);
    }

    public List<Voronoi2DPoint> GetAllPoints()
    {
        return voronoiPoints;
    }

    protected override void AddVoroniPoints(List<Node<Vector2Int>> seccionsToCull)
    {
        voronoiPoints = new List<Voronoi2DPoint>();

        int color = 0;

        foreach (Node<Vector2Int> point in seccionsToCull)
        {
            voronoiPoints.Add(new Voronoi2DPoint(point, color, grapf));
            color++;
        }
    }

    protected override void OrderByDistance()
    {
        List<Voronoi2DPoint> temporalObjects = new List<Voronoi2DPoint>();
        List<Voronoi2DPoint> orderedObjects = new List<Voronoi2DPoint>();

        for (int i = 0; i < voronoiPoints.Count; i++)
        {
            temporalObjects.Clear();
            orderedObjects.Clear();

            temporalObjects.AddRange(voronoiPoints);
            temporalObjects.RemoveAt(i);

            while (temporalObjects.Count > 0)
            {
                float minDistance = Mathf.Infinity;
                int minIndex = 0;

                for (int j = 0; j < temporalObjects.Count; j++)
                {
                    Vector2 a = voronoiPoints[i].node.GetCoordinate();
                    Vector2 b = temporalObjects[j].node.GetCoordinate();

                    float distance = Vector2.Distance(a, b);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minIndex = j;
                    }
                }

                orderedObjects.Add(temporalObjects[minIndex]);
                temporalObjects.RemoveAt(minIndex);
            }

            voronoiPoints[i].SetPlanes(orderedObjects);
        }
    }
}
