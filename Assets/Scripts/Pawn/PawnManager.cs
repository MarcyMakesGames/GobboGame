using PawnControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PawnManager : MonoBehaviour
{
    public static PawnManager instance;

    [SerializeField] private GamePanelManager gamePanelManager;
    [Space]
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private Transform pawnAnchor;

    private PawnController pawnController;
    private GameObject pawnObject;
    public PawnController PawnController { get => pawnController; }
    public GameObject PawnObject { get => pawnObject; }

    public void SetAnimation(AnimationEnums animationDirection, Action onAnimationComplete = null)
    {
        pawnController.SetAnimation(animationDirection, onAnimationComplete);
    }

    public void InitializePawn(SaveDataObject data)
    {
        SpawnNewPawn(data.pawnStatusContainer);
        pawnController.InitPawnController(data.pawnStatusContainer);

        UpdatePawnSinceLastLogin();
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
                Debug.Log("Invalid session data.");
            else
                Debug.Log("Invalid session time:" + sessionData.logoutTime);
            return;
        }
        else
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastLogin = sessionData.logoutTime;
            TimeSpan timeSpan = currentTime - lastLogin;
            
            for (int i = 0; i < (int)timeSpan.TotalHours; i++)
            {
                IncrementAllStatuses();
                Debug.Log("Updating statuses.");
            }
        }
    }

    private void IncrementAllStatuses()
    {
        pawnController.PawnStatusController.IncrementAllStatuses();
        gamePanelManager.UpdateStatsPanel(pawnController.PawnStatusController);
    }
}
