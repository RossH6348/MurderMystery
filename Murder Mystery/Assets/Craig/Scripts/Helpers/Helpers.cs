//Authors: Craig Zeki - Zek21003166
//Authors: Ross Hutchins

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalResources
{

    public static List<AudioClip> footsteps = new List<AudioClip>()
    {
        Resources.Load<AudioClip>("Sounds/Footsteps/Footstep1"),
        Resources.Load<AudioClip>("Sounds/Footsteps/Footstep2"),
        Resources.Load<AudioClip>("Sounds/Footsteps/Footstep3"),
        Resources.Load<AudioClip>("Sounds/Footsteps/Footstep4"),
        Resources.Load<AudioClip>("Sounds/Footsteps/Footstep5")
    };

}

public class Helpers : MonoBehaviour
{
    private static Helpers instance;

    public static Helpers Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Helpers>();
            return instance;
        }
    }

    //allows AI to wait for the item to be shown
    public IEnumerator showPresentedItem(float showDuration, Action callback)
    {
        Debug.Log("ShowPresentedItem - Start of " + showDuration.ToString() + "s delay");
        yield return new WaitForSeconds(showDuration);
        Debug.Log("ShowPresentedItem - showDuration expired, pre Callback");
        callback();
        Debug.Log("ShowPresentedItem - post Callback");
    }

    //This will start a coroutine loop of moving an object through a path.
    public static IEnumerator movePath(List<GameObject> path, Transform transform, float time, Action callback)
    {

        //If we find an audio source attached to VR player or AI player or whatever, we can play some footstep noises.
        AudioSource source = transform.GetComponent<AudioSource>();

        //status = turnStatus.Action; //Set this player to action status.
        while (path.Count > 0)
        {
            GameObject node = path[0];

            if (source != null && GlobalResources.footsteps.Count - 1 > 0)
            {
                //Debug.Log("PLAY FOOTSTEP!");
                source.PlayOneShot(GlobalResources.footsteps[UnityEngine.Random.Range(0, GlobalResources.footsteps.Count - 1)]);
            }

            Vector3 currentPos = transform.position;
            float t = 0.0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime / time;
                transform.position = Vector3.Lerp(currentPos, node.transform.position, t);
                yield return new WaitForEndOfFrame();
            }

            path.RemoveAt(0);
        }

        source.Stop();
        callback();
       //status = turnStatus.Play; //Set this player back to play status.
    }
}
