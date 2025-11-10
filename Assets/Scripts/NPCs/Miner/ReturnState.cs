using UnityEngine;
using System;

public class ReturnState : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Transform), // miner
        typeof(Transform), // baseCamp
        typeof(float),     // speed
        typeof(float),     // deltaTime
        typeof(Miner) // reference
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Transform minerTransform = parameters[0] as Transform;
        Transform baseCampTransform = parameters[1] as Transform;
        float speed = (float)parameters[2];
        float deltaTime = (float)parameters[3];
        Miner miner = parameters[4] as Miner;

        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.AddMainTrheadableBehaviour(0, () =>
        {
            minerTransform.position += (baseCampTransform.position - minerTransform.position).normalized * speed * deltaTime;
        });

        behaviourActions.SetTransitionBehaviour(() =>
        {
            if (Vector3.Distance(minerTransform.position, baseCampTransform.position) < 0.1f)
            {
                OnFlag?.Invoke(Miner.Flags.OnBaseReached);
            }
        });

        return behaviourActions;
    }
}
