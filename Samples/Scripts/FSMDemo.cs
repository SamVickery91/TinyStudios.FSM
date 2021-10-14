using UnityEngine;

namespace TinyStudios.FSM.Demo
{
    public partial class FSMDemo : MonoBehaviour
    {
        DemoState state1 = new DemoState("State 1");
        DemoState state2 = new DemoState("State 2");
        DemoState state3 = new DemoState("State 3");

        StateMachine stateMachine = new StateMachine();

        // Start is called before the first frame update
        void Start()
        {
            stateMachine.AddTransition(state1, new Transition(state2, ExampleTransitionCondition));
            stateMachine.AddTransition(state2, new Transition(state1, ExampleTransitionCondition));

            stateMachine.AddTransition(new KeyboardInputTransition(state1, KeyCode.Alpha1));
            stateMachine.AddTransition(new KeyboardInputTransition(state2, KeyCode.Alpha2));
            stateMachine.AddTransition(new KeyboardInputTransition(state3, KeyCode.Alpha3));

            stateMachine.GoToState(state1);
        }

        private bool ExampleTransitionCondition()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
    }
}
