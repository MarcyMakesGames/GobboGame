using PawnControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[System.Serializable]
public class SaveDataObject
{
    public string userName;
    public string password;
    public PawnStatusContainer pawnStatusContainer;
    public List<SessionData> sessionDataList;
}

public class SessionData
{
    //Session interactions
    public int timesPet;
    public int timesFed;
    public int timesHealed;
    public int timesPlayed;
    public int timesGivenItems;

    //Session times
    public DateTime loginTime;
    public DateTime logoutTime;
}