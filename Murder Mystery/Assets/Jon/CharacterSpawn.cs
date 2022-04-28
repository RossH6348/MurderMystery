using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    public GameObject[] chars;

    public GameObject spawnPoints;

    List<GameObject> characters = new List<GameObject>();

    List<Transform> points = new List<Transform>();

    private int spawnPointIndex = 0;

    //GameObject charSpawn;

    // Start is called before the first frame update
    void Start()    
    {

        //GameObject charSpawn = Instantiate(chars[Random.Range(0, chars.Length)], spawnPoints[0].transform.position, transform.rotation);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Transform getSpawnPoint()
    {
        if (spawnPointIndex >= points.Count) return null;
        return points[spawnPointIndex++];
    }
}