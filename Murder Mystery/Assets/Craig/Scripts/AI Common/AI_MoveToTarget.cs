using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZekstersLab.BehaviourTree;
public class AI_MoveToTarget : ActionNode
{
    private GridSystem gridSystem;
    private Transform myTransform;
    private Vector3 targetPosition;
    private int diceValue;
    private NodeState moveState;
    private float runningTime;
    private Coroutine movingCoroutine;

    public float moveSpeed = 0.5f;
    public float movementTimeout = 0f;

    public override void Pause()
    {
        
    }

    protected override void OnStart()
    {
        moveState = NodeState.Ready;
        diceValue = 0;
        runningTime = 0;
        gridSystem = (GridSystem)myTree.GetData("GridSystem");
        myTransform = (Transform)myTree.GetData("CurrentTransform");
        targetPosition = (Vector3)myTree.GetData("TargetPosition");
        diceValue = (int)myTree.GetData("DiceRoll");
        movingCoroutine = null;
        movementTimeout = moveSpeed * (diceValue + 1); //+1 to allow small buffer for imperfect lerp
    }

    protected override void OnStop()
    {
        Debug.Log("AI Move Test - OnStop() - State: " + state.ToString() + " - moveState: " + moveState.ToString());
        //turn over! - if we haven't made it to our final space we need to stop where we are (closest grid location)
        if(moveState != NodeState.Success)
        {
            Helpers.Instance.StopCoroutine(movingCoroutine);

            //force to move instantly to closest grid point --> not pretty but we are out of time!
            Node gridNode = gridSystem.getNodeAtPos(myTransform.position);
            myTransform.position = gridNode.position;
        }

        myTree.SetData("Position", myTransform.position);
    }

    protected override NodeState OnUpdate()
    {
        switch (moveState)
        {
            case NodeState.Ready:
                if (gridSystem == null) return NodeState.Failed;
                if (myTransform == null) return NodeState.Failed;
                if (targetPosition == null) return NodeState.Failed;
                if (diceValue == 0) return NodeState.Failed;
                List<GameObject> path;

                //plot shortest path to targetPosition
                path = gridSystem.findPath(myTransform.position, targetPosition);
                if (path == null) //no path = already there!
                {
                    moveState = NodeState.Success; //must set this first as it is checked in OnStop()
                    return moveState;
                }
                //shorten path to dice roll
                if (path.Count > diceValue)
                {
                    path.RemoveRange(diceValue, path.Count - diceValue);
                }

                //as this class is not a monobehaviour it cannot directly call StartCoroutine, instead Helpers Singleton provides this service
                //Helpers also provides access to the same movement mechanics as the player
                movingCoroutine = Helpers.Instance.StartCoroutine(Helpers.movePath(path, myTransform, moveSpeed, () => { moveState = NodeState.Success; }));
                moveState = NodeState.Running;
                break;

            case NodeState.Running:
                myTree.SetData("Position", myTransform.position);
                //Debug.Log("MoveToTarget: myTransformPosition: " + myTransform.position.ToString());
                //coroutine is running, here we will do a check to make sure this is not locked up / running over, as well as check if the turn is over
                runningTime += Time.deltaTime;
                if((runningTime >= movementTimeout) || (true == (bool)myTree.GetData("TurnOver"))) moveState = NodeState.Failed;
                break;

            case NodeState.Failed:
            case NodeState.Success:
            case NodeState.NumOfStates:
            default:
                break;
        }

        return moveState;

        
    }
}
