using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    [SerializeField] private Herbivorous hervivorousPrefab;
    [SerializeField] private Carnivorous carnivorousPrefab;
    [SerializeField] private Scavenger scavengerPrefab;
    [SerializeField] private Plant plantPrefab;

    private List<Herbivorous> hervivorousAgents = new List<Herbivorous>();
    private List<Carnivorous> carnivorousAgents = new List<Carnivorous>();
    private List<Scavenger> scavengerAgents = new List<Scavenger>();
    private List<Plant> plantAgents = new List<Plant>();

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

        InstantiateScavengers();

        InstantiatePlants();
    }

    private List<Vector2Int> GetRandomAvailableNodes(Vector2IntGrapf grapf, int totalNodes, Vector2Int yRange)
    {
        List<Vector2Int> randomPositions = new List<Vector2Int>();
        List<Vector2Int> freePositions = CheckIfAvailableNodes(grapf, totalNodes, yRange);

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
        foreach(Plant agent in plantAgents)
        {
            Destroy(agent.gameObject);
            plantAgents = new List<Plant>();
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
        Node<Vector2Int> zeroNode = zeroNode = grapf.GetNodeAt(0, 0);
        Node<Vector2Int> limitNode = grapf.GetNodeAt(GameData.width, GameData.height);

        Vector2Int minRange = zeroNode.GetWorldPosition();
        Vector2Int maxRange = limitNode.GetWorldPosition();

        float xPosition;
        float yPosition;

        for (int i = 0; i < GameData.scavengerQnty; i++)
        {
            xPosition = UnityEngine.Random.Range(minRange.x, maxRange.x);
            yPosition = UnityEngine.Random.Range(minRange.y, maxRange.y);

            Vector3 worldPosition = new Vector3(xPosition, yPosition, 0f);

            Scavenger scavenger = Instantiate(scavengerPrefab, worldPosition, Quaternion.identity);
            scavengerAgents.Add(scavenger);
        }
    }

    private void InstantiatePlants()
    {
        int instanceQnty = GameData.hervivorousQnty * 2;

        Vector2Int yLimits = new Vector2Int();
        yLimits.x = 0;
        yLimits.y = GameData.height;

        List<Vector2Int> randomPositions = GetRandomAvailableNodes(grapf, instanceQnty, yLimits);

        foreach (Vector2Int position in randomPositions)
        {
            Node<Vector2Int> current = grapf.GetNodeAt(position);
            current.SetBlocked(true);

            Vector3 worldPosition = new Vector3(current.GetWorldPosition().x, current.GetWorldPosition().y);
            Plant plantAgent = Instantiate(plantPrefab, worldPosition, Quaternion.identity);
            plantAgent.SetNode(current);

            plantAgents.Add(plantAgent);
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

    public List<Plant> GetPlants()
    {
        return plantAgents;
    }
}