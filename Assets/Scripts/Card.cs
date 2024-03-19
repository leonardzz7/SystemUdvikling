using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Deck.CardType type;
    public int cardNumber;
    public GameObject cardObject;
    public bool selected;
    public int playedBy;
    public int positionParent;
    public int positionChlid;
    public int rank;
    public void FillCard(Vector3 pos, Deck.CardType typ, int no, GameObject obj)
    {
        type = typ;
        cardNumber = no;
        cardObject = obj;
        cardObject.transform.position = pos;
        
    }
}
