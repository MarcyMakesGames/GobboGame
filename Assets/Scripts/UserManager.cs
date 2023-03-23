using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    [SerializeField] private string username;
    [SerializeField] private SessionData sessionData;
    [SerializeField] private List<SessionData> sessionDataList;

    public string Username { get => username; set => username = value; }
    public string Password { get; private set; }

    public void InitializeUserLogin(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public List<SessionData> GetUserSessionData()
    {
        //Get activity data;
        sessionData.logoutTime = DateTime.Now;
        Debug.Log("Session Logout: " + DateTime.Now);
        sessionDataList.Add(sessionData);

        return sessionDataList;
    }

    public SessionData GetLastSessionData()
    {
        if(sessionDataList.Count == 0)
            return null;
        else
            return sessionDataList[sessionDataList.Count - 1];
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

        sessionData = new SessionData();
        sessionData.loginTime = DateTime.Now;

        if (userSaveData.sessionDataList == null)
            sessionDataList = new List<SessionData>();
        else
            sessionDataList = userSaveData.sessionDataList;
    }
}
