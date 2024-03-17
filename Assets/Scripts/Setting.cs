using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    public FirebaseManager firebaseManager;
    string currentScene;

    void Awake()
    {
        firebaseManager = GameObject.FindObjectOfType<FirebaseManager>();
    }
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void DisableEssentialGameObjects()
    {
        // Find and enable essential GameObjects
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            mainCamera.SetActive(false);
        }

        GameObject mainCharacter = GameObject.Find("MainCharacter");
        if (mainCharacter != null)
        {
            mainCharacter.SetActive(false);
        }

        GameObject canvasPanel = GameObject.Find("Canvas");
        if (canvasPanel != null)
        {
            canvasPanel.SetActive(false);
        }

        GameObject iconHighlight = GameObject.Find("IconHighlight");
        if (iconHighlight != null)
        {
            iconHighlight.SetActive(false);
        }

        GameObject grid = GameObject.Find("Grid");
        if(grid != null)
        {
            Transform marker = grid.transform.Find("MarkerTilemap");
            if(marker != null)
            {
                marker.gameObject.SetActive(false);
            }
        }

        GameObject gameManager = GameObject.Find("GameManager");
        if(gameManager != null)
        {
            Transform weatherManager = gameManager.transform.Find("WeatherManager");
            if (weatherManager != null)
            {
                weatherManager.gameObject.SetActive(false);
            }
        }

        GameObject menuCanvas = GameObject.Find("MenuCanvas");
        if (menuCanvas != null)
        {
            Transform mainMenu = menuCanvas.transform.Find("Main Menu");
            Transform loginMenu = menuCanvas.transform.Find("Login Menu");
            if (mainMenu != null && loginMenu != null)
            {
                mainMenu.gameObject.SetActive(true);
                loginMenu.gameObject.SetActive(false);
            }
        }
    }

    public void ExitGame()
    {
        PlayerData currentPlayerData = GameManager.Instance.playerDataManager.GetPlayerData();
        StartCoroutine(firebaseManager.UpdatePlayerData(currentPlayerData));
        Application.Quit();
    }

    public IEnumerator ExitToMainMenu()
    {
        PlayerData currentPlayerData = GameManager.Instance.playerDataManager.GetPlayerData();
        Debug.Log($"Player name: {currentPlayerData.name}");
        Debug.Log($"Player money: {currentPlayerData.money.amount}");
        StartCoroutine(firebaseManager.UpdatePlayerData(currentPlayerData));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenuScene"));

        DisableEssentialGameObjects();
    }


    public void ExitToMenu()
    {
        StartCoroutine(ExitToMainMenu());
    }

}

