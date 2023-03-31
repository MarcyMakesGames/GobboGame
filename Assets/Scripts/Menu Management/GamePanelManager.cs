using PawnControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject foodPanel;
    [SerializeField] private GameObject sleepPanel;
    [SerializeField] private GameObject entertainmentPanel;
    [Space]
    [SerializeField] private Slider happinessSlider;
    [SerializeField] private Slider foodSlider;
    [SerializeField] private Slider sleepSlider;
    [SerializeField] private Slider entertainmentSlider;

    public void ChangeGamePanel(PanelType panel)
    {
        switch (panel)
        {
            case PanelType.None:
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(false);
                break;
            case PanelType.Entertainment:
                PawnManager.instance.MovePawnToPlayPosition();
                entertainmentPanel.SetActive(!entertainmentPanel.activeSelf);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(false);
                break;
            case PanelType.Food:
                PawnManager.instance.MovePawnToFeedingPosition();
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(!foodPanel.activeSelf);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(false);
                break;
            case PanelType.Sleep:
                PawnManager.instance.MovePawnToSleepingPosition();
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(!sleepPanel.activeSelf);
                statsPanel.SetActive(false);
                break;
            case PanelType.Stats:
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(true);
                UpdateStatsPanel(PawnManager.instance.Controller.PawnStatusController);
                break;
        }
    }

    public void UpdateStatsPanel(PawnStatusController pawnStatusController)
    {
        happinessSlider.value = pawnStatusController.GetHappinessPercent();
        foodSlider.value = pawnStatusController.GetHungerPercent();
        sleepSlider.value = pawnStatusController.GetSleepPercent();
        entertainmentSlider.value = pawnStatusController.GetEntertainmentPercent();
    }

    private void Start()
    {
        UpdateStatsPanel(PawnManager.instance.Controller.PawnStatusController);
    }
}
