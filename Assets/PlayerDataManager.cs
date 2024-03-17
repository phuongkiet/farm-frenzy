using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public PlayerData PlayerData;

    public PlayerData GetPlayerData()
    {
        return PlayerData;
    }
    public void UpdatePlayerData(PlayerData newData)
    {
        PlayerData = newData;
    }
}
