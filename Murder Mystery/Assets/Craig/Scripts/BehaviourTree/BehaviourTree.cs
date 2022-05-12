using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZekstersLab.BehaviourTree
{
    public class BehaviourTree : ScriptableObject
    {
        public Node root;
        public NodeState treeState = NodeState.Ready;

        public NodeState Update()
        {
            return root.Update();
        }

        public void Pause()
        {
            root.Pause();
        }

        public void Reset()
        {
            root.Reset();
        }
    }
}

