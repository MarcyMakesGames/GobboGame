using PawnControl;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PawnController : MonoBehaviour, IUpdateOnHour
{
    [SerializeField] private PawnStatusController statusController;

    public PawnStatusController PawnStatusController { get => statusController; }

    public void InitPawnController(PawnStatusContainer statusContainer)
    {
        statusController.SetFirstName(statusContainer.FirstName);
        statusController.SetNickname(statusContainer.NickName);
        statusController.SetLastName(statusContainer.LastName);

        statusController.MentalStatus = new PawnMentalStatus(statusContainer.MentalStatusContainer);
        statusController.PhysicalStatus = new PawnPhysicalStatus(statusContainer.PhysicalStatusContainer);
        statusController.NeedsStatus = new PawnNeedsStatus(statusContainer.NeedsStatusContainer);        
    }

    public void UpdateOnHour()
    {
        IncrementAllStatuses();
    }

    private void Awake()
    {
        statusController = new PawnStatusController();
    }

    private void IncrementAllStatuses()
    {
        statusController.MentalStatus.IncrementAllStatuses();
        statusController.PhysicalStatus.IncrementAllStatuses();
        statusController.NeedsStatus.IncrementAllStatuses();
    }
}
