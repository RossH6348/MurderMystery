using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

static class ExtensionsClass
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject listOfSpawnPoints;
    private List<Transform> spawnPoints = new List<Transform>();
    private int spawnPointIndex = -1;
    private static SpawnManager instance;

    public static SpawnManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<SpawnManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        

        foreach (Transform pointTransform in listOfSpawnPoints.GetComponentsInChildren<Transform>())
        {
            spawnPoints.Add(pointTransform);
        }
        if(spawnPoints.Count > 0)
        {
            spawnPoints.RemoveAt(0);
        }
        spawnPoints.Shuffle();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject Spawn(GameObject characterPrefab)
    {
        if (characterPrefab == null) return null;
        spawnPointIndex++;
        if (spawnPointIndex >= spawnPoints.Count) return null;

        return Instantiate(characterPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);        
    }
}
