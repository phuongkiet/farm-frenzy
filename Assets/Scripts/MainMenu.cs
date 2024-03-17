using Firebase.Auth;
using Firebase.Database;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public FirebaseManager firebaseManager;

    [Header("Login")]
    public InputField emailInputField;
    public InputField passwordInputField;
    public Text warningLoginText;
    public Text confirmLoginText;

    [Header("Register")]
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField passwordRegisterVerifyField;
    public Text warningRegisterText;

    [Header("Scoreboard")]
    public Transform scoreboard;
    public GameObject scoreboardContent;

    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject creationPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject scoreBoardPanel;
    [SerializeField] string nameEssentialScene;
    [SerializeField] string newGameStartScene;
    [SerializeField] InputField usernameInputField;

    private bool flag = true;

    private void Start()
    {
        firebaseManager = FirebaseManager.Instance;
    }

    public void ChangeFromRegisterToLoginMenu()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void ChangeFromLoginToRegisterMenu()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void ChangeFromScoreboardToMainMenu()
    {
        scoreBoardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Login()
    {
        StartCoroutine(firebaseManager.Login(emailInputField.text, passwordInputField.text));
        flag = true;
        if(flag == true)
        {
            loginPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);
            ClearLoginField();
        }
    }

    public void Register()
    {
        StartCoroutine(firebaseManager.Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        flag = true;
        if(flag == true)
        {
            loginPanel.gameObject.SetActive(true);
            registerPanel.gameObject.SetActive(false);
            ClearRegisterField();
        }
    }

    public void LoadScoreBoard()
    {
        mainMenuPanel.SetActive(false);
        scoreBoardPanel.SetActive(true);
        foreach (Transform child in scoreboard.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(firebaseManager.LoadScoreBoard(scoreboard, scoreboardContent));

    }

    public void SaveData()
    {
        StartCoroutine(firebaseManager.UpdateUsernameAuth(usernameInputField.text));
        StartCoroutine(firebaseManager.UpdateUsernameDatabase(usernameInputField.text));
        firebaseManager.SaveDataButton();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Logout()
    {
        firebaseManager.SignOutButton();
        ClearLoginField();
        mainMenuPanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        creationPanel.SetActive(true);
    }
    

    private void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        creationPanel.SetActive(false);
    }

    private void EnableEssentialGameObjects()
    {
        // Find and enable essential GameObjects
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            mainCamera.SetActive(true);
        }

        GameObject mainCharacter = GameObject.Find("Main Character");
        if (mainCharacter != null)
        {
            mainCharacter.SetActive(true);
        }

        GameObject canvasPanel = GameObject.Find("Canvas");
        if (canvasPanel != null)
        {
            canvasPanel.SetActive(true);
        }

        GameObject iconHighlight = GameObject.Find("IconHighlight");
        if (iconHighlight != null)
        {
            iconHighlight.SetActive(true);
        }
    }

    private void ClearLoginField()
    {
        emailInputField.text = "";
        passwordInputField.text = "";
    }

    private void ClearRegisterField()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }
}
