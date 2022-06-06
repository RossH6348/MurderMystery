using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static IEnumerator showPresentedItem(float showDuration, Action callback)
    {
        yield return new WaitForSeconds(showDuration);
        callback();
    }

    //This will start a coroutine loop of moving an object through a path.
    public static IEnumerator movePath(List<GameObject> path, Transform transform, float time, Action callback)
    {
        //status = turnStatus.Action; //Set this player to action status.
        while (path.Count > 0)
        {
            GameObject node = path[0];

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
        callback();
       //status = turnStatus.Play; //Set this player back to play status.
    }
}
