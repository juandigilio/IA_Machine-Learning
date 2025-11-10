using UnityEngine;
using System;

public class DepositState : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Miner)
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Miner miner = parameters[0] as Miner;
        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.AddMainTrheadableBehaviour(0, () =>
        {
            UnityEngine.Debug.Log($"Deposited {miner.GetCarriedGold()} gold at base.");
            miner.EmptyBag();
            OnFlag?.Invoke(Miner.Flags.OnEmptyBag);
        });

        return behaviourActions;
    }
}
