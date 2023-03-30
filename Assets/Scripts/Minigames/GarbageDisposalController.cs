using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageDisposalController : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        StackItemController stackItem = collision.gameObject.GetComponent<StackItemController>();

        if(stackItem != null)
        {
            stackItem.DestroyItem();
        }
    }
}
