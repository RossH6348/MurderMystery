using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the base class, I will be using this to inherit stuff.
public class LaserInput : MonoBehaviour
{

    //This function is called when the player is pointing at the object, thus selecting.
    public virtual void onLaserSelected(CharacterScript character)
    {

    }

    //This function is called when the player no longer is pointing at the object, hint deselecting.
    public virtual void onLaserDeselected(CharacterScript character)
    {

    }

    //This function is called when the player clicks on the laser while selecting this object.
    public virtual void onLaserClick(CharacterScript character)
    {

    }

}
