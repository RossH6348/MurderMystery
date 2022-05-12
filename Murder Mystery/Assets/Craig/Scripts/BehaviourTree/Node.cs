using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    public enum NodeState
    {
        Ready = 0,
        Running,
        Failed,
        Success,
        //Add new states above this line
        NumOfStates
    }
    public abstract class Node : ScriptableObject
    {
        public NodeState state = NodeState.Ready;
        private Dictionary<string, object> contextData = new Dictionary<string, object>();

        public NodeState Update()
        {
            switch (state)
            {
                case NodeState.Ready:
                    OnStart();
                    state = NodeState.Running;
                    break;
                case NodeState.Running:
                    state = OnUpdate();
                    
                    //exit transition
                    if(state != NodeState.Running)
                    {
                        OnStop();
                    }
                    break;
                case NodeState.Failed:
                case NodeState.Success:
                    //Do nothing, await reset
                    break;
                case NodeState.NumOfStates:
                default:
                    break;
            }

            return state;
        }

        public virtual void Reset()
        {
            if(state != NodeState.Running)
            {
                state = NodeState.Ready;
            }
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();
        public abstract void Pause();


    }

}

