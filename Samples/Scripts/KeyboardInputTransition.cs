using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyStudios.FSM.Demo
{
    public class KeyboardInputTransition : Transition
    {
        KeyCode keyCode;

        public KeyboardInputTransition(IState stateTo, KeyCode keyCode) : base(stateTo)
        {
            this.keyCode = keyCode;
        }

        public override bool Condition()
        {
            return Input.GetKeyUp(keyCode);
        }
    }
}
