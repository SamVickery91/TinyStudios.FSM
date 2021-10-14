using System;

namespace TinyStudios.FSM
{
    public interface IState
    {
        event Action OnStateExitComplete;
        event Action OnStateEnterComplete;
        void Enter();
        void Exit();
        void Update();
    }
}
