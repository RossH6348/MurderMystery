using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInput : LaserInput
{

    private List<GameObject> currentPath = null;
    [SerializeField] private GameObject pathNumber;

    public override void onLaserSelected(CharacterScript character)
    {

        currentPath = character.gridSystem.findPath(character.transform.position, transform.position);

        if (currentPath != null)
        {

            Color nodeColor = (character.maxRoll >= currentPath.Count ? new Color(1.0f, 1.0f, 1.0f) : new Color(1.0f, 0.0f, 0.0f));

            for (int i = currentPath.Count - 1; i > -1; i--)
            {
                currentPath[i].GetComponent<MeshRenderer>().enabled = true;
                currentPath[i].GetComponent<Renderer>().material.color = nodeColor;
            }

            pathNumber.SetActive(true);
            pathNumber.transform.position = transform.position + Vector3.up;
            pathNumber.transform.LookAt(character.transform.position + (Vector3.up * 0.5f));

            pathNumber.GetComponentInChildren<Text>().text = currentPath.Count.ToString();

        }
    }

    public override void onLaserDeselected(CharacterScript character)
    {
        if (currentPath != null)
        {
            for (int i = currentPath.Count - 1; i > -1; i--)
                currentPath[i].GetComponent<MeshRenderer>().enabled = false;
            pathNumber.SetActive(false);
        }
    }

    public override void onLaserClick(CharacterScript character, bool clickState)
    {
        //Okay, they clicked this node so let make them start moving to this node!
        if(clickState)
            character.MoveTo(transform.position);
    }

}
