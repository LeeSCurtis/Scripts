using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Photon.Pun;

public class HealthManager : MonoBehaviour
{
    //4 variables for each player's health
    public float player1Health = 100;
    public float player2Health = 100;
    public float player3Health = 100;
    public float player4Health = 100;
    
    //Reference the TMP_Text component for each player
    public TMPro.TMP_Text player1HealthText;
    public TMPro.TMP_Text player2HealthText;
    public TMPro.TMP_Text player3HealthText;
    public TMPro.TMP_Text player4HealthText;

    void Start()
    {
        //Update the health text for each player
        UpdateHealthTextRPC();
    }

    void Update()
    {
        
    }
    //A method to reduce the health of a player
    [PunRPC]
    public void UpdateHealthTextRPC(){
        //Update the TMP_Text component for each player
        player1HealthText.text = player1Health.ToString();
        player2HealthText.text = player2Health.ToString();
        player3HealthText.text = player3Health.ToString();
        player4HealthText.text = player4Health.ToString();
    }
}
