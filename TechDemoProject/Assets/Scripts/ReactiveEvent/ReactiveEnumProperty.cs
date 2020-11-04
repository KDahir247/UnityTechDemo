using System;
using Tech.Data;
using UniRx;

namespace Tech.Event.Variable
{
    [Serializable]
    public class ReactiveTouchProperty : ReactiveProperty<ButtonCommand>
    {
        public ReactiveTouchProperty()
        {
        }

        public ReactiveTouchProperty(ButtonCommand command) : base(command)
        {
        }
    }
}