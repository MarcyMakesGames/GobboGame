using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GoblinBuildController spriteBuildController;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject introText;
    [Space]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject statsText;
    [SerializeField] private GameObject historyText;
    [SerializeField] private GameObject buttonsText;
    [Space]
    [SerializeField] private GameObject goblinPanel;
    [Space]
    [SerializeField] private GameObject gamePanel;
    [Space]
    [SerializeField] private GameObject nextTutorialButton;
    [SerializeField] private GameObject previousTutorialButton;
    [SerializeField] private GameObject startGameButton;

    private TutorialTypeEnum tutorialType = TutorialTypeEnum.Intro;
    private int currentTutorialIndex = 1;

    public void LoadNextTutorial()
    {
        currentTutorialIndex++;
        tutorialType = (TutorialTypeEnum)currentTutorialIndex;

        ChangeTutorialPage(tutorialType);
    }

    public void LoadPreviousTutorial()
    {
        currentTutorialIndex--;
        tutorialType = (TutorialTypeEnum)currentTutorialIndex;

        ChangeTutorialPage(tutorialType);
    }

    private void ChangeTutorialPage(TutorialTypeEnum tutorialType)
    {
        CloseAllTutorialPages();

        switch (tutorialType)
        {
            case TutorialTypeEnum.Intro:
                introText.SetActive(true);
                nextTutorialButton.SetActive(true);
                break;

            case TutorialTypeEnum.Stats:
                statsPanel.SetActive(true);
                statsText.SetActive(true);
                previousTutorialButton.SetActive(true);
                nextTutorialButton.SetActive(true);
                break;

            case TutorialTypeEnum.History:
                statsPanel.SetActive(true);
                historyText.SetActive(true);
                previousTutorialButton.SetActive(true);
                nextTutorialButton.SetActive(true);
                break;

            case TutorialTypeEnum.Buttons:
                statsPanel.SetActive(true);
                buttonsText.SetActive(true);
                previousTutorialButton.SetActive(true);
                nextTutorialButton.SetActive(true);
                break;

            case TutorialTypeEnum.Goblin:
                goblinPanel.SetActive(true);
                previousTutorialButton.SetActive(true);
                nextTutorialButton.SetActive(true);
                break;

            case TutorialTypeEnum.GoblinCutsomization:
                goblinPanel.SetActive(true);
                previousTutorialButton.SetActive(true);
                startGameButton.SetActive(true);
                break;

            case TutorialTypeEnum.Game:
                UserManager.instance.SpawnPawn();
                spriteBuildController.InitializePawn();
                gamePanel.SetActive(true);
                tutorialPanel.SetActive(false);
                break;
        }
    }

    private void CloseAllTutorialPages()
    {
        introText.SetActive(false);
        statsPanel.SetActive(false);
        statsText.SetActive(false);
        historyText.SetActive(false);
        buttonsText.SetActive(false);
        goblinPanel.SetActive(false);
        gamePanel.SetActive(false);
        nextTutorialButton.SetActive(false);
        previousTutorialButton.SetActive(false);
        startGameButton.SetActive(false);
    }
}
