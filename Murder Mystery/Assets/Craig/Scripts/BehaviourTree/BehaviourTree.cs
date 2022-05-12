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
using UnityEditor;

namespace ZekstersLab.BehaviourTree
{
    
    public class BehaviourTree : ScriptableObject
    {
        public Node root;
        public NodeState treeState = NodeState.Ready;
        //public List<Node> nodes = new List<Node>();
        private Dictionary<string, object> contextData = new Dictionary<string, object>();

        public NodeState Update()
        {
            return root.Update();
        }

        public void SetRoot(Node root)
        {
            this.root = root;
            this.root.SetTree(this);
        }

        public void Pause()
        {
            root.Pause();
        }

        public void Reset()
        {
            root.Reset();
        }

        public void SetData(string dataName, object value)
        {
            contextData[dataName] = value;
            
        }

        public object GetData(string dataName)
        {
            object temp = null;
            contextData.TryGetValue(dataName, out temp);
            return temp != null ? temp : null;
        }

        public void EraseData(string dataName)
        {
            contextData.Remove(dataName);
        }

        //public Node CreateNode(System.Type type)
        //{
        //    Node node = ScriptableObject.CreateInstance(type) as Node;
        //    node.name = type.Name;
        //    //node.guid = GUID.Generate().ToString();
        //    nodes.Add(node);

        //    AssetDatabase.AddObjectToAsset(node, this);
        //    AssetDatabase.SaveAssets();
        //    return node;
        //}

        //public void DeleteNote(Node node)
        //{
        //    nodes.Remove(node);
        //    AssetDatabase.RemoveObjectFromAsset(node);
        //    AssetDatabase.SaveAssets();
        //}

    }
}

