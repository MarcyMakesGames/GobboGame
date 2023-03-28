using UnityEngine;

public class PawnMoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pauseDurationMinimum = 2f;
    [SerializeField] private float pauseDurationMaximum = 10f;

    private Vector3 minPosition;
    private Vector3 maxPosition;
    private Vector3 targetPosition;
    private Vector3 centerPosition;

    private bool isMovingToCenter = false;
    private bool hasAnnouncedArrival = false;
    private bool isPaused = true;
    private float pauseTimer;
    private float pauseDuration;
    private float moveTimer;

    public delegate void onPawnReachedCenter();
    public static event onPawnReachedCenter OnPawnReachedCenter;

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

    private void Start()
    {
        Camera mainCamera = Camera.main;
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        centerPosition = Camera.main.ScreenToWorldPoint(centerPosition);
        centerPosition = new Vector3(centerPosition.x, centerPosition.y, 0);

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
        if(isMovingToCenter)
            targetPosition = centerPosition;

        if (!isPaused)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            moveTimer += Time.deltaTime;

            if (transform.position == targetPosition && !isMovingToCenter)
            {
                isPaused = true;
                pauseTimer = 0f;
                pauseDuration = Random.Range(pauseDurationMinimum, pauseDurationMaximum);
            }
            else if (transform.position == targetPosition && isMovingToCenter && !hasAnnouncedArrival)
            {
                OnPawnReachedCenter?.Invoke();
                hasAnnouncedArrival = true;
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
