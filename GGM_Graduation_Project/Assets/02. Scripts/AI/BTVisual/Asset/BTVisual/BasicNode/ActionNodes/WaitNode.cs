using UnityEngine;

namespace BTVisual
{
    public class WaitNode : ActionNode
    {
        public float duration = 1f;

        private float _startTime;
        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Time.time - _startTime > duration)
            {
                return State.SUCCESS;
            }
            else
                return State.RUNNING;
        }
    }
}
