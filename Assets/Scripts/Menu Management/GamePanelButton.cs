using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelButton : MonoBehaviour
{
    [SerializeField] private GamePanelManager gamePanelManager;
    [SerializeField] private PanelType panelType;

    public void SwapPanel()
    {
        gamePanelManager.ChangeGamePanel(panelType);
    }
}
