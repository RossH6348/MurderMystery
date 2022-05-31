using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    private Task task;
    // Start is called before the first frame update
    void Start()
    {
        task = GetComponentInParent<Task>();
    }

    private void OnTriggerStay(Collider other)
    {
        task.OnTriggerStayChild(other);
    }

    private void OnTriggerExit(Collider other)
    {
        task.OnTriggerExitChild(other);
    }
}
