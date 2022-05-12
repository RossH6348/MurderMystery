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

