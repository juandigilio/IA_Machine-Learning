using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    [SerializeField] private Herbivorous hervivorousPrefab;
    [SerializeField] private Carnivorous carnivorousPrefab;
    [SerializeField] private Scavenger scavengerPrefab;

    private List<Herbivorous> hervivorousAgents = new List<Herbivorous>();
    private List<Carnivorous> carnivorousAgents = new List<Carnivorous>();
    private List<Scavenger> scavengerAgents = new List<Scavenger>();

    public void InstantiateAgents(Vector2IntGrapf grapf, Mine prefab, int totalMines)
    {
        ClearAgents();

        for (int i = 0; i < GameData.carnivorousQnty; i++)
        {

        }

        foreach (Mine mine  in mines)
        {
            GameObject.Destroy(mine.gameObject);
            mines = new List<Mine>();
        }

        List<Vector2Int> randomPositions = GetRandomAvailableNodes(grapf, totalMines);

        foreach (Vector2Int position in randomPositions)
        {
            Node<Vector2Int> current = grapf.GetNodeAt(position);
            current.SetBlocked(true);

            Vector3 worldPosition = new Vector3(current.GetWorldPosition().x, current.GetWorldPosition().y);
            Mine mineInstance = Instantiate(prefab, worldPosition, Quaternion.identity);
            mineInstance.SetNode(current);

            mines.Add(mineInstance);
        }
    }

    private List<Vector2Int> GetRandomAvailableNodes(Vector2IntGrapf grapf, int totalMines)
    {
        List<Vector2Int> randomPositions = new List<Vector2Int>();
        List<Vector2Int> freePositions = CheckIfAvailableNodes(grapf, totalMines);

        while (randomPositions.Count < totalMines)
        {
            int rand = UnityEngine.Random.Range(0, freePositions.Count);

            randomPositions.Add(freePositions[rand]);
            freePositions.RemoveAt(rand);
        }
        return randomPositions;
    }

    private List<Vector2Int> CheckIfAvailableNodes(Vector2IntGrapf grapf, int totalMines)
    {
        List<Vector2Int> freePositions = new List<Vector2Int>();

        foreach (Node<Vector2Int> node in grapf.GetAllNodes())
        {
            if (!node.IsBlocked())
            {
                freePositions.Add(node.GetCoordinate());
            }
        }

        if (freePositions.Count < totalMines)
        {
            Debug.LogError("Mines qnty can't be higher than available nodes!!");
        }

        return freePositions;
    }

    private void ClearAgents()
    {
        foreach (Herbivorous agent in hervivorousAgents)
        {
            Destroy(agent.gameObject);
            hervivorousAgents = new List<Herbivorous>();
        }
        foreach (Carnivorous agent in carnivorousAgents)
        {
            Destroy(agent.gameObject);
            carnivorousAgents = new List<Carnivorous>();
        }
        foreach (Scavenger agent in scavengerAgents)
        {
            Destroy(agent.gameObject);
            scavengerAgents = new List<Scavenger>();
        }
    }

    public List<Herbivorous> GetHerbivorous()
    {
        return hervivorousAgents;
    }

    public List<Carnivorous> GetCarnous()
    {
        return carnivorousAgents;
    }

    public List<Scavenger> GetScavengers()
    {
        return scavengerAgents;
    }
}