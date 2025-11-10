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

    private Vector2IntGrapf grapf;

    public void SetGrapf(Vector2IntGrapf grapf)
    {
        this.grapf = grapf;
    }

    public void InstantiateAgents()
    {
        ClearAgents();

        Vector2Int yRange = new Vector2Int();

        yRange.x = 0;
        yRange.y = GameData.height / 4;
        InstantiateSpecies(hervivorousPrefab, hervivorousAgents, yRange, GameData.hervivorousQnty);

        
        yRange.x = (int)(GameData.height * 0.75f);
        yRange.y = GameData.height;
        InstantiateSpecies(carnivorousPrefab, carnivorousAgents, yRange, GameData.carnivorousQnty);


    }

    private List<Vector2Int> GetRandomAvailableNodes(Vector2IntGrapf grapf, int totalNodes, Vector2Int yRange)
    {
        List<Vector2Int> randomPositions = new List<Vector2Int>();
        List<Vector2Int> freePositions = CheckIfAvailableNodes(grapf, totalNodes, xLimits, yRange);

        while (randomPositions.Count < totalNodes)
        {
            int rand = UnityEngine.Random.Range(0, freePositions.Count);

            randomPositions.Add(freePositions[rand]);
            freePositions.RemoveAt(rand);
        }

        return randomPositions;
    }

    private List<Vector2Int> CheckIfAvailableNodes(Vector2IntGrapf grapf, int totalNodes, Vector2Int yRange)
    {
        List<Vector2Int> freePositions = new List<Vector2Int>();

        for (int i = 0; i < GameData.width; i++)
        {
            for (int j = yRange.x; j < yRange.y; j++)
            {
                Node<Vector2Int> node = new Node<Vector2Int>();
                node = grapf.GetNodeAt(i, j);

                if (!node.IsBlocked())
                {
                    freePositions.Add(node.GetCoordinate());
                }
            }
        }

        if (freePositions.Count < totalNodes)
        {
            Debug.LogError("Agents qnty can't be higher than available nodes!!");
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

    private void InstantiateSpecies<T>(T agentPrefab, List<T> agentsList, Vector2Int yLimits, int instanceQnty) where T : Agent
    {
        List<Vector2Int> randomPositions = GetRandomAvailableNodes(grapf, instanceQnty, yLimits);

        foreach (Vector2Int position in randomPositions)
        {
            Node<Vector2Int> current = grapf.GetNodeAt(position);
            current.SetBlocked(true);

            Vector3 worldPosition = new Vector3(current.GetWorldPosition().x, current.GetWorldPosition().y);
            T agentInstance = Instantiate(agentPrefab, worldPosition, Quaternion.identity);
            agentInstance.SetNode(current);

            agentsList.Add(agentInstance);
        }
    }

    private void InstantiateScavengers()
    {

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