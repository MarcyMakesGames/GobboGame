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
        GameObject newPawn = Instantiate(pawnPrefab, pawnAnchor);
        PawnController pawnController = newPawn.GetComponent<PawnController>();

        PawnManager.instance.AssignNewPawnController(pawnController, pawnStatusContainer);
    }
}
