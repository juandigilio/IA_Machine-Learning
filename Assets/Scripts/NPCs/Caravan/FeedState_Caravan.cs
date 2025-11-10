using System;

public class FeedState_Caravan : State
{
    public override Type[] OnTickParameterTypes => new Type[]
    {
        typeof(Caravan)
    };

    public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
    {
        Caravan caravan = parameters[0] as Caravan;
        BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

        behaviourActions.AddMainTrheadableBehaviour(0, () =>
        {
            caravan.FeedMiners();
        });

        behaviourActions.SetTransitionBehaviour(() =>
        {
            if (caravan.hungryMiners.Count <= 0 && caravan.currentFood > 0)
            {
                OnFlag?.Invoke(Caravan.Flags.OnNoMoreHungryPeople);
            }
            else if (caravan.currentFood <= 0)
            {
                OnFlag?.Invoke(Caravan.Flags.OnEmptyBag);
            }
        });

        return behaviourActions;
    }
}
