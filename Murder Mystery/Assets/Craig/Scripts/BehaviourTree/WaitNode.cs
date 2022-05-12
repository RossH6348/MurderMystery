using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    public class WaitNode : ActionNode
    {
        public float duration = 0;
        private float timeElapsed = 0;
        public override void Pause()
        {
            //Do nothing
        }

        protected override void OnStart()
        {
            timeElapsed = 0;
        }

        protected override void OnStop()
        {
            //Do nothing
        }

        protected override NodeState OnUpdate()
        {
            timeElapsed += Time.deltaTime;

            if(timeElapsed >= duration)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}
