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
    public enum NodeState
    {
        Ready = 0,
        Running,
        Failed,
        Success,
        //Add new states above this line
        NumOfStates
    }
    public abstract class BehaviourTreeBaseNode : ScriptableObject
    {
        public NodeState state = NodeState.Ready;
        
        //public string guid;

        //reference to the top of the tree (to get access to the database)
        protected BehaviourTree myTree;

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

        public virtual void SetTree(BehaviourTree tree)
        {
            myTree = tree;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();
        public abstract void Pause();


    }

}

