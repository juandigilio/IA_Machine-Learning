using UnityEngine;

public class Herbivorous : Agent
{
    private Node<Vector2Int> nearestPlant;


    public void SetNearestPlant(Node<Vector2Int> plantNode)
    {
        nearestPlant = plantNode;
    }

    public Node<Vector2Int> GetNearestPlant()
    {
        return nearestPlant;
    }
}
