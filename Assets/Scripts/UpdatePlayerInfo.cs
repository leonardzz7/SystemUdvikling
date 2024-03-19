using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class UpdatePlayerInfo : MonoBehaviour
{
    public static UpdatePlayerInfo instence;
    private void Awake()
    {
        if(instence == null)
        {
            instence = this; 
        }
    }
    public TextMeshProUGUI[] names;
    public GameObject[] characterParents;  
    private void Start()
    { 
        names[0].text = "Player";
        names[1].text = "Player";
        names[2].text = "Player";
        names[3].text = "Player";
    }
    public void UpdatePlayerName()
    {
        if (persistantmanager.instence.multiplayer)
        {
            names[0].text = ""+persistantmanager.instence.players[0].name;
            names[1].text = ""+persistantmanager.instence.players[1].name;
            names[2].text = ""+persistantmanager.instence.players[2].name;
            names[3].text = ""+persistantmanager.instence.players[3].name;
        }
        else
        {
            names[0].text = "Player1";
            names[1].text = "Player2";
            names[2].text = "Player3";
            names[3].text = "Player4";
        }
    }
}
