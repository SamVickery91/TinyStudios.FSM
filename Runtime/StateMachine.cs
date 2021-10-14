using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyStudios.FSM
{
    public class StateMachine
    {
        public delegate void GotoNewStateEvent(IState from, IState to);
        public event GotoNewStateEvent OnGoToNewState;

        public IState currentState { get; private set; }
        public IState targetState { get; private set; }

        public List<IState> history { get; private set; }

        public IState previousState
        {
            get
            {
                return GetPreviousState(1);
            }
        }

        public IState GetPreviousState(int stepsBack)
        {
            if (history.Count > stepsBack)
                return history[history.Count - stepsBack - 1];
            else
                return null;
        }

        List<Transition> anyTransitions = new List<Transition>();
        Dictionary<IState, List<Transition>> stateTransitions = new Dictionary<IState, List<Transition>>();

        public StateMachine()
        {
            history = new List<IState>();
        }

        public void Update()
        {
            if (currentState != null)
            {
                (currentState as IStateUpdate)?.Update();

                foreach (var item in anyTransitions)
                {
                    if (item.Condition())
                    {
                        GoToState(item.StateTo);
                        return;
                    }
                }

                if (stateTransitions.ContainsKey(currentState))
                {
                    foreach (var item in stateTransitions[currentState])
                    {
                        if (item.Condition())
                        {
                            GoToState(item.StateTo);
                            return;
                        }
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            if (currentState == null) return;

            (currentState as IStateFixedUpdate)?.FixedUpdate();
        }

        public void GoToState(IState state)
        {
            if (currentState == state) return;

            targetState = state;

            //Debug.Log($"Goto: {state.GetType()}");

            // Cache the state 
            if (currentState != null)
            {
                OnGoToNewState?.Invoke(currentState, state);
                currentState.OnStateExitComplete -= GoToTargetState;
                currentState.OnStateExitComplete += GoToTargetState;
                currentState.Exit();
            }
            else
            {
                OnGoToNewState?.Invoke(null, state);
                GoToTargetState();
            }
        }

        public void ForceState(IState state)
        {
            currentState = state;
            OnGoToNewState?.Invoke(null, state);
            history.Add(currentState);
        }

        private void GoToTargetState()
        {
            if (currentState != null)
                currentState.OnStateExitComplete -= GoToTargetState;

            currentState = targetState;

            history.Add(currentState);

            if (currentState != null)
                currentState.Enter();
        }

        public void CleanUp()
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = null;
        }

        public void ClearHistory()
        {
            history.Clear();
        }

        public void LogHistory()
        {
            if (history.Count == 0) return;

            string s = history.Count + ": " + history[0].ToString();
            for (int i = 1; i < history.Count; i++)
            {
                s += " > " + history[i];
            }

            Debug.Log(s);
        }

        public void GoBackToState(IState state)
        {
            RemoveHistoryToState(state);
            GoToState(state);
        }

        public void RemoveHistoryToState(IState state)
        {
            for (int i = history.Count - 1; i >= 0; i--)
            {
                if (history[i] == state)
                {
                    history.RemoveRange(i, history.Count - i);
                }
            }
        }

        public bool GoBack(int steps)
        {
            if (history.Count > steps)
            {
                IState targetState = history[history.Count - steps - 1];

                history.RemoveAt(history.Count - 1);
                for (int i = 0; i < steps; i++)
                {
                    history.RemoveAt(history.Count - 1);
                }

                GoToState(targetState);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GoBack()
        {
            return GoBack(1);
        }

        public void AddTransition(Transition transition)
        {
            anyTransitions.Add(transition);
        }

        public void AddTransition(IState fromState, Transition transition)
        {
            if (!stateTransitions.ContainsKey(fromState))
                stateTransitions.Add(fromState, new List<Transition>() { transition });
            else
                stateTransitions[fromState].Add(transition);
        }

        public void RemoveTransition(Transition transition)
        {
            anyTransitions.Remove(transition);
        }

        public void RemoveTransition(IState fromState, Transition transition)
        {
            if (stateTransitions.ContainsKey(fromState))
                stateTransitions[fromState].Remove(transition);
        }
    }
}