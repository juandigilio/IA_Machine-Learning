using System;


public class IdleState_Caravan : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Caravan)
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Caravan caravan = parameters[0] as Caravan;
        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.SetTransitionBehaviour(() =>
        {
            if (caravan.hungryMiners.Count > 0)
            {
                OnFlag?.Invoke(Caravan.Flags.OnHungryPeople);
            }
        });

        return behaviourActions;
    }
}
