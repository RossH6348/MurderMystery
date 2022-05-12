using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{

    public abstract class DecoratorNode : Node
    {
        public Node child;

        public override void Reset()
        {
            base.Reset();
            child.Reset();
        }
    }

}
