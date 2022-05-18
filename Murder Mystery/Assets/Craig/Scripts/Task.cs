using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] bool requiresItem = false;
    

    public bool DoTask()
    {
        return Random.Range((int)0, (int)2) == 0 ? false : true;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
