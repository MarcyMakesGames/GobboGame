using PawnControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PawnManager : MonoBehaviour, IUpdateOnHour
{
    public static PawnManager instance;
    public PawnController PawnController { get => pawnController; }
    public GameObject PawnObject { get => pawnObject; }

    [SerializeField] private GamePanelManager gamePanelManager;
    [Space]
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private Transform pawnAnchor;

    private PawnController pawnController;
    private GameObject pawnObject;

    private bool delayLoad = false;
    private float delayLoadTimer = 1f;

    public void InitializePawn(SaveDataObject data)
    {
        Debug.Log("Initializing pawn.");
        SpawnNewPawn(data.pawnStatusContainer);
        pawnController.InitPawnController(data.pawnStatusContainer);

        delayLoad = true;
    }

    public void SetAnimation(AnimationEnums animationDirection, Action onAnimationComplete = null)
    {
        pawnController.SetAnimation(animationDirection, onAnimationComplete);
    }

    public void MovePawnToPlayPosition()
    {
        pawnController.PawnMoveController.MovePawnToPosition(ActivityStatus.Play);
    }

    public void MovePawnToFeedingPosition()
    {
        pawnController.PawnMoveController.MovePawnToPosition(ActivityStatus.Eat);
    }

    public void MovePawnToSleepingPosition()
    {
        pawnController.PawnMoveController.MovePawnToPosition(ActivityStatus.Sleep);
    }

    public void MovePawnToWanderPosition()
    {
        pawnController.PawnMoveController.MovePawnToPosition(ActivityStatus.Wander);
    }

    public void InteractedWithPawn(ActivityStatus activityStatus)
    {
        UserManager.instance.LogInteraction(activityStatus);
    }

    public void UpdateStatus(MentalStatus newStatus, int magnitude)
    {
        if (newStatus == MentalStatus.None)
            return;

        pawnController.UpdateMentalStatus(newStatus, magnitude);
    }

    public void UpdateStatus(PhysicalStatus newStatus, int magnitude)
    {
        if(newStatus == PhysicalStatus.None)
            return;

        pawnController.UpdatePhysicalStatus(newStatus, magnitude);
    }

    public void UpdateStatus(NeedStatus newStatus, int magnitude)
    {
        if (newStatus == NeedStatus.None)
            return;

        pawnController.UpdateNeedStatus(newStatus, magnitude);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        DateTimeManager.instance.SubscribeToHourlyUpdate(this);
    }

    private void Update()
    {
        if (delayLoad)
        {
            delayLoadTimer -= Time.deltaTime;

            if (delayLoadTimer <= 0)
            {
                Debug.Log("Delayed load complete.");
                UpdatePawnSinceLastLogin();
                delayLoad = false;
            }
        }
    }

    private void SpawnNewPawn(PawnStatusContainer pawnStatusContainer)
    {
        pawnObject = Instantiate(pawnPrefab, pawnAnchor);
        pawnController = pawnObject.GetComponent<PawnController>();
    }

    private void UpdatePawnSinceLastLogin()
    {
        SessionData sessionData = UserManager.instance.GetPreviousSessionData();

        if(sessionData == null || sessionData.logoutTime.Year == 0001)
        {
            if (sessionData == null)
                Debug.Log("No previous session data found.");
            else
                Debug.Log("Invalid session time: " + sessionData.logoutTime + ". If this was in error, please contact an administrator.");
            return;
        }
        else
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastLogin = sessionData.logoutTime;
            TimeSpan timeSpan = currentTime - lastLogin;
            Debug.Log("Previous session logout time: " + sessionData.logoutTime + ".");

            for (int i = 0; i < (int)timeSpan.TotalHours; i++)
            {
                Debug.Log("Updating statuses.");
                IncrementAllStatuses();
            }

            for(int i = 0; i < (int)timeSpan.TotalHours * 4; i++)
                HistoryManager.instance.PostNewEvent();

            gamePanelManager.UpdateStatsPanel(pawnController.PawnStatusController);
        }
    }

    private void IncrementAllStatuses()
    {
        pawnController.PawnStatusController.IncrementAllStatuses();
        gamePanelManager.UpdateStatsPanel(pawnController.PawnStatusController);
    }

    public void UpdateOnHour()
    {
        IncrementAllStatuses();
        gamePanelManager.UpdateStatsPanel(pawnController.PawnStatusController);
    }
}
