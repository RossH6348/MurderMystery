using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Assassin Class Object", menuName = "Class System/Types/Assassin")]
public class AssassinClass : ClassObject 
{
    public void Awake()
    {
        type = Classtypes.Assassin;
    }
}
