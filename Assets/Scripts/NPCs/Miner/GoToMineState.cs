using System;
using UnityEngine;

public class GoToMineState : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Miner), // miner
        typeof(Transform), // mine
        typeof(float),     // speed
        typeof(float),     // deltaTime
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Miner miner = parameters[0] as Miner;
        Transform mineTransform = parameters[1] as Transform;
        float speed = (float)parameters[2];
        float deltaTime = (float)parameters[3];

        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.AddMainTrheadableBehaviour(0, () =>
        {
            miner.transform.position += (mineTransform.position - miner.transform.position).normalized * speed * deltaTime;
        });

        behaviourActions.SetTransitionBehaviour(() =>
        {
            if (Vector3.Distance(miner.transform.position, mineTransform.position) < 0.1f)
            {

                OnFlag?.Invoke(Miner.Flags.OnMineReached);
            }
        });

        return behaviourActions;
    }
}
