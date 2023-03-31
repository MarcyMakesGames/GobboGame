using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageDisposalController : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        FoodItemController stackItem = collision.gameObject.GetComponent<FoodItemController>();

        if(stackItem != null)
        {
            stackItem.DestroyItem();
        }
    }
}
