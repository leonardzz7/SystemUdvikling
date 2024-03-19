using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObjects : MonoBehaviour
{
    public static CardObjects instence; 

    public GameObject[] cards;
    public void Awake()
    {
        if(instence == null)
        {
            instence = this;
        }
    }
}
