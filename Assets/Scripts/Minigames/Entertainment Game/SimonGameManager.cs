using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimonGameManager : MonoBehaviour
{
    [SerializeField] private GamePanelManager gamePanelManager;
    [SerializeField] private GameObject buttonTray;
    [SerializeField] private TMP_Text instructionsText;
    [SerializeField] private int maxRounds = 5;
    [TextArea(1,5)]
    [SerializeField] private string instructions;


    private List<AnimationEnums> currentGame;
    private Queue<AnimationEnums> currentRound;
    private int currentGuessIndex = 0;

    public void StartNewGame()
    {
        currentGame = new List<AnimationEnums>();
        currentRound = new Queue<AnimationEnums>();

        instructionsText.text = instructions;
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

        PawnManager.instance.SetAnimation(currentRound.Dequeue(), ShowNextMove);
    }

    public void CheckButton(AnimationEnums button)
    {
        if(currentGuessIndex > currentGame.Count)
        {
            PawnManager.instance.SetAnimation(AnimationEnums.Failure, LoseGame);
            return;
        }

        switch (button)
        {
            case AnimationEnums.Up:
                AudioManager.instance.PlaySound(SoundType.highBlip);
                break;
            case AnimationEnums.Down:
                AudioManager.instance.PlaySound(SoundType.LowBlip);
                break;
            case AnimationEnums.Left:
                AudioManager.instance.PlaySound(SoundType.highMidBlip);
                break;
            case AnimationEnums.Right:
                AudioManager.instance.PlaySound(SoundType.LowMidBlip);
                break;
            case AnimationEnums.Celebration:
                break;
            case AnimationEnums.Failure:
                break;
        }

        if (currentGame[currentGuessIndex] == button)
        {
            currentGuessIndex++;

            if (currentGuessIndex == currentGame.Count)
            {
                buttonTray.SetActive(false);

                if(currentGame.Count >= maxRounds)
                {
                    AudioManager.instance.PlaySound(SoundType.Celebration);
                    PawnManager.instance.SetAnimation(AnimationEnums.Celebration, WinGame);
                }
                else
                    StartNewRound();
            }
        }
        else
        {
            AudioManager.instance.PlaySound(SoundType.Failure);
            PawnManager.instance.SetAnimation(AnimationEnums.Failure, LoseGame);
        }
    }



    private void OnEnable()
    {
        PawnMoveController.OnPawnReachedPlayPosition += StartNewGame;
    }

    private void OnDisable()
    {
        PawnMoveController.OnPawnReachedPlayPosition -= StartNewGame;
        PawnManager.instance.MovePawnToWanderPosition();

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
            PawnManager.instance.SetAnimation(currentRound.Dequeue(), ShowNextMove);
        }
    }

    private void WinGame()
    {
        buttonTray.SetActive(false);
        currentGame.Clear();
        currentRound.Clear();
        currentGuessIndex = 0;

        PawnManager.instance.InteractedWithPawn(ActivityStatus.Play);
        PawnManager.instance.Controller.PawnStatusController.UpdateNeedStatus(NeedStatus.Entertainment, 7);

        PawnManager.instance.MovePawnToWanderPosition();
        gamePanelManager.ChangeGamePanel(PanelType.Stats);
    }

    private void LoseGame()
    {
        buttonTray.SetActive(false);
        currentGame.Clear();
        currentRound.Clear();
        currentGuessIndex = 0;

        PawnManager.instance.InteractedWithPawn(ActivityStatus.Play);
        PawnManager.instance.Controller.PawnStatusController.UpdateNeedStatus(NeedStatus.Entertainment, -1);

        PawnManager.instance.MovePawnToWanderPosition();
        gamePanelManager.ChangeGamePanel(PanelType.Stats);
    }
}
