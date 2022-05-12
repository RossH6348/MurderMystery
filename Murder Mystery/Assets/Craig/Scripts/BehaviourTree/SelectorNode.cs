using System.Collections;
using System.Collections.Generic;
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


