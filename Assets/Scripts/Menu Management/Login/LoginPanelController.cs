using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Toggle rememberCredentialsToggle;

    [SerializeField, TextArea(1, 5)] private string loginFailedText;

    public void TryLogin()
    {
        CheckSetCredentials();
        SaveLoadManager.instance.LoadAccountData(CreateUserPassObject());
    }

    public void TryCreateAccount()
    {
        SaveLoadManager.instance.CreateAccountData(CreateUserPassObject());
    }

    private void Start()
    {
        SaveLoadManager.OnPlayerDataUpdated += UpdateLoginPage;
        CheckLoadCredentials();
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnPlayerDataUpdated -= UpdateLoginPage;
    }

    private void UpdateLoginPage(SaveDataObject saveData)
    {
        if(saveData == null)
        {
            statusText.text = loginFailedText;
            usernameInput.text = "";
            passwordInput.text = "";
        }
        else
        {
            UserManager.instance.InitializeUserLogin(usernameInput.text, passwordInput.text);
            UserManager.instance.InitializeUserData(saveData);

            if (saveData.completedTutorial == false)
            {
                Debug.Log("Tutorial not completed.");
                tutorialPanel.SetActive(true);
                loginPanel.SetActive(false);
                return;
            }

            UserManager.instance.SpawnPawn();
            gamePanel.SetActive(true);
            loginPanel.SetActive(false);
        }
    }

    private UserPassObject CreateUserPassObject()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        UserPassObject userPassObject = new UserPassObject();
        userPassObject.username = username;
        userPassObject.password = password;

        return userPassObject;
    }

    private void CheckSetCredentials()
    {
        if (rememberCredentialsToggle.isOn)
        {
            PlayerPrefs.SetString("username", usernameInput.text);
            PlayerPrefs.SetString("password", passwordInput.text);
            PlayerPrefs.SetInt("rememberCredentials", 1);
        }
        else
        {
            PlayerPrefs.SetString("username", "");
            PlayerPrefs.SetString("password", "");
            PlayerPrefs.SetInt("rememberCredentials", 0);
        }
    }

    private void CheckLoadCredentials()
    {
        if(PlayerPrefs.GetInt("rememberCredentials") == 1)
        {
            rememberCredentialsToggle.isOn = true;
            usernameInput.text = PlayerPrefs.GetString("username");
            passwordInput.text = PlayerPrefs.GetString("password");
        }
        else
        {
            rememberCredentialsToggle.isOn = false;
            usernameInput.text = PlayerPrefs.GetString("");
            passwordInput.text = PlayerPrefs.GetString("");
        }
    }
}
