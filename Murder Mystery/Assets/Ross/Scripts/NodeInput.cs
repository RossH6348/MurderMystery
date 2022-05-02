using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInput : LaserInput
{

    private List<GameObject> currentPath = null;
    public override void onLaserSelected(CharacterScript character)
    {
        currentPath = character.gridSystem.findPath(character.transform.position, transform.position);
        if(currentPath != null)
            for (int i = currentPath.Count - 1; i > -1; i--)
                currentPath[i].GetComponent<MeshRenderer>().enabled = true;
    }

    public override void onLaserDeselected(CharacterScript character)
    {
        if (currentPath != null)
            for (int i = currentPath.Count - 1; i > -1; i--)
                currentPath[i].GetComponent<MeshRenderer>().enabled = false;
    }

    public override void onLaserClick(CharacterScript character)
    {
        //Okay, they clicked this node so let make them start moving to this node!
        character.MoveTo(transform.position);
    }

}
