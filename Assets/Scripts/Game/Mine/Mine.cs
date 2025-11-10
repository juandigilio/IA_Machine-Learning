using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private List<Miner> assignedMiners;

    private Node<Vector2Int> node;
    public int totalGold = 1000;
    public int maxAssignedMiners = 10;

    private void Start()
    {
        assignedMiners = new List<Miner>();
    }

    public void AssignMiner(Miner miner)
    {
        if (!assignedMiners.Contains(miner))
        {
            if (assignedMiners.Count >= maxAssignedMiners)
            {
                Debug.LogWarning("Cannot assign more miners to this mine. Maximum capacity reached.");
                return;
            }

            assignedMiners.Add(miner);
        }
        else
        {
            Debug.LogWarning("Miner is already assigned to this mine.");
        }
    }

    public void RemoveMiner(Miner miner)
    {
        if (assignedMiners.Contains(miner))
        {
            assignedMiners.Remove(miner);
        }
        else
        {
            Debug.LogWarning("Miner is not assigned to this mine.");
        }
    }

    public int ExtractGold(int amount)
    {
        if (totalGold >= amount)
        {
            totalGold -= amount;
        }
        else
        {
            amount = totalGold;
            totalGold = 0;
        }

        return amount;
    }

    public int GetAvailableGold()
    {
        return totalGold;
    }

    public void SetNode(Node<Vector2Int> node)
    {
        this.node = node;
        node.SetBlocked(true);
        transform.position = new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y);
    }

    public Node<Vector2Int> GetNode()
    {
        return node;
    }
}
