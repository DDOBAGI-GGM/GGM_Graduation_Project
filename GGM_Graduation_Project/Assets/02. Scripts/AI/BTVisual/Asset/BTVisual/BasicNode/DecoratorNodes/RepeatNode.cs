using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            child.Update();
            return State.RUNNING;
        }
    }
}
