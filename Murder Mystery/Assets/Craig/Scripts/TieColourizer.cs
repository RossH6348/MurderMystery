//Author: Craig Zeki - Zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieColourizer : MonoBehaviour
{
    [SerializeField] private Renderer knot;
    [SerializeField] private Renderer tail;
    [SerializeField] private Renderer head;

    private void Awake()
    {
        if (knot == null) return;
        if (tail == null) return;
        if (head == null) return;

        Material myMat = head.material;

        knot.material = myMat;
        tail.material = myMat;

    }
  
}
