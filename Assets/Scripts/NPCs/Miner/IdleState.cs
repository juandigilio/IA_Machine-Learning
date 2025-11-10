using System;
using UnityEngine;

public class IdleState : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Miner)
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Miner miner = parameters[0] as Miner;
        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.SetTransitionBehaviour(() =>
        {
            if (miner.GetCarriedGold() == 0)
            {
                //miner.SetTargetMine(MineManager.GetNearestMine(miner.transform.position));
                OnFlag?.Invoke(Miner.Flags.OnEmptyBag);
            }
        });

        return behaviourActions;
    }
}
