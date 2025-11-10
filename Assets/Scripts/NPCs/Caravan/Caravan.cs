using System;
using System.Collections.Generic;
using UnityEngine;

public class Caravan : MonoBehaviour, IAutomaton<Caravan.States, Caravan.Flags>
{
    public enum States
    {
        Idle,
        GoToMine,
        Feed,
        Return,
        Refill
    }

    public enum Flags
    {
        OnHungryPeople,
        OnNoMoreHungryPeople,
        OnMineReached,
        OnEmptyBag,
        OnBaseReached
    }

    public FSM<States, Flags> Fsm { get; private set; }

    public Transform baseLocation;
    public Mine mine;
    public float speed = 2f;
    public int currentFood;
    public int maxFood = 10;

    public List<Miner> hungryMiners;

    private void Start()
    {
        transform.position = baseLocation.position;
        hungryMiners = new List<Miner>();
        currentFood = maxFood;

        Fsm = new FSM<States, Flags>(States.Idle);

        Fsm.AddState<IdleState_Caravan>(States.Idle,
            onTickParameters: () => new object[] { this });

        Fsm.AddState<GoToMineState>(States.GoToMine,
            onTickParameters: () => new object[] { transform, mine.transform, speed, Time.deltaTime });

        Fsm.AddState<CollectState>(States.Feed,
            onTickParameters: () => new object[] { this });

        Fsm.AddState<ReturnState>(States.Return,
            onTickParameters: () => new object[] { transform, baseLocation, speed, Time.deltaTime, this });

        Fsm.AddState<DepositState>(States.Refill,
            onTickParameters: () => new object[] { this });


        Fsm.SetTransition(States.Idle, Flags.OnHungryPeople, States.GoToMine);
        Fsm.SetTransition(States.GoToMine, Flags.OnMineReached, States.Feed);

        Fsm.SetTransition(States.Feed, Flags.OnNoMoreHungryPeople, States.GoToMine);
        Fsm.SetTransition(States.Feed, Flags.OnEmptyBag, States.Return);

        //Fsm.SetTransition(States.Collect, Flags.OnEmptyBelly, States.Eat);
        //Fsm.SetTransition(States.Return, Flags.OnBaseReached, States.Deposit);
        //Fsm.SetTransition(States.Deposit, Flags.OnEmptyBag, States.Idle);
    }

    private void Update()
    {
        Fsm.Tick();
    }

    public void FeedMiners()
    {
        foreach (Miner miner in hungryMiners)
        {
            if (miner.isHunger && currentFood > 0)
            {
                if (currentFood <= 0)
                {
                    Debug.Log("Caravan has run out of food.");
                    break;
                }

                miner.Feed();
                currentFood--;      
            }
        }
    }

    public void AddHungryMiner(Miner miner)
    {
        if (!hungryMiners.Contains(miner))
        {
            hungryMiners.Add(miner);
            miner.StartBeingFed();
        }
        else
        {
            Debug.LogWarning("Miner is already in the caravan hungry list.");
        }
    }

    public void RemoveHungryMiner(Miner miner)
    {
        if (hungryMiners.Contains(miner))
        {
            hungryMiners.Remove(miner);
        }
    }

    public void SetCurrentMine(Mine mine)
    {
        this.mine = mine;
    }
}
