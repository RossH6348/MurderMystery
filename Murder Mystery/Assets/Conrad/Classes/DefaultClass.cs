using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Class Object", menuName ="Class System/Types/Default")]
public class PlayerRoles : ClassObject
{
    public void Awake()
    {
        type = Classtypes.Default;
    }
}
