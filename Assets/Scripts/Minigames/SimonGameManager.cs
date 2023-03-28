using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGameManager : MonoBehaviour
{
    [SerializeField] private GamePanelManager gamePanelManager;
    [SerializeField] private GameObject buttonTray;

    private List<AnimationEnums> currentGame;
    private Queue<AnimationEnums> currentRound;
    private int currentGuessIndex = 0;

    public void StartNewGame()
    {
        currentGame = new List<AnimationEnums>();
        currentRound = new Queue<AnimationEnums>();

        StartNewRound();
    }

    public void StartNewRound()
    {
        buttonTray.SetActive(false);

        int randNum = Random.Range(1, 5);
        AnimationEnums randomEnum = (AnimationEnums)randNum;

        currentGame.Add(randomEnum);
        
        foreach(AnimationEnums direction in currentGame)
            currentRound.Enqueue(direction);

        PawnManager.instance.SetAnimation(currentRound.Dequeue());
    }

    public void CheckButton(AnimationEnums button)
    {
        if (currentGame[currentGuessIndex] == button)
        {
            currentGuessIndex++;

            if (currentGuessIndex == currentGame.Count)
            {
                buttonTray.SetActive(false);

                if(currentGame.Count >= 2)
                {
                    PawnManager.instance.SetAnimation(AnimationEnums.Celebration, CloseGame);
                }
                else
                    StartNewRound();
            }
        }
        else
        {
            PawnManager.instance.SetAnimation(AnimationEnums.Negative);
            currentGame.Clear();
            currentRound.Clear();
        }
    }

    private void OnEnable()
    {
        PawnAnimatorController.OnAnimationComplete += ShowNextMove;
        PawnMoveController.OnPawnReachedCenter += StartNewGame;
    }

    private void OnDisable()
    {
        PawnMoveController.OnPawnReachedCenter -= StartNewGame;
        PawnAnimatorController.OnAnimationComplete -= ShowNextMove;

        buttonTray.SetActive(false);
        currentGame = null;
        currentRound = null;
    }

    private void ShowNextMove()
    {
        if (currentRound == null)
            return;

        if (currentGame.Count == 0)
            StartNewGame();

        if (currentRound.Count == 0)
        {
            buttonTray.SetActive(true);
            currentGuessIndex = 0;
            return;
        }
        else
        {
            PawnManager.instance.SetAnimation(currentRound.Dequeue());
        }
    }

    private void CloseGame()
    {
        Debug.Log("Closing game.");
        PawnManager.instance.Controller.PawnStatusController.UpdateNeedStatus(NeedStatus.Entertainment, 7);
        buttonTray.SetActive(false);
        currentGame.Clear();
        currentRound.Clear();
        currentGuessIndex = 0;

        PawnManager.instance.LockPawnToCenter(false);
        gamePanelManager.ChangeGamePanel(PanelType.Stats);
    }
}
