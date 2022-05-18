using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestingPointGizmo : MonoBehaviour
{
    [SerializeField] Color colour = Color.cyan;
    private void OnDrawGizmos()
    {
        Gizmos.color = colour;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
