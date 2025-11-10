using System;

public interface IAutomaton<TState, TFlag>
    where TState : Enum
    where TFlag : Enum
{
    FSM<TState, TFlag> Fsm { get; }
}
