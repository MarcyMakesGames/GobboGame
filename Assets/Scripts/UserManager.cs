using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    [SerializeField] private string username;
    [SerializeField] private string password;
    [SerializeField] private SessionData sessionData;
    [SerializeField] private List<SessionData> sessionDataList;

    private SaveDataObject saveData;

    private float quitCountdown = 2f;
    private bool quitGame = false;
    private bool hasUser = false;
    private bool hasUpdatedSaveData = false;

    public string Username { get => username; set => username = value; }
    public string Password { get; private set; }

    public void InitializeUserLogin(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public List<SessionData> GetSessionDataList()
    {
        if (!hasUpdatedSaveData)
        {
            sessionData.logoutTime = DateTime.Now;
            sessionDataList.Add(sessionData);

            Debug.Log("Saving logout time: " + sessionData.logoutTime);
            hasUpdatedSaveData = true;
        }
        else
        {
            sessionData.logoutTime = DateTime.Now;
            sessionDataList[sessionDataList.Count - 1] = sessionData;
            Debug.Log("Saving logout time: " + sessionData.logoutTime);
        }

        return sessionDataList;
    }

    public SessionData GetPreviousSessionData()
    {
        if(sessionDataList.Count == 0)
        {
            Debug.Log("Found no session data.");
            return null;
        }
        else
            return sessionDataList[sessionDataList.Count - 1];
    }

    public SessionData GetCurrentSessionData()
    {
        return sessionData;
    }

    public void LogInteraction(ActivityStatus activity)
    {
        sessionData = GetCurrentSessionData();

        switch (activity)
        {
            case ActivityStatus.Wander:
                break;

            case ActivityStatus.Play:
                sessionData.timesPlayed++;
                break;

            case ActivityStatus.Eat:
                sessionData.timesFed++;
                break;

            case ActivityStatus.Sleep:
                sessionData.timesRested++;
                break;
        }

        SaveLoadManager.instance.UpdateAccountData();
    }

    public void QuitGame()
    {
        if(hasUser)
        {
            SaveLoadManager.instance.SaveAccountData();
        }

        quitCountdown = 2;
        quitGame = true;
    }


    private void Awake()
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

        SaveLoadManager.OnPlayerDataUpdated += InitializeUserData;
    }

    private void Update()
    {
        if (quitGame)
        {
            quitCountdown -= Time.deltaTime;
            if (quitCountdown <= 0)
            {
                Application.Quit();
            }
        }
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnPlayerDataUpdated -= InitializeUserData;
    }

    private void InitializeUserData(SaveDataObject userSaveData)
    {
        if (userSaveData == null)
            return;

        saveData = userSaveData;
        sessionData = new SessionData();
        sessionData.loginTime = DateTime.Now;

        if (userSaveData.sessionDataList == null)
        {
            sessionDataList = new List<SessionData>();
        }
        else
        {
            sessionDataList = userSaveData.sessionDataList;
        }

        hasUser = true;

        PawnManager.instance.InitializePawn(saveData);
    }
}
