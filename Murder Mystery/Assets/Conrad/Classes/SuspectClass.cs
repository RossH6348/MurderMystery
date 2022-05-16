using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Suspect Class Object", menuName = "Class System/Types/Suspect")]
public class SuspectClass : ClassObject
{
    public void Awake()
    {
        type = Classtypes.Suspect;
    }
}
