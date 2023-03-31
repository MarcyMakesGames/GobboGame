using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepGameDreamController : MonoBehaviour
{
    public DreamType DreamType { get { return dreamType; } }

    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private DreamType dreamType;

    private Rigidbody2D rb;
    private SleepGameManager sleepingGameManager;
    private Transform pawn;

    private float moveSpeed;


    public void OnCollisionEnter2D(Collision2D collision)
    {
        PawnMoveController pawnMoveController = collision.gameObject.GetComponent<PawnMoveController>();

        if (pawnMoveController != null)
        {
            rb.simulated = false;
            sleepingGameManager.ConsumeDream(this);
            Destroy(gameObject);
        }
    }

    public void DestroyItem()
    {
        sleepingGameManager.RemoveFromActiveDreams(gameObject);
        Destroy(gameObject);
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        sleepingGameManager = FindObjectOfType<SleepGameManager>();
        pawn = PawnManager.instance.PawnObject.transform;

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pawn.transform.position, moveSpeed * Time.deltaTime);
    }
}
