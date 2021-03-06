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

    public abstract class DecoratorNode : BehaviourTreeBaseNode
    {
        public BehaviourTreeBaseNode child;

        public override void Reset()
        {
            base.Reset();
            child.Reset();
        }

        public override void SetTree(BehaviourTree tree)
        {
            base.SetTree(tree);
            child.SetTree(tree);
        }
    }

}
