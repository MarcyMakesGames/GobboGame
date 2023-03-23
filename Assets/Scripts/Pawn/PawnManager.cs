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

    public PawnStatusController GetPawnStatusController()
    {
        return controller.PawnStatusController;
    }

    public void AssignNewPawnController(PawnController newPawn, PawnStatusContainer pawnStatusContainer)
    {
        controller = newPawn;
        controller.InitPawnController(pawnStatusContainer);

        UpdatePawnSinceLastLogin();
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

        if(sessionData == null)
        {
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
            }
        }
    }

    private void IncrementAllStatuses()
    {
        controller.PawnStatusController.IncrementAllStatuses();
        gamePanelManager.UpdateStatsPanel(controller.PawnStatusController);
    }
}
