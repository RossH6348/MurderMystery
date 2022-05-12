//Author: Craig Zeki
//ID: zek21003166

//Based on and modified from: Unity | Create Behaviour Trees using UI Builder, GraphView, and Scriptable Objects [AI #11]
//Reference: https://youtu.be/nKpM98I7PeM
//Ref. Author: TheKiwiCoder
//Ref. Date: 28-May-2021
//Accessed. Date: 12-May-2021

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    
    public class SelectorNode : CompositeNode
    {
        private int currentChild = 0;

        public override void Pause()
        {
           //Do nothing
        }

        protected override void OnStart()
        {
            currentChild = 0;
        }

        protected override void OnStop()
        {
            //Do nothing
        }

        protected override NodeState OnUpdate()
        {
            switch (children[currentChild].Update())
            {
                case NodeState.Ready:
                    return NodeState.Ready;
                    
                case NodeState.Running:
                    return NodeState.Running;
                    
                case NodeState.Failed:
                    currentChild++;
                    break;

                case NodeState.Success:
                    return NodeState.Success;

                case NodeState.NumOfStates:
                default:
                    return NodeState.Failed;
            }

            return currentChild >= children.Count ? NodeState.Failed : NodeState.Running;
        }
    }
}


