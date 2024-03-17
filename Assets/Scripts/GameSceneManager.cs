using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] ScreenTint screenTint;
    [SerializeField] CameraConfiner confiner;
    string currentScene;
    AsyncOperation unload;
    AsyncOperation load;

    bool respawn;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void InitSwitchScene(string to, Vector3 targettPosition)
    {
        StartCoroutine(Transition(to, targettPosition));
    }
    IEnumerator Transition(string to, Vector3 targettPosition)
    {
        screenTint.Tint();
        yield return new WaitForSeconds(1f / screenTint.speed + 0.1f);
        SwitchScene(to, targettPosition);
        while(load != null && unload != null)
        {
            if (load.isDone) { load = null; }
            if (unload.isDone) { unload = null; }
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        confiner.UpdateBounds();
        screenTint.UnTint();
    }

    public void SwitchScene(string to, Vector3 targettPosition)
    {
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unload = SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;
        MoveCharacter(targettPosition);

    }

    private void MoveCharacter(Vector3 targettPosition)
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        Cinemachine.CinemachineBrain currentCamera = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(playerTransform, targettPosition - playerTransform.position);
        playerTransform.position = new Vector3(targettPosition.x, targettPosition.y, playerTransform.position.z);
        if (respawn)
        {
            playerTransform.GetComponent<Character>().FullRest();
            playerTransform.GetComponent<DisableControls>().EnableControl();
            respawn = false;
        }
    }

    internal IEnumerator Respawn(Vector3 respawnPosition, string respawnSceneName)
    {
        respawn = true;
        if(currentScene != respawnSceneName)
        {
            InitSwitchScene(respawnSceneName, respawnPosition);
        }
        else
        {
            GameManager.Instance.screenTint.Tint();
            yield return new WaitForSeconds(2);
            MoveCharacter(respawnPosition);
            GameManager.Instance.screenTint.UnTint();
        }
    }
}
