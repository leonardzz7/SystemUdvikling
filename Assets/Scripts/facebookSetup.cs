//using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class facebookSetup : MonoBehaviour
{
    public static facebookSetup instence;
    public TextMeshProUGUI id, username, email ;
    void Awake()
    {
        if (instence == null)
        {
            instence = this;
        }
    }

    public void UpdateStats()
    {
        id.text = Player1.instance.id;
        username.text = Player1.instance.user_name;
        email.text = Player1.instance.email;
    }
}
