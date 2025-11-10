using System;
using UnityEngine;

public class HungryState : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Miner), // miner
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Miner miner = parameters[0] as Miner;

        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.AddMainTrheadableBehaviour(0, () =>
        {
            if (!miner.IsHunger())
            {
                OnFlag?.Invoke(Miner.Flags.OnMineReached);
            }
        });

        behaviourActions.SetTransitionBehaviour(() =>
        {
            
        });

        return behaviourActions;
    }
}
