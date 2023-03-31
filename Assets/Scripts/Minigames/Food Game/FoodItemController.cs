using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FoodType foodType;

    public FoodType FoodType { get { return foodType; } }

    private Rigidbody2D rb;
    private FoodDropGameManager foodDropManager;
    private Transform pawn;

    private bool hasBeenCaught = false;
    private bool isEating = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PawnMoveController pawnMoveController = collision.gameObject.GetComponent<PawnMoveController>();

        if (!hasBeenCaught)
        {
            if (pawnMoveController != null)
            {
                hasBeenCaught = true;
                rb.simulated = false;
                foodDropManager.AddFoodToStack(gameObject);
                return;
            }
        }
    }

    public void DropItem()
    {
        rb.simulated = true;
        transform.SetParent(null);
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 100f);
    }

    public void DestroyItem()
    {
        foodDropManager.RemoveFromTotalFood(this.gameObject);
        Destroy(gameObject);
    }

    public void EatItem()
    {
        isEating = true;
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        foodDropManager = FindObjectOfType<FoodDropGameManager>();
        pawn = PawnManager.instance.PawnObject.transform;
    }

    private void Update()
    {
        if (!isEating)
            return;

        transform.position = Vector3.MoveTowards(transform.position, pawn.transform.position, moveSpeed * Time.deltaTime);

        if (transform.position == pawn.transform.position)
        {
            DestroyItem();
        }
    }
}
