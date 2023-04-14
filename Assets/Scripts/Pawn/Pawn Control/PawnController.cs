using PawnControl;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PawnController : MonoBehaviour, IUpdateOnHour
{
    [SerializeField] private PawnStatusController statusController;
    [SerializeField] private PawnAnimatorController animatorController;
    [SerializeField] private PawnMoveController moveController;

    public PawnStatusController PawnStatusController { get => statusController; }
    public PawnMoveController PawnMoveController { get => moveController; }

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

    public void SetAnimation(AnimationEnums animationDirection, Action onAnimationComplete = null)
    {
        animatorController.SetAnimation(animationDirection, onAnimationComplete);
    }

    public void UpdateMentalStatus(MentalStatus newStatus, int magnitude)
    {
        statusController.UpdateMentalStatus(newStatus, magnitude);
    }

    public void UpdatePhysicalStatus(PhysicalStatus newStatus, int magnitude)
    {
        statusController.UpdatePhysicalStatus(newStatus, magnitude);
    }

    public void UpdateNeedStatus(NeedStatus needStatus, int magnitude)
    {
        statusController.UpdateNeedStatus(needStatus, magnitude);
    }

    public void SetFirstName(string name)
    {
        if (name == null || name == string.Empty)
            return;

        statusController.SetFirstName(name);
    }

    public void SetNickName(string name)
    {
        if (name == null || name == string.Empty)
            return;

        statusController.SetNickname(name);
    }

    public void SetLastName(string name)
    {
        if (name == null || name == string.Empty)
            return;

        statusController.SetLastName(name);
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
