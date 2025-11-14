using System.Collections.Generic;
using UnityEngine;


//Agent (miner)
//Esta en un lugar
//lo mandas a minar
//va a la mina
//junta oro hasta cierta cantidad
//vuelve a donde estaba a dejarlo
//repite hasta que se agote el oro o hasta que lo mandes a otra mina


public class Agent : MonoBehaviour, IAutomaton<Agent.States, Agent.Flags>
{
    public enum States
    {
        Patrol,
        Chase,
        Explode
    }

    public enum Flags
    {
        OnTargetReach,
        OnTargetNear,
        OnTargetLost
    }

    [SerializeField] protected Color color = Color.white;
    [SerializeField] private MeshRenderer mesh;


    public FSM<States, Flags> Fsm { get; private set; }



    //Graph
    private Node<Vector2Int> node;
    private List<Node<Vector2Int>> currentPath;

    private Traveler traveler;


    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = color;
    }
    //public void Start()
    //{
    //    //pasar cada fsm a cada agente en especifico?????
    //    /////////////////////////////////////////////////
    //    ///
    //    //mesh = GetComponent<MeshRenderer>();
    //    mesh.material.color = color;

    //    Fsm = new FSM<States, Flags>(States.Patrol);

    //    Fsm.AddState<PatrolState>(States.Patrol,
    //        onTickParameters: () => new object[] { wayPoint1, wayPoint2, transform, target, speed, chaseDistance, Time.deltaTime }
    //    );

    //    Fsm.AddState<ChaseState>(States.Chase,
    //        onTickParameters: () => new object[] { transform, target, speed, explodeDistance, lostDistance, Time.deltaTime }
    //    );

    //    Fsm.AddState<ExplodeState>(States.Explode);

    //    Fsm.SetTransition(States.Patrol, Flags.OnTargetNear, States.Chase, () => { Debug.Log("Te vi"); });
    //    Fsm.SetTransition(States.Chase, Flags.OnTargetReach, States.Explode);
    //    Fsm.SetTransition(States.Chase, Flags.OnTargetLost, States.Patrol);
    //}

    //private void Update()
    //{
    //    Fsm.Tick();
    //}

    public void Move()
    {
        //StopAllCoroutines();
        StartCoroutine(traveler.Move());
    }

    public void SetTraveler(Traveler traveler)
    {
        this.traveler = traveler;
    }

    public void SetPath(List<Node<Vector2Int>> currentPath)
    {
        this.currentPath = currentPath;
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