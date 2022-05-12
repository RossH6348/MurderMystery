using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    
    public class InverterNode : DecoratorNode
    {
        public override void Pause()
        {
            //Do Nothing
        }

        protected override void OnStart()
        {
            //Do nothing
        }

        protected override void OnStop()
        {
            //Do nothing
        }

        protected override NodeState OnUpdate()
        {
            switch (child.Update())
            {
                case NodeState.Ready:
                    return NodeState.Ready;

                case NodeState.Running:
                    return NodeState.Running;

                case NodeState.Failed:
                    return NodeState.Success;

                case NodeState.Success:
                    return NodeState.Failed;

                case NodeState.NumOfStates:
                default:
                    return NodeState.Failed;
            }
        }
    }
}

