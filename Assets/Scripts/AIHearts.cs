using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class AIHearts : MonoBehaviour
{
    public static AIHearts instence;
    private void Awake()
    {
        if (instence == null)
        {
            instence = this; 
        }
    }
    public bool actionStarted;
    public int speed; 
    
    private void Start()
    {
        actionStarted = false;
        if (persistantmanager.instence.multiplayer)
        {
            //if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Process", speed, speed);
            }
        }
        else
        {
            InvokeRepeating("Process", speed, speed);
        }
    }
    public IEnumerator ResetProcess()
    {
        yield return new WaitForSeconds(0.5f);
        actionStarted = false; 
    }
    public void Process()
    {
        //OJ_Comment
        //if (CheckifthisIsAiPlayer() && !actionStarted && Bidding.instence.gamestarted)
        //if (CheckifthisIsAiPlayer() && !actionStarted)
        {
            actionStarted = true;
            WaitForStart();
        }
    }
    public bool CheckifthisIsAiPlayer()
    {
        //OJ_COMMENT
        int count = 0;
        foreach (bool player in persistantmanager.instence.ArtificalPlayers)
        {
            if (player && count == HeartsGameManager.instence.playersTurn)
            {
                return true;
            }
            count++;
        }
        return false; 
    }
    public void WaitForStart()
    {
        Debug.Log("wait started");
        // return new WaitForSeconds(2f);
        Debug.Log("repeat");
        switch (HeartsGameManager.instence.state)
        {
            
            case HeartsGameManager.State.PLayable:
                Playing();
                break;
            case HeartsGameManager.State.Exchanging:
                Exchanging();
                break;
            case HeartsGameManager.State.Selection:
                Selecting();
                break;
        }
    }
    public void Exchanging()
    {
        if (persistantmanager.instence.ArtificalPlayers[HeartsGameManager.instence.exchangeTurn] ==true)
        {
            for (int j = 0; j < 3; j++) 
            {
                Debug.Log(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeTurn].transform.GetChild(j).gameObject.name);
                HeartsGameManager.instence.ForRpc.ExchangeCardSelected(HeartsGameManager.instence.ExchangeCardSelected, System.Array.IndexOf(CardObjects.instence.cards, HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeTurn].transform.GetChild(j).gameObject));
            }

        }
    }

    public void Selecting()
    {
        int SelectTrump = Random.Range(0, 4);
        switch (selectionTP.instence.mode)
        {
            case Bidding.GameMode.Ordinary:
                
                GameManager.instence.trump = (Deck.CardType)SelectTrump;
                selectionTP.instence.trumpSelected = SelectTrump; 
                if(SelectTrump == 3)
                {
                    GameManager.instence.partnerAce = (Deck.CardType)0;
                    selectionTP.instence.partnerAceSelected = 0;
                }
                else
                {
                    GameManager.instence.partnerAce = (Deck.CardType)(SelectTrump+1);
                    selectionTP.instence.partnerAceSelected = (SelectTrump + 1);
                }
                selectionTP.instence.Selection1Done();

                actionStarted = false;
                break;
            case Bidding.GameMode.Clubs:
                selectionTP.instence.partnerAceSelected = 0;
                selectionTP.instence.Selection1Done();

                actionStarted = false;
                break;
            case Bidding.GameMode.Halfs:
                if (GameManager.instence.selectedpartnerAce == -1)
                {
                    GameManager.instence.partnerAce = (Deck.CardType)Random.Range(0, 5);
                    selectionTP.instence.partnerAceSelected =(int) GameManager.instence.partnerAce;
                    selectionTP.instence.Selection1Done();
                }
                else
                {
                    if ((int)GameManager.instence.partnerAce == 3)
                    {
                        GameManager.instence.trump = (Deck.CardType)0;
                        selectionTP.instence.trumpSelected = 0;
                    }
                    else
                    {
                        GameManager.instence.trump = (Deck.CardType)(GameManager.instence.partnerAce + 1);
                        selectionTP.instence.trumpSelected = (int)(GameManager.instence.partnerAce + 1);
                    }

                    selectionTP.instence.Selection2Done() ;
                }

                actionStarted = false;
                break;
            case Bidding.GameMode.Sans:
                GameManager.instence.trump = (Deck.CardType)SelectTrump;
                selectionTP.instence.trumpSelected = SelectTrump;
                GameManager.instence.partnerAce = Deck.CardType.Club;
                selectionTP.instence.partnerAceSelected = (int)Deck.CardType.Club;
                selectionTP.instence.Selection1Done();

                actionStarted = false;
                break;
            case Bidding.GameMode.noTrickGame:
            case Bidding.GameMode.noTrickGameFaceUp:
            case Bidding.GameMode.TrickGame1:

                actionStarted = false;
                break;
            case Bidding.GameMode.Vip:

                GameManager.instence.trump = (Deck.CardType)SelectTrump;
                GameManager.instence.partnerAce = Deck.CardType.Club;
                selectionTP.instence.Selection2Done();
                break;
        }
    }
    public void Playing()
    {
        GameObject temp; 

        if (HeartsGameManager.instence.CheckIfPlayerhasType(HeartsGameManager.instence.playersTurn , HeartsGameManager.instence.cardtypeFortheRound))
        {
            temp = HeartsGameManager.instence.GetHighestPlayer(HeartsGameManager.instence.playersTurn, HeartsGameManager.instence.cardtypeFortheRound);
        }
        else
        {
            if(HeartsGameManager.instence.CheckIfPlayerhasType(HeartsGameManager.instence.playersTurn , HeartsGameManager.instence.trump))
            {
                temp = HeartsGameManager.instence.GetHighestPlayer(HeartsGameManager.instence.playersTurn, HeartsGameManager.instence.trump);
            }
            else
            {
                //temp = HeartsGameManager.instence.GiveLeftcardType(HeartsGameManager.instence.playersTurn);
            }
        }
       // HeartsGameManager.instence.selectedCard = temp.GetComponent<Card>();
        InputController.instence.PlayCardMultiplayerToo();
    }
    public void VIPCardsAI()
    {
        if (CheckifthisIsAiPlayer())
        {
            Invoke("SelectCard1", 8f);
        }
    }
    public void SelectCard1()
    {
        //HeartsGameManager.instence.StopSelectionVip();
        Invoke("ActionStop" , 3f);
    }
    public void ActionStop()
    {
        actionStarted = false;
    }
    public void ExchangeCardsAI()
    {
        if (CheckifthisIsAiPlayer())
        {
            Invoke("DeclineAxchange", 2f);
        }
    }
    public void DeclineAxchange()
    {
       // HeartsGameManager.instence.DontWantToExchange();
    }
}
