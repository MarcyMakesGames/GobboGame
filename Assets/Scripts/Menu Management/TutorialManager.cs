using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject introText;
    [Space]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject statsText;
    [SerializeField] private GameObject historyText;
    [SerializeField] private GameObject buttonsText;
    [Space]
    [SerializeField] private GameObject goblinPanel;
    [SerializeField] private GameObject goblinText;
    [SerializeField] private GameObject goblinName;
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
                previousTutorialButton.SetActive(false);
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
                goblinText.SetActive(true);
                previousTutorialButton.SetActive(true);
                nextTutorialButton.SetActive(true);
                break;

            case TutorialTypeEnum.GoblinCutsomization:
                goblinPanel.SetActive(true);
                goblinName.SetActive(true);
                previousTutorialButton.SetActive(true);
                startGameButton.SetActive(true);
                break;

            case TutorialTypeEnum.Game:
                TMP_Text nameText = goblinName.GetComponent<TMP_Text>();
                PawnManager.instance.SetName(nameText.text, null, null);
                gamePanel.SetActive(true);
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
        goblinText.SetActive(false);
        goblinName.SetActive(false);
        gamePanel.SetActive(false);
        nextTutorialButton.SetActive(false);
        previousTutorialButton.SetActive(false);
        startGameButton.SetActive(false);
    }
}
