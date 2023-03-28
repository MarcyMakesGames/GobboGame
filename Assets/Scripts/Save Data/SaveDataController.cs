using UnityEngine;
using Newtonsoft.Json;
using PawnControl;
using System.Collections.Generic;

public class SaveDataController
{
    public void SaveGameData()
    {
        SaveDataObject currentData = new SaveDataObject();
        PawnStatusController pawnStatus = PawnManager.instance.Controller.PawnStatusController;
        List<SessionData> sessionDataList = UserManager.instance.GetUserSessionData();

        currentData.pawnStatusContainer = new PawnStatusContainer();
        currentData.pawnStatusContainer.MentalStatusContainer = new PawnMentalStatusContainer();
        currentData.pawnStatusContainer.MentalStatusContainer.statusEffects = new List<MentalStatusEffect>();
        currentData.pawnStatusContainer.PhysicalStatusContainer = new PawnPhysicalStatusContainer();
        currentData.pawnStatusContainer.PhysicalStatusContainer.statusEffects = new List<PhysicalStatusEffect>();
        currentData.pawnStatusContainer.NeedsStatusContainer =  new PawnNeedsStatusContainer();

        currentData.pawnStatusContainer.FirstName = pawnStatus.FirstName;
        currentData.pawnStatusContainer.NickName = pawnStatus.NickName;
        currentData.pawnStatusContainer.LastName = pawnStatus.LastName;

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
        

        string jsonPost = JsonConvert.SerializeObject(currentData);

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

    public void LoadGame(UserPassObject userPassObject)
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
            SaveDataObject newData = JsonConvert.DeserializeObject<SaveDataObject>(response);
            SaveLoadManager.instance.UpdatePlayerData(newData);
        });
    }
}
