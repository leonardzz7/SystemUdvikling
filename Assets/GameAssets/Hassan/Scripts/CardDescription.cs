using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDescription : MonoBehaviour
{
    [SerializeField] Image frontImage;
    [SerializeField] GameObject backCard;
    public string rank;
    public int player;

    public bool selected;


    public Deck.CardType cardType;

    public void SetCardDescription(Deck.CardType suitval, string rankval)
    {
        cardType = suitval;
        rank = rankval;

    }

    public void SetFrontImage(Sprite val)
    {
        frontImage.sprite = val;
    }

    public Deck.CardType GetSuit()
    {
        return cardType;
    }

    public string GetRank()
    {
        return rank;
    }

    public void ShowCard()
    {
        backCard.SetActive(false);
    }

    public void HideCard()
    {
        backCard.SetActive(true);
    }


    public void TurnOffCard()
    {
        this.gameObject.GetComponent<Button>().interactable = false;
    }

    public void TurnOnCard()
    {
        this.gameObject.GetComponent<Button>().interactable = true;
    }

    public void SelectCard()
    {
        if (player == 0)
        {


            if (!HeartGameManager.Instance.selection)
            {
                if (!DeckSpawner.Instance.GetHeartPlayers()[player].selectedCards.Contains(this) || DeckSpawner.Instance.GetHeartPlayers()[player].selectedCards.Count==0)
                {
                    DeckSpawner.Instance.GetHeartPlayers()[player].selectedCards.Add(this);
                    DeckSpawner.Instance.totalSelected++;
                }
            
               

                selected = true;
                if (DeckSpawner.Instance.totalSelected == 3)
                {
                    DeckSpawner.Instance.totalSelected = 0;
                    DeckSpawner.Instance.SetGameDesciptionText("");

                    HeartGameManager.cardsSelectedAction?.Invoke();
                    HeartGameManager.Instance.selection = true;
                }
            }
            else
            {

                DeckSpawner.Instance.SetPlayerTurnCard(this);

            }


        }




    }



    public int GetCardValue()
    {

        switch (rank)
        {
            case "2":
                return 1;
            case "3":
                return 2;
            case "4":
                return 3;
            case "5":
                return 4;
            case "6":
                return 5;
            case "7":
                return 6;
            case "8":
                return 7;
            case "9":
                return 8;
            case "10":
                return 9;
            case "Jack":
                return 10;
            case "Queen":
                return 11;
            case "King":
                return 12;
            case "Ace":
                return 13;
            default:
                return 0;
        }
    }



    public void PlayShuffleAnimation()
    {
        this.gameObject.GetComponent<Animator>().Play("CardShuffleAnimation");
    }



}
