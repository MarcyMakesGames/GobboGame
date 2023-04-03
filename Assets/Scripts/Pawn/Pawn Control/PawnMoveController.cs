using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PawnMoveController : MonoBehaviour
{
    public delegate void onPawnReachedPlayPosition();
    public static event onPawnReachedPlayPosition OnPawnReachedPlayPosition;

    public delegate void onPawnReachedEatPosition();
    public static event onPawnReachedEatPosition OnPawnReachedEatPosition;

    public delegate void onPawnReachedSleepPosition();
    public static event onPawnReachedSleepPosition OnPawnReachedSleepPosition;

    [SerializeField] private Animator pawnAnimator;
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
    private bool isWalking = false;
    private bool isSleeping = false;

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
                targetPosition = bottomPosition;
                SetTransformOrientation(bottomPosition);
                break;
            case ActivityStatus.Eat:
                targetPosition = bottomPosition;
                SetTransformOrientation(bottomPosition);
                break;
            case ActivityStatus.Sleep:
                targetPosition = bottomPosition;
                SetTransformOrientation(bottomPosition);
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

        float bottomLimit = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 3f, 0)).y;

        minPosition = new Vector3(mainCamera.transform.position.x - camWidth + transform.localScale.x,
                                  mainCamera.transform.position.y - camHeight,
                                  transform.position.z);

        maxPosition = new Vector3(mainCamera.transform.position.x + camWidth - transform.localScale.x,
                                  bottomLimit,
                                  transform.position.z);

        targetPosition = GetRandomPosition();
    }



    private void Update()
    {
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
                MoveToSleepPosition();
                break;
        }
    }

    private void MoveToSleepPosition()
    {
        if (!arrivedAtSleepPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
            SetPawnAnimation(MovementType.Walking);

            if (transform.position == targetPosition)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                SetPawnAnimation(MovementType.Sleeping);
                OnPawnReachedSleepPosition?.Invoke();
                arrivedAtSleepPosition = true;
            }
        }
    }

    private void MoveToEatPosition()
    {
        if (!arrivedAtEatPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
            SetPawnAnimation(MovementType.Walking);

            if (transform.position == targetPosition)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                SetPawnAnimation(MovementType.Idle);
                OnPawnReachedEatPosition?.Invoke();
                arrivedAtEatPosition = true;
            }
        }
        else
            InterpretPlayerMovement();
    }

    private void MoveToPlayPosition()
    {
        if(!arrivedAtPlayPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
            SetPawnAnimation(MovementType.Walking);

            if (transform.position == targetPosition)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                SetPawnAnimation(MovementType.Idle);
                OnPawnReachedPlayPosition?.Invoke();
                arrivedAtPlayPosition = true;
            }
        }            
    }

    private void Wander()
    {
        if (!isPaused)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
            SetPawnAnimation(MovementType.Walking);

            if (transform.position == targetPosition)
            {
                isPaused = true;
                pauseTimer = 0f;
                pauseDuration = Random.Range(pauseDurationMinimum, pauseDurationMaximum);
            }
        }
        else
        {
            SetPawnAnimation(MovementType.Idle);
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
        Debug.Log("Interpreting player movement.");
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Moving left.");
            transform.position = Vector3.MoveTowards(transform.position,
                                                    new Vector3(Mathf.Clamp(transform.position.x - 1, minPosition.x, maxPosition.x),
                                                                            transform.position.y, transform.position.z),
                                                    playMoveSpeed * Time.deltaTime);
            SetPawnAnimation(MovementType.Walking);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                    new Vector3(Mathf.Clamp(transform.position.x + 1, minPosition.x, maxPosition.x),
                                                                            transform.position.y, transform.position.z),
                                                    playMoveSpeed * Time.deltaTime);

            Debug.Log("Moving right.");
            SetPawnAnimation(MovementType.Walking);
            return;
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            SetPawnAnimation(MovementType.Idle);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y),
                                                        transform.position.z);

        SetTransformOrientation(newPosition);

        return newPosition;
    }

    private void SetTransformOrientation(Vector3 newPosition)
    {
        if (transform.position.x > newPosition.x)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }

    private void SetPawnAnimation(MovementType animation)
    {
        switch (animation)
        {
            case MovementType.Idle:
                
                if (!isWalking)
                    break;

                Debug.Log("New animation type: " + animation.ToString());
                pawnAnimator.SetBool("IsWalking", false);
                pawnAnimator.SetBool("IsSleeping", false);
                isWalking = false;
                isSleeping = false;
                break;
            case MovementType.Walking:
                
                if (isWalking)
                    break;

                Debug.Log("New animation type: " + animation.ToString());
                pawnAnimator.SetBool("IsWalking", true);
                pawnAnimator.SetBool("IsSleeping", false);
                isWalking = true;
                isSleeping = false;
                break;

            case MovementType.Sleeping:

                if (isSleeping)
                    break;

                Debug.Log("New animation type: " + animation.ToString());
                pawnAnimator.SetBool("IsWalking", false);
                pawnAnimator.SetBool("IsSleeping", true);
                isSleeping = true;
                isWalking = false;
                break;
        }
    }
}
