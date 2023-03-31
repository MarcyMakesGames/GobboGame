using PawnControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PawnManager : MonoBehaviour
{
    public static PawnManager instance;

    [SerializeField] private GamePanelManager gamePanelManager;

    private PawnController controller;

    public PawnController Controller { get => controller; }
    public GameObject PawnObject { get => controller.gameObject; }

    public void SetAnimation(AnimationEnums animationDirection, Action onAnimationComplete = null)
    {
        controller.SetAnimation(animationDirection, onAnimationComplete);
    }

    public void AssignNewPawnController(PawnController newPawn, PawnStatusContainer pawnStatusContainer)
    {
        controller = newPawn;
        controller.InitPawnController(pawnStatusContainer);

        UpdatePawnSinceLastLogin();
    }

    public void MovePawnToPlayPosition()
    {
        controller.PawnMoveController.MovePawnToPosition(ActivityStatus.Play);
    }

    public void MovePawnToFeedingPosition()
    {
        controller.PawnMoveController.MovePawnToPosition(ActivityStatus.Eat);
    }

    public void MovePawnToSleepingPosition()
    {
        controller.PawnMoveController.MovePawnToPosition(ActivityStatus.Sleep);
    }

    public void MovePawnToWanderPosition()
    {
        controller.PawnMoveController.MovePawnToPosition(ActivityStatus.Wander);
    }

    public void InteractedWithPawn(ActivityStatus activityStatus)
    {
        UserManager.instance.LogInteraction(activityStatus);
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

    private void UpdatePawnSinceLastLogin()
    {
        SessionData sessionData = UserManager.instance.GetLastSessionData();

        if(sessionData == null || sessionData.logoutTime.Year == 0001)
        {
            return;
        }
        else
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastLogin = sessionData.logoutTime;
            TimeSpan timeSpan = currentTime - lastLogin;
            
            for (int i = 0; i < (int)timeSpan.TotalHours; i++)
                IncrementAllStatuses();
        }
    }

    private void IncrementAllStatuses()
    {
        controller.PawnStatusController.IncrementAllStatuses();
        gamePanelManager.UpdateStatsPanel(controller.PawnStatusController);
    }
}
