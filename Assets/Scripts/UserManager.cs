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

    public string Username { get => username; set => username = value; }
    public string Password { get; private set; }

    public void InitializeUserLogin(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public List<SessionData> CreateNewUserSessionDataList()
    {
        sessionData = GetLastSessionData();

        sessionData.logoutTime = DateTime.Now;
        return sessionDataList;
    }

    public SessionData GetLastSessionData()
    {
        if(sessionDataList.Count == 0)
        {
            Debug.Log("Found no session data.");
            return null;
        }
        else
            return sessionDataList[sessionDataList.Count - 1];
    }

    public void LogInteraction(ActivityStatus activity)
    {
        sessionData = GetLastSessionData();

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

        SaveLoadManager.instance.SaveAccountData();
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

    private void InitializeUserData(SaveDataObject userSaveData)
    {
        if (userSaveData == null)
            return;
        Debug.Log("Initializing user data");

        sessionData = new SessionData();
        sessionData.loginTime = DateTime.Now;

        if (userSaveData.sessionDataList == null)
        {
            sessionDataList = new List<SessionData>();
            Debug.Log("Session data list is null, creating new list.");
        }
        else
        {
            sessionDataList = userSaveData.sessionDataList;
            Debug.Log("Session data list size: " + sessionDataList.Count);

            sessionDataList.Add(sessionData);
        }

        sessionData = GetLastSessionData();
    }
}
