using PawnControl;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private SaveDataController saveController;

    public delegate void onPlayerDataUpdated(SaveDataObject data);
    public static onPlayerDataUpdated OnPlayerDataUpdated;


    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        saveController = new SaveDataController();
    }

    public void SaveAccountData()
    {
        saveController.SaveAccountData(false);
    }

    public void UpdateAccountData()
    {
        saveController.SaveAccountData(true);
    }

    public void LoadAccountData(UserPassObject userPassObject)
    {
        saveController.LoadAccountData(userPassObject);
    }

    public void CreateAccountData(UserPassObject userPassObject)
    {
        saveController.CreateAccountData(userPassObject);
    }

    public void UpdatePlayerData(SaveDataObject data = null)
    {
        OnPlayerDataUpdated?.Invoke(data);
    }
}
