using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Miner : MonoBehaviour, IAutomaton<Miner.States, Miner.Flags>
{
    public enum States
    {
        Idle,
        GoToMine,
        Collect,
        Return,
        Deposit,
        Eat
    }

    public enum Flags
    {
        OnMineReached,
        OnBagFull,
        OnBaseReached,
        OnEmptyBag,
        OnEmptyBelly
    }

    public FSM<States, Flags> Fsm { get; private set; }

    public Transform baseLocation;

    public float speed = 2f;
    public int bagCapacity = 15;
    public float collectTimer = 0;
    public float collectRate = 1f;
    public int collectCapacity = 1;
    public int currentBagAmount = 0;
    public float hungerLevel = 0;
    public float maxHunger = 3f;
    public bool isHunger = false;
    public bool isWorking = false;
    public bool isBeingFed = false;

    private Mine mine;

    

    private void Start()
    {
        //mine = MineManager.GetNearestMine(transform.position);

        if (mine == null)
        {
            Debug.LogError("No mines available for the miner.");
        }

        Fsm = new FSM<States, Flags>(States.Idle);

        Fsm.AddState<IdleState>(States.Idle,
            onTickParameters: () => new object[] { this });

        Fsm.AddState<GoToMineState>(States.GoToMine,
            onTickParameters: () => new object[] { this, mine.transform, speed, Time.deltaTime });

        Fsm.AddState<CollectState>(States.Collect,
            onTickParameters: () => new object[] { this, mine, Time.deltaTime });

        Fsm.AddState<ReturnState>(States.Return,
            onTickParameters: () => new object[] { transform, baseLocation, speed, Time.deltaTime, this });

        Fsm.AddState<DepositState>(States.Deposit,
            onTickParameters: () => new object[] { this });

        ///////////
        Fsm.AddState<HungryState>(States.Eat,
            onTickParameters: () => new object[] { this });


        // Transitions
        Fsm.SetTransition(States.Idle, Flags.OnEmptyBag, States.GoToMine);
        Fsm.SetTransition(States.GoToMine, Flags.OnMineReached, States.Collect);
        Fsm.SetTransition(States.Collect, Flags.OnBagFull, States.Return);
        Fsm.SetTransition(States.Collect, Flags.OnEmptyBelly, States.Eat);
        Fsm.SetTransition(States.Return, Flags.OnBaseReached, States.Deposit);
        Fsm.SetTransition(States.Deposit, Flags.OnEmptyBag, States.Idle);
    }

    private void Update()
    {
        Fsm.Tick();
    }

    public int GetCarriedGold()
    {
        return currentBagAmount;
    }

    public void CollectGold(Mine mine)
    {
        if (currentBagAmount < bagCapacity)
        {
            int extractedGold = mine.ExtractGold(collectCapacity);
            currentBagAmount += extractedGold;
            hungerLevel += extractedGold;

            if (hungerLevel >= maxHunger)
            {
                isHunger = true;
            }
        }
    }

    public bool IsBagFull()
    {
        return currentBagAmount >= bagCapacity;
    }

    public void EmptyBag()
    {
        currentBagAmount = 0;
    }

    public void SetTargetMine(Mine targetMine)
    {
        mine = targetMine;
    }

    public bool IsHunger()
    {
        return isHunger;
    }

    public void StartBeingFed()
    {
        isBeingFed = true;
    }

    public bool IsBeingFed()
    {
        return isBeingFed;
    }

    public void Feed()
    {
        hungerLevel = 0;
        isHunger = false;
        isBeingFed = false;
    }
}
