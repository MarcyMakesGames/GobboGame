using UnityEngine;

public class PawnMoveController : MonoBehaviour
{
    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float playMoveSpeed = 5f;
    [SerializeField] private float pauseDurationMinimum = 2f;
    [SerializeField] private float pauseDurationMaximum = 10f;

    private Vector3 minPosition;
    private Vector3 maxPosition;
    private Vector3 targetPosition;
    private Vector3 centerPosition;
    private Vector3 bottomPosition;

    private bool isMovingToCenter = false;
    private bool isMovingToBottom = false;
    private bool hasAnnouncedArrival = false;
    private bool isPlayerControlled;
    private bool isPaused = true;
    private float pauseTimer;
    private float pauseDuration;
    private float moveTimer;

    public delegate void onPawnReachedCenter();
    public static event onPawnReachedCenter OnPawnReachedCenter;

    public delegate void onPawnReachedBottom();
    public static event onPawnReachedBottom OnPawnReachedBottom;

    public void LockPawnToCenter(bool moveToCenter)
    {
        isMovingToCenter = moveToCenter;

        if(isMovingToCenter)
        {
            targetPosition = centerPosition;
            isPaused = false;
        }

        if(!isMovingToCenter)
        {
            targetPosition = GetRandomPosition();
            hasAnnouncedArrival = false;
            isPaused = false;
        }
    }

    public void LockPawnToBottom(bool moveToBottom)
    {
        isMovingToBottom = moveToBottom;

        if (isMovingToBottom)
        {
            targetPosition = bottomPosition;
            isPaused = false;
        }

        if (!isMovingToBottom)
        {
            targetPosition = GetRandomPosition();
            isPlayerControlled = false;
            isPaused = false;
        }
    }

    private void Start()
    {
        Camera mainCamera = Camera.main;
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);
        centerPosition = new Vector3(centerPosition.x, centerPosition.y, 0);

        bottomPosition = new Vector3(Screen.width / 2, Screen.height / 4, 0);
        bottomPosition = Camera.main.ScreenToWorldPoint(bottomPosition);
        bottomPosition = new Vector3(bottomPosition.x, bottomPosition.y, 0);

        minPosition = new Vector3(mainCamera.transform.position.x - camWidth + transform.localScale.x,
                                  mainCamera.transform.position.y - camHeight + transform.localScale.y,
                                  transform.position.z);

        maxPosition = new Vector3(mainCamera.transform.position.x + camWidth - transform.localScale.x,
                                  mainCamera.transform.position.y + camHeight - transform.localScale.y,
                                  transform.position.z);

        targetPosition = GetRandomPosition();
    }

    private void Update()
    {
        if(isPlayerControlled)
        {
            if(Input.GetKey(KeyCode.A))
            {
                transform.position = Vector3.MoveTowards(transform.position,
                                                        new Vector3(Mathf.Clamp(transform.position.x - 1, minPosition.x, maxPosition.x),
                                                                                transform.position.y, transform.position.z),
                                                        playMoveSpeed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.D))
            {
                transform.position = Vector3.MoveTowards(transform.position,
                                                        new Vector3(Mathf.Clamp(transform.position.x + 1, minPosition.x, maxPosition.x),
                                                                                transform.position.y, transform.position.z),
                                                        playMoveSpeed * Time.deltaTime);
            }

            return;
        }

        if (!isPaused)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
            moveTimer += Time.deltaTime;

            if (transform.position == targetPosition && !isMovingToCenter && !isMovingToBottom)
            {
                isPaused = true;
                pauseTimer = 0f;
                pauseDuration = Random.Range(pauseDurationMinimum, pauseDurationMaximum);
            }
            else if (transform.position == targetPosition && isMovingToCenter && !isMovingToBottom && !hasAnnouncedArrival)
            {
                OnPawnReachedCenter?.Invoke();
                hasAnnouncedArrival = true;
            }
            else if (transform.position == targetPosition && !isMovingToCenter && isMovingToBottom && !isPlayerControlled)
            {
                OnPawnReachedBottom?.Invoke();
                isPlayerControlled = true;
            }
        }
        else
        {
            pauseTimer += Time.deltaTime;

            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                moveTimer = 0f;
                targetPosition = GetRandomPosition();
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(minPosition.x, maxPosition.x), 
                           Random.Range(minPosition.y, maxPosition.y), 
                           transform.position.z);
    }
}
