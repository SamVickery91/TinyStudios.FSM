using System;
using UnityEngine;

namespace TinyStudios.FSM.Demo
{
    public partial class FSMDemo
    {
        class DemoState : IState, IStateUpdate, IStateFixedUpdate
        {
            public event Action OnStateExitComplete;
            public event Action OnStateEnterComplete;

            string stateName;

            public DemoState(string name)
            {
                stateName = name;
            }

            public void Enter()
            {
                Debug.Log($"State{stateName}: Enter");
                OnStateEnterComplete?.Invoke();
            }

            public void Exit()
            {
                Debug.Log($"State{stateName}: Exit");
                OnStateExitComplete?.Invoke();
            }

            public void Update()
            {
                //Debug.Log($"State{stateName}: Update");
            }

            public void FixedUpdate()
            {
                //Debug.Log($"State{stateName}: FixedUpdate");
            }
        }
    }
}
