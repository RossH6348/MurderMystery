using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Vector3 roomPoint;

    public Vector3 RoomPoint { get => roomPoint; }

    private void Awake()
    {
        roomPoint = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<CharacterScript>(out CharacterScript characterScript))
        {
            RoomManager.Instance.RegisterInRoom(other.gameObject, this);
        }
        
    }

}
