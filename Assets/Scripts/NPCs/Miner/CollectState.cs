using System;
using UnityEngine;

public class CollectState : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Miner), // miner
        typeof(Mine), // mine
        typeof(float)  // deltaTime
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Miner miner = parameters[0] as Miner;
        Mine mine = parameters[1] as Mine;
        float deltaTime = (float)parameters[2];

        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.AddMainTrheadableBehaviour(0, () =>
        {
            miner.collectTimer += deltaTime;

            if (miner.collectTimer >= miner.collectRate)
            {
                miner.collectTimer = 0f;
                miner.CollectGold(mine);
                UnityEngine.Debug.Log($"Miner collected gold. Carrying {miner.GetCarriedGold()}");

                if (miner.IsBagFull())
                {
                    OnFlag?.Invoke(Miner.Flags.OnBagFull);
                }
                else if (miner.IsHunger())
                {
                    OnFlag?.Invoke(Miner.Flags.OnEmptyBelly);
                    Debug.Log($"Miner hungry. Carrying {miner.GetCarriedGold()}");
                }
            }
        });

        return behaviourActions;
    }
}
