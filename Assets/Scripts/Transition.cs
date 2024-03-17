using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum TransitionType
{
    Warp,
    Scene
}
public class Transition : MonoBehaviour
{
    [SerializeField] TransitionType type;
    [SerializeField] string sceneNameToTransition;
    [SerializeField] Vector3 targettPosition;
    [SerializeField] Collider2D confiner;
    Transform destination;
    CameraConfiner cameraConfiner;

    internal void InitiateTransition(Transform toTransition)
    {   
        switch (type)
        {
            case TransitionType.Warp:
                Cinemachine.CinemachineBrain currentCamera = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
                if(cameraConfiner != null)
                {
                    cameraConfiner.UpdateBounds(confiner);
                }
                currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(toTransition, destination.position - toTransition.position);
                toTransition.position = new Vector3(destination.position.x, destination.position.y, toTransition.position.z); 
                break;
            case TransitionType.Scene:
                if (cameraConfiner != null)
                {
                    cameraConfiner.UpdateBounds(confiner);
                }
                GameSceneManager.instance.InitSwitchScene(sceneNameToTransition, targettPosition);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(confiner != null)
        {
            cameraConfiner = FindObjectOfType<CameraConfiner>();
        }
        destination= transform.GetChild(1);
    }
}
