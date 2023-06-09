using UnityEngine;
using Newtonsoft.Json;
using PawnControl;
using System.Collections.Generic;

public class SaveDataController
{
    public void SaveAccountData(bool loggingOut = false)
    {
        SaveDataObject currentData = new SaveDataObject();
        PawnStatusController pawnStatus = PawnManager.instance.PawnController.PawnStatusController;
        PawnSpriteController pawnSprites = PawnManager.instance.PawnSpriteController;

        List<SessionData> sessionDataList;

        if (loggingOut)
            sessionDataList = UserManager.instance.GetSessionDataList();
        else
            sessionDataList = UserManager.instance.GetSessionDataList();

        currentData.pawnStatusContainer = new PawnStatusContainer();
        currentData.pawnStatusContainer.MentalStatusContainer = new PawnMentalStatusContainer();
        currentData.pawnStatusContainer.MentalStatusContainer.statusEffects = new List<MentalStatusEffect>();
        currentData.pawnStatusContainer.PhysicalStatusContainer = new PawnPhysicalStatusContainer();
        currentData.pawnStatusContainer.PhysicalStatusContainer.statusEffects = new List<PhysicalStatusEffect>();
        currentData.pawnStatusContainer.NeedsStatusContainer =  new PawnNeedsStatusContainer();

        currentData.pawnStatusContainer.FirstName = pawnStatus.FirstName;
        currentData.pawnStatusContainer.NickName = pawnStatus.NickName;
        currentData.pawnStatusContainer.LastName = pawnStatus.LastName;
        currentData.pawnStatusContainer.HeadType = pawnSprites.HeadType;
        currentData.pawnStatusContainer.BodyType = pawnSprites.BodyType;
        currentData.completedTutorial = true;

        currentData.pawnStatusContainer.MentalStatusContainer.health = pawnStatus.MentalStatus.MentalHealth;
        currentData.pawnStatusContainer.MentalStatusContainer.statusEffects = pawnStatus.MentalStatus.MentalStatusEffects;

        currentData.pawnStatusContainer.PhysicalStatusContainer.health = pawnStatus.PhysicalStatus.PhysicalHealth;
        currentData.pawnStatusContainer.PhysicalStatusContainer.statusEffects = pawnStatus.PhysicalStatus.PhysicalStatusEffects;

        currentData.pawnStatusContainer.NeedsStatusContainer.hunger = pawnStatus.NeedsStatus.hunger;
        currentData.pawnStatusContainer.NeedsStatusContainer.sleep = pawnStatus.NeedsStatus.sleep;
        currentData.pawnStatusContainer.NeedsStatusContainer.entertainment = pawnStatus.NeedsStatus.entertainment;
        currentData.pawnStatusContainer.NeedsStatusContainer.happiness = pawnStatus.NeedsStatus.happiness;

        currentData.userName = UserManager.instance.Username;
        currentData.password = UserManager.instance.Password;
        currentData.sessionDataList = sessionDataList;
        

        string jsonPost = JsonConvert.SerializeObject(currentData, Formatting.Indented);

        WebRequestUtil.PostJson("https://thesisprojectdata.azurewebsites.net/api/PostSaveData?code=9Da2f0e7WgDrSZFucqSdlNxluqRoexOfLPmLWstfnBagAzFuVQ452A==",
        jsonPost,
        (string error) =>
        {
            Debug.Log("Error: " + error);
        },
        (string response) =>
        {
            Debug.Log("Response: " + response);
        });
    }

    public void LoadAccountData(UserPassObject userPassObject)
    {
        string jsonPost = JsonConvert.SerializeObject(userPassObject);

        WebRequestUtil.PostJson("https://thesisprojectdata.azurewebsites.net/api/GetSaveData?code=epT71UhJe0lMg_Fv-QMfDe-IEORnpUKvy20qczVU9tdFAzFuueDbmw==",
        jsonPost,
        (string error) =>
        {
            Debug.Log("Error: " + error);
        },
        (string response) =>
        {
            Debug.Log("Response: " + response);

            if (!string.IsNullOrEmpty(response))
            {
                SaveDataObject newData = JsonConvert.DeserializeObject<SaveDataObject>(response);
                SaveLoadManager.instance.UpdatePlayerData(newData);
            }
            else
                SaveLoadManager.instance.UpdatePlayerData(null);
        });
    }

    public void CreateAccountData(UserPassObject userPassObject)
    {
        string jsonPost = JsonConvert.SerializeObject(userPassObject);

        WebRequestUtil.PostJson("https://thesisprojectdata.azurewebsites.net/api/CreatePostData?code=SSxW5B436OY3CpBVVGGYCDSr40tzsCRivwXsbGdBq2XkAzFu25Q4-w==",
        jsonPost,
        (string error) =>
        {
            Debug.Log("Error: " + error);
        },
        (string response) =>
        {
            Debug.Log("Response: " + response);

            if (!string.IsNullOrEmpty(response))
            {
                SaveDataObject newData = JsonConvert.DeserializeObject<SaveDataObject>(response);
                SaveLoadManager.instance.UpdatePlayerData(newData);
            }
            else
                SaveLoadManager.instance.UpdatePlayerData(null);
        });
    }
}
