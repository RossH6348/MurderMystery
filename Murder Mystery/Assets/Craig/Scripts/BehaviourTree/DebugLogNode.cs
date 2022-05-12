using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        public override void Pause()
        {
            Debug.Log($"Pause: {message}");
        }

        protected override void OnStart()
        {
            Debug.Log($"OnStart: {message}" + state.ToString());
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop: {message}" + state.ToString());
        }

        protected override NodeState OnUpdate()
        {
            Debug.Log($"OnUpdate: {message} " + state.ToString());
            return NodeState.Success;
        }
    }
}

