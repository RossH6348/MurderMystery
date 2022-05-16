using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Target Class Object", menuName = "Class System/Types/Target")]
public class TargetClass : ClassObject
{
    public void Awake()
    {
        type = Classtypes.Target;
    }
}
