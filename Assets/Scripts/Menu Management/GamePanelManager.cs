using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject entertainmentPanel;
    [SerializeField] private GameObject foodPanel;
    [SerializeField] private GameObject sleepPanel;
    [SerializeField] private GameObject statsPanel;

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
                entertainmentPanel.SetActive(!entertainmentPanel.activeSelf);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(false);
                break;
            case PanelType.Food:
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(!foodPanel.activeSelf);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(false);
                break;
            case PanelType.Sleep:
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(!sleepPanel.activeSelf);
                statsPanel.SetActive(false);
                break;
            case PanelType.Stats:
                entertainmentPanel.SetActive(false);
                foodPanel.SetActive(false);
                sleepPanel.SetActive(false);
                statsPanel.SetActive(!statsPanel.activeSelf);
                break;
        }
    }
}
