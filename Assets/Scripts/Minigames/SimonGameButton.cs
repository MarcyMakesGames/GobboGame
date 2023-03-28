using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGameButton : MonoBehaviour
{
    [SerializeField] private SimonGameManager simonGameManager;
    [SerializeField] private AnimationEnums buttonDirection;

    public void ButtonPress()
    {
        simonGameManager.CheckButton(buttonDirection);
    }
}
