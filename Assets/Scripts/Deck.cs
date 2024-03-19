using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck 
{
    public enum CardType{
        Club ,
        Diamond ,
        Heart ,
        Spade , 
        Joker , 
        NotSelected , 
        Sans
    } 
    public Card[] deck;
    public Vector3 tempPosition; 
    public Deck(int tNoCards)
    {
        int count = 0 ;
        deck = new Card[tNoCards];
        tempPosition = new Vector3(0, 0, 80);
        for(int x = 0; x < 13; x++)
        {
            count++;
            tempPosition = new Vector3(tempPosition.x, tempPosition.y, tempPosition.z + 0.1f);
            deck[count] = CardObjects.instence.cards[count - 1].GetComponent<Card>();
            deck[count].FillCard(tempPosition, CardType.Club, x , CardObjects.instence.cards[count - 1]);

            count++;
            tempPosition = new Vector3(tempPosition.x, tempPosition.y, tempPosition.z + 0.1f);
            deck[count] = CardObjects.instence.cards[count - 1].GetComponent<Card>();
            deck[count].FillCard(tempPosition, CardType.Diamond, x, CardObjects.instence.cards[count - 1]);

            count++;
            tempPosition = new Vector3(tempPosition.x, tempPosition.y, tempPosition.z + 0.1f);
            deck[count] = CardObjects.instence.cards[count - 1].GetComponent<Card>();
            deck[count].FillCard(tempPosition, CardType.Heart, x, CardObjects.instence.cards[count - 1]);

            count++;
            tempPosition = new Vector3(tempPosition.x, tempPosition.y, tempPosition.z + 0.1f);
            deck[count] = CardObjects.instence.cards[count - 1].GetComponent<Card>();
            deck[count].FillCard(tempPosition, CardType.Spade, x, CardObjects.instence.cards[count - 1]);
        }
    }
}
