using System;

namespace TinyStudios.FSM
{
    public class Transition
    {
        public IState StateTo => stateTo;

        private IState stateTo;
        private Func<bool> condition;

        public Transition(IState stateTo, Func<bool> condition)
        {
            this.stateTo = stateTo;
            this.condition = condition;
        }

        public Transition(IState stateTo)
        {
            this.stateTo = stateTo;
        }

        public virtual bool Condition()
        {
            return condition();
        }
    }
}
