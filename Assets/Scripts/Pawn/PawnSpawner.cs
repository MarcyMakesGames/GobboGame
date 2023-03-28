using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : MonoBehaviour 
{
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private Transform pawnAnchor;

    public void SpawnNewPawn(PawnStatusContainer pawnStatusContainer)
    {
        if (PawnManager.instance.Controller != null)
            return;

        GameObject newPawn = Instantiate(pawnPrefab, pawnAnchor);
        PawnController pawnController = newPawn.GetComponent<PawnController>();

        PawnManager.instance.AssignNewPawnController(pawnController, pawnStatusContainer);
    }
}
