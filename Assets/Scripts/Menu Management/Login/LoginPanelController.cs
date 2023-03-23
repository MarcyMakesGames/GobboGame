using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginPanelController : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text statusText;

    [SerializeField, TextArea(1, 5)] private string loginFailedText;

    public void TryLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        UserPassObject userPassObject = new UserPassObject();
        userPassObject.username = username;
        userPassObject.password = password;

        SaveLoadManager.instance.LoadGameData(userPassObject);
    }

    private void Start()
    {
        SaveLoadManager.OnPlayerDataUpdated += UpdateLoginPage;
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
            loginPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }
}
