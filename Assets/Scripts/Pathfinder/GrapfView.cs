using UnityEngine;

public class GrapfView
{
    private Vector2IntGrapf grapf;
    private Node<Vector2Int> startNode;
    private Node<Vector2Int> destinationNode;

    public void DrawGizmos()
    {
        if (grapf == null)
        {
            //.Log("null grapf");
            return;
        }
            
        foreach (Node<Vector2Int> node in grapf.GetAllNodes())
        {
            if (node.IsBlocked())
            {
                Gizmos.color = Color.black;
            }
            else
            {
                float normalizedCost = (node.GetTerrainCost() - 1) / 9f;

                float r = normalizedCost;
                float g = 1f - normalizedCost;
                float b = 0f;

                Gizmos.color = new Color(r, g, b);
            }

            Gizmos.DrawWireSphere(new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y), 0.1f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(startNode.GetWorldPosition().x, startNode.GetWorldPosition().y), 0.3f);
        Gizmos.DrawSphere(new Vector3(destinationNode.GetWorldPosition().x, destinationNode.GetWorldPosition().y), 0.3f);
    }

    public void SetGraph(Vector2IntGrapf grapf, Node<Vector2Int> startNode, Node<Vector2Int> destinationNode)
    {
        this.grapf = grapf;
        this.startNode = startNode;
        this.destinationNode = destinationNode;

        //Debug.Log("Graph set in GrapfView");
    }
}