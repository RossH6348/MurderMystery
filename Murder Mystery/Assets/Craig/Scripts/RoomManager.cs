//Author: Craig Zeki - Zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public enum RoomList : int
    {
        Office = 0,
        Hallway,
        GamesRoom,
        Garden,
        //New rooms above this line
        NumOfRooms
    }

    [SerializeField] private Dictionary<int, Room> charactersLocation = new Dictionary<int, Room>();

    private static RoomManager instance;
    
    

    public static RoomManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<RoomManager>();
            return instance;
        }
    }

    public Vector3 GetRoomPointForPlayer(GameObject player)
    {
        return charactersLocation[player.GetInstanceID()].RoomPoint;
    }

    public void RegisterInRoom(GameObject player, Room room)
    {
        //use direct reference instead of .Add as this will replace the value if exists already. .Add will throw an exception
        charactersLocation[player.GetInstanceID()] = room;
        
    }

    public void RemovePlayer(GameObject player)
    {
        charactersLocation.Remove(player.GetInstanceID());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
