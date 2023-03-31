using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStackController
{
    [SerializeField] private GameObject pawnObject;

    private Stack<GameObject> stack = new Stack<GameObject>();

    private Vector3 stackOffset;

    public int StackCount { get => stack.Count; }

    public void AddObjectToStack(GameObject stackObject)
    {

        if (stack.Count == 0)
        {
            stackObject.transform.SetParent(pawnObject.transform);

            Vector3 stackOffsetPosition = pawnObject.transform.position + stackOffset;
            stackObject.transform.position = stackOffsetPosition;
            stack.Push(stackObject);
        }
        else
        {
            stackObject.transform.SetParent(stack.Peek().transform);

            Vector3 stackOffsetPosition = stack.Peek().transform.position + stackOffset;
            stackObject.transform.position = stackOffsetPosition;
            stack.Push(stackObject);
        }
    }

    public void StartNewGame(Vector3 offset)
    {
        pawnObject = PawnManager.instance.PawnObject;
        stack = new Stack<GameObject>();
        stackOffset = offset;
    }

    public void EatStack()
    {
        int stackCount = stack.Count;

        for (int i = 0; i < stackCount; i++)
        {
            GameObject stackItem = stack.Pop();
            stackItem.GetComponent<FoodItemController>().EatItem();
        }
    }

    public void DropStack()
    {
        int stackCount = stack.Count;

        for(int i = 0; i < stackCount; i++)
        {
            GameObject stackItem = stack.Pop();
            stackItem.GetComponent<FoodItemController>().DropItem();
        }
    }
}
