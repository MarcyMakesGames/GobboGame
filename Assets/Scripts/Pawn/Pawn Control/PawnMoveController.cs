using UnityEngine;

public class PawnMoveController : MonoBehaviour
{
    public delegate void onPawnReachedPlayPosition();
    public static event onPawnReachedPlayPosition OnPawnReachedPlayPosition;

    public delegate void onPawnReachedEatPosition();
    public static event onPawnReachedEatPosition OnPawnReachedEatPosition;

    public delegate void onPawnReachedSleepPosition();
    public static event onPawnReachedSleepPosition OnPawnReachedSleepPosition;

    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float playMoveSpeed = 5f;
    [SerializeField] private float pauseDurationMinimum = 2f;
    [SerializeField] private float pauseDurationMaximum = 10f;

    private Vector3 minPosition;
    private Vector3 maxPosition;
    private Vector3 targetPosition;
    private Vector3 centerPosition;
    private Vector3 bottomPosition;

    private bool arrivedAtPlayPosition = false;
    private bool arrivedAtEatPosition = false;
    private bool arrivedAtSleepPosition = false;
    private bool isPaused = true;
    private float pauseTimer;
    private float pauseDuration;
    private ActivityStatus currentActivityStatus = ActivityStatus.Wander;

    public void SetActivityStatus(ActivityStatus activityStatus)
    {
        currentActivityStatus = activityStatus;
    }

    public void MovePawnToPosition(ActivityStatus statusPosition)
    {
        currentActivityStatus = statusPosition;
        arrivedAtPlayPosition = false;
        arrivedAtEatPosition = false;
        arrivedAtSleepPosition = false;

        switch (currentActivityStatus)
        {
            case ActivityStatus.Wander:
                targetPosition = GetRandomPosition();
                break;
            case ActivityStatus.Play:
                targetPosition = centerPosition;
                break;
            case ActivityStatus.Eat:
                targetPosition = bottomPosition;
                break;
            case ActivityStatus.Sleep:
                break;
        }

        isPaused = false;
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
        if(arrivedAtEatPosition)
        {
            InterpretPlayerMovement();
            return;
        }

        switch (currentActivityStatus)
        {
            case ActivityStatus.Wander:
                Wander();
                break;
            case ActivityStatus.Play:
                MoveToPlayPosition();
                break;
            case ActivityStatus.Eat:
                MoveToEatPosition();
                break;
            case ActivityStatus.Sleep:
                break;
        }
    }

    private void MoveToEatPosition()
    {
        if (!arrivedAtEatPosition)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);

        if (transform.position == targetPosition && !arrivedAtEatPosition)
        {
            OnPawnReachedEatPosition?.Invoke();
            arrivedAtEatPosition = true;
        }
    }

    private void MoveToPlayPosition()
    {
        if(!arrivedAtPlayPosition)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);

        if (transform.position == targetPosition && !arrivedAtPlayPosition)
        {
            OnPawnReachedPlayPosition?.Invoke();
            arrivedAtPlayPosition = true;
        }
    }

    private void Wander()
    {
        if (!isPaused)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isPaused = true;
                pauseTimer = 0f;
                pauseDuration = Random.Range(pauseDurationMinimum, pauseDurationMaximum);
            }
        }
        else
        {
            pauseTimer += Time.deltaTime;

            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                targetPosition = GetRandomPosition();
            }
        }
    }

    private void InterpretPlayerMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                    new Vector3(Mathf.Clamp(transform.position.x - 1, minPosition.x, maxPosition.x),
                                                                            transform.position.y, transform.position.z),
                                                    playMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                    new Vector3(Mathf.Clamp(transform.position.x + 1, minPosition.x, maxPosition.x),
                                                                            transform.position.y, transform.position.z),
                                                    playMoveSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(minPosition.x, maxPosition.x), 
                           Random.Range(minPosition.y, maxPosition.y), 
                           transform.position.z);
    }
}
