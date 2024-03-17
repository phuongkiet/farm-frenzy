using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour
{
    public Text usernameText; // Assign in Inspector
    public Text moneyText; // Assign in Inspector

    public void NewScoreElement(string username, int money)
    {
        usernameText.text = username;
        moneyText.text = money.ToString();
    }
}
