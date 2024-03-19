using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class rotateImage : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public float speed; public bool check; 

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!check)
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            switch (GameManager.instence.state)
            {
                case GameManager.State.Biding:
                    loadingText.text = persistantmanager.instence.players[GameManager.instence.playersTurn].name + Languages.instence.GetText("biddingWait");
                    break;
                case GameManager.State.Selection:
                    loadingText.text = persistantmanager.instence.players[GameManager.instence.playersTurn].name + Languages.instence.GetText("selectingWait");
                    break;
            }
        }
        else
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
