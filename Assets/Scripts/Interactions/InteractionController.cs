using PawnControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private PawnStatusController pawnStatusController;

    public void FeedPawn()
    {
        pawnStatusController.UpdateNeedStatus(NeedStatus.Hunger, 5);
    }

    public void PlayWithPawn()
    {
        pawnStatusController.UpdateNeedStatus(NeedStatus.Entertainment, 5);
    }

    public void PutPawnToBed()
    {
        pawnStatusController.UpdateNeedStatus(NeedStatus.Sleep, 5);
    }

    private void Start()
    {
        pawnStatusController = PawnManager.instance.PawnController.PawnStatusController;
    }
}
