using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum Classtypes
    {
        Default,
        Suspect,
        Assassin,
        Target,
    }

    public abstract class ClassObject:ScriptableObject
    {
        public GameObject icon;
        public Classtypes type;
        [TextArea(15, 20)]
        public string description;
    }

