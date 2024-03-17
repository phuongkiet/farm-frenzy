using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
/*using UnityEngine.SocialPlatforms.Impl;
*/using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class FirebaseManager : MonoBehaviour
{
    //FireBase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference databaseReference;
    public static FirebaseManager Instance;

    public Transform scoreBoardContent;
    public GameObject scoreElement;

    [SerializeField] GameObject LoginPanel;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject CreationPanel;
    [SerializeField] GameObject ScoreboardPanel;
    [SerializeField] string nameEssentialScene;
    [SerializeField] string newGameStartScene;
    [SerializeField] InputField username;

    public PlayerData playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.Log("Could not resolve all FireBase dependencies: " + dependencyStatus);
                }
            });
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        DisableEssentialGameObjects();
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

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /*public void ClearLoginField()
    {
        email.text = "";
        password.text = "";
    }*/

    /*public void LoginButton()
    {
        StartCoroutine(Login(email.text, password.text));
    }*/

    public IEnumerator Login(string email, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Login Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Passowrd";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User Not Found";
                    break;
            }
            /*warningLogin.text = message;*/
        }
        else
        {
            user = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            /*warningLogin.text = "";
            warningLogin.gameObject.SetActive(true);
            warningLogin.text = "Logged In";*/
            StartCoroutine(LoadUserData());
            yield return new WaitForSeconds(3);
            /*username.text = user.DisplayName;
            warningLogin.gameObject.SetActive(false);
            warningLogin.text = "";
            LoginPanel.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);*/
        }
    }

    public IEnumerator Register(string _email, string _password, string _username)
    {

        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Register Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
            }
            /*warningRegisterText.text = message;*/
        }
        else
        {
            //User has now been created
            //Now get the result
            user = RegisterTask.Result.User;

            if (user != null)
            {
                //Create a user profile and set the username
                UserProfile profile = new UserProfile { DisplayName = _username };

                //Call the Firebase auth update user profile function passing the profile with the username
                Task ProfileTask = user.UpdateUserProfileAsync(profile);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    /*warningRegisterText.text = "Username Set Failed!";*/
                }
            }
        }
    }

    public void SignOutButton()
    {
        auth.SignOut();
        /*MainMenu.gameObject.SetActive(false);
        ClearLoginField();
        LoginPanel.gameObject.SetActive(true);*/
    }

    public void SaveDataButton()
    {
        StartCoroutine(LoadUserData());
        EnableEssentialGameObjects();
        SceneManager.LoadScene(newGameStartScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }

    public IEnumerator UpdateUsernameAuth(string username)
    {
        UserProfile profile = new UserProfile
        {
            DisplayName = username,
        };

        var ProfileTask = user.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {

        }
    }

    public IEnumerator UpdateUsernameDatabase(string username)
    {
        var dbTask = databaseReference.Child("users").Child(user.UserId).Child("username").SetValueAsync(username);
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
        }
        else
        {

        }
    }

    public IEnumerator UpdatePlayerData(PlayerData newData)
    {
        // Convert player data to JSON
        string jsonData = JsonUtility.ToJson(newData);

        // Set the player data in Firebase
        var dbTask = databaseReference.Child("users").Child(user.UserId).SetRawJsonValueAsync(jsonData);

        yield return new WaitUntil(() => dbTask.IsCompleted);

        if (dbTask.IsFaulted)
        {
            Debug.LogError($"Failed to update player data: {dbTask.Exception}");
        }
        else if (dbTask.IsCanceled)
        {
            Debug.LogWarning($"Update player data task was canceled.");
        }
        else
        {
            Debug.Log("Player data updated successfully");
        }
    }

    public IEnumerator LoadUserData()
    {
        var dbTask = databaseReference.Child("users").Child(user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        if (dbTask.Exception != null)
        {
            Debug.LogWarning($"Failed to update player data: {dbTask.Exception}");
        }
        else if (dbTask.Result.Value == null)
        {
            PlayerData newData = new PlayerData
            {
                name = user.DisplayName,
                money = new Currency(500),
            };

            GameManager.Instance.playerDataManager.PlayerData = newData;
            GameManager.Instance.currencyReferenceManager.UpdateCurrencyAmount(newData.money.amount);
            StartCoroutine(UpdatePlayerData(newData));
        }
        else
        {
            CurrencyManager.Instance.isUserDataLoaded = true;
            DataSnapshot snapshot = dbTask.Result;
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(snapshot.GetRawJsonValue());
            Debug.Log("Player name: " + playerData.name);
            Debug.Log("Player money: " + playerData.money.amount);
            CurrencyManager.Instance.UpdateCurrencyAmount(playerData.money.amount);
            GameManager.Instance.playerDataManager.PlayerData = playerData;
            GameManager.Instance.currencyReferenceManager.UpdateCurrencyAmount(playerData.money.amount);
            CurrencyManager.Instance.currency = new Currency(playerData.money.amount);
        }
    }

    public void ScoreBoardLoad()
    {
        MainMenu.SetActive(false);
        ScoreboardPanel.SetActive(true);
        StartCoroutine(LoadScoreBoard(scoreBoardContent, scoreElement));
    }

    public IEnumerator LoadScoreBoard(Transform scoreBoardContent, GameObject scoreElement)
    {
        var dbTask = databaseReference.Child("users").OrderByChild("money").GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if(dbTask.Exception != null)
        {
            Debug.LogWarning($"Failed to update player data: {dbTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = dbTask.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("name").Value.ToString();
                int money = int.Parse(childSnapshot.Child("money").Child("amount").Value.ToString());
                GameObject go = Instantiate(scoreElement, scoreBoardContent);
                go.GetComponent<ScoreElement>().NewScoreElement(username, money);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        MainMenu.gameObject.SetActive(false);
        CreationPanel.gameObject.SetActive(true);
    }
}
