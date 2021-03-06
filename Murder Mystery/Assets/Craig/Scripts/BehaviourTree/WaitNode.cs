//Author: Craig Zeki
//ID: zek21003166

//Based on and modified from: Unity | Create Behaviour Trees using UI Builder, GraphView, and Scriptable Objects [AI #11]
//Reference: https://youtu.be/nKpM98I7PeM
//Ref. Author: TheKiwiCoder
//Ref. Date: 28-May-2021
//Accessed. Date: 12-May-2021

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
