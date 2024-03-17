using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] Vector3 respawnPosition;
    [SerializeField] string respawnSceneName;

    internal void StartRespawn()
    {
        StartCoroutine(GameSceneManager.instance.Respawn(respawnPosition, respawnSceneName));
    }
}
