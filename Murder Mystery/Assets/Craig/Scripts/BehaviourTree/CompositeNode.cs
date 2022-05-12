using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();

        public override void Reset()
        {
            base.Reset();
            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }
}

