using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class Task : MonoBehaviour
{
    [SerializeField] private bool requiresItem = false;
    [SerializeField] private ItemObject item;
    [SerializeField] private Transform taskItemPoint = null;
    [SerializeField] private Vector3 posOffset = Vector3.zero;
    [SerializeField] private Vector3 itemScale = Vector3.one;
    [SerializeField] private Vector3 rotationSpeed = Vector3.one;
    [SerializeField] private Material itemMaterial = null;
    [SerializeField] private float minTimeItemPresent = 0.25f;
    [SerializeField] private ParticleSystem myPS = null;

    private GameObject myItem = null;
    private bool coroutineRunning = false;
    private float timeInContact = 0f;

    private float taskCompletionDelay = 1.0f; //the estimated time it takes to spawn an item once task is initiated - used by AI
    private GameObject taskReward = null; //the item which was spawned - used by the AI

    public float TaskCompletionDelay { get => taskCompletionDelay; }
    public GameObject TaskReward { get => taskReward; }
    public ItemObject Item { get => item; }
    public Transform TaskItemPoint { get => taskItemPoint;  }
    public bool RequiresItem { get => requiresItem; }
    public Vector3 PosOffset { get => posOffset; }

    private void Awake()
    {
        if(requiresItem && (item != null))
        {
            myItem = Instantiate(item.prefab3d, taskItemPoint);
            myItem.transform.position += posOffset;
        }

        //set properties used by AI
        taskReward = null;
        if (myPS != null) taskCompletionDelay = myPS.main.duration + 2f; //long enough so VR player can see
    }

    private void Start()
    {
        if (requiresItem && (myItem != null))
        {
            
            myItem.transform.localScale = Vector3.Scale(myItem.transform.localScale, itemScale);
            //myItem.transform.lossyScale.Scale(itemScale);

            //if (myItem.TryGetComponent<Interactable>(out Interactable interactable)) interactable.enabled = false;
            //if (myItem.TryGetComponent<Throwable>(out Throwable throwable)) throwable.enabled = false;
            if (myItem != null) myItem.AddComponent<IgnoreHovering>(); //disables ability to interact and pickup - using .enabled as above does not work
            foreach (Collider col in myItem.GetComponentsInChildren<Collider>()) col.enabled = false;
            if (myItem.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) rigidbody.isKinematic = true;

            if (itemMaterial != null)
            {

                foreach(Renderer rend in myItem.GetComponentsInChildren<Renderer>())
                {
                    for (int i = 0; i < rend.materials.Length; i++)
                    {
                        //Copy the transparent blue material onto the item
                        rend.materials[i].CopyPropertiesFromMaterial(itemMaterial);
                        //renderer.materials[i] = itemMaterial; - according to the documentation this should work... but doesn't
                    }
                }
                    
                
                    
                
            }


        }
    }

    private void Update()
    {
        if (myItem == null) return;

        myItem.transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    IEnumerator DoTaskComplete(Vector3 particleSystemPos)
    {
        taskReward = null;

        if (myPS != null)
        {
            myPS.gameObject.transform.position = particleSystemPos;
            myPS.Play();
        }
        //TODO trigger hapitic pulse

        //wait for particle system to finish
        yield return new WaitForSeconds(myPS.main.duration);

        //drawn new item from deck and spawn
        ItemObject newItem = ItemDeck.Instance.DrawFromDeck();
        if(newItem != null)
        {
            taskReward = Instantiate(newItem.prefab3d, taskItemPoint.position + posOffset, transform.rotation);
            //TODO play positive sound
        }
        else
        {
            
            //play negative sound
        }
        
        coroutineRunning = false;
    }

    private bool DoTask()
    {
        if (coroutineRunning) return false;

        if (!requiresItem)
        {
            coroutineRunning = true;
            StartCoroutine(DoTaskComplete(taskItemPoint.transform.position + posOffset));

            return true;
        }

        return false;
    }

    public bool DoTask(ItemObject itemPresented, GameObject itemGameObject)
    {
        if (coroutineRunning) return false;

        if((itemPresented == null) && !requiresItem)
        {
            coroutineRunning = true;
            StartCoroutine(DoTaskComplete(taskItemPoint.transform.position + posOffset));
            
            return true;
        }
        else if(requiresItem &&(itemPresented.name == item.name))
        {
            ItemDeck.Instance.ReturnToDeck(itemPresented);
            
            coroutineRunning = true;
            StartCoroutine(DoTaskComplete(itemGameObject.transform.position));
            Destroy(itemGameObject);
            return true;
        }

        return false;
        //return Random.Range((int)0, (int)2) == 0 ? false : true; //used for testing the AI
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        
        
    }

    public void OnTriggerStayChild(Collider other)
    {
        
        if (!other.gameObject.TryGetComponent<ItemFoundation>(out ItemFoundation itemFoundation)) return;

        if (other.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
        {
            //object is interactible
            //test to see if it is still in hand
            if (!interactable.attachedToHand)
            {
                
                if(timeInContact >= minTimeItemPresent)
                {
                    //not attached to the hand anymore - player has dropped it
                    DoTask(itemFoundation.item, other.gameObject);
                }
                else
                {
                    timeInContact = 0;
                }
                
            }
            else
            {
                timeInContact += Time.deltaTime;
                if (timeInContact >= minTimeItemPresent)
                {
                    //Check if item presented is correct and then
                    //TODO change material maybe
                    //TODO trigger haptic
                    //TODO trigger sound
                    //TODO display hint to drop
                    //ready to drop
                }

            }
        }
    }

    public void OnTriggerExitChild(Collider other)
    {
        //timeInContact = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<VRPlayerScript>(out VRPlayerScript vRPlayerScript) && !requiresItem)
        {
            DoTask();
        }
    }

}
