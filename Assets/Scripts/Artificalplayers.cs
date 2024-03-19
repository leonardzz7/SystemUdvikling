using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class Artificalplayers : MonoBehaviour
{
    public static Artificalplayers instence;
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
            if (PhotonNetwork.IsMasterClient)
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
        if (CheckifthisIsAiPlayer() && !actionStarted && Bidding.instence.gamestarted)
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
            if (player && count == GameManager.instence.playersTurn)
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
        switch (GameManager.instence.state)
        {
            case GameManager.State.Biding:
                bidding();
                break;
            case GameManager.State.PLayable:
                Playing();
                break;
            case GameManager.State.Selection:
                Selecting();
                break;
        }
    }
    public void bidding()
    {
        if (Bidding.instence.highestBidderDeclared)
        {
            if (Bidding.instence.bids[Bidding.instence.highestCount].rank < 95)
            {
                if (Bidding.instence.bids[Bidding.instence.highestCount].mode == Bidding.GameMode.TrickGame1)
                {
                    Bidding.instence.AutoBid(0, 5);
                }
                else
                {
                    Bidding.instence.AutoBid(Bidding.instence.bids[Bidding.instence.highestCount].number +1 , (int)Bidding.instence.bids[Bidding.instence.highestCount].mode);
                }
            }
            else
            {
                Bidding.instence.Pass(true);
                Debug.Log("AI passed");
            }
        }
        else
        {
            Bidding.instence.AutoBid( 8 , 0 );
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

        if (GameManager.instence.CheckIfPlayerhasType(GameManager.instence.playersTurn , GameManager.instence.cardtypeFortheRound))
        {
            temp = GameManager.instence.GetHighestPlayer(GameManager.instence.playersTurn, GameManager.instence.cardtypeFortheRound);
        }
        else
        {
            if(GameManager.instence.CheckIfPlayerhasType(GameManager.instence.playersTurn , GameManager.instence.trump))
            {
                temp = GameManager.instence.GetHighestPlayer(GameManager.instence.playersTurn, GameManager.instence.trump);
            }
            else
            {
                temp = GameManager.instence.GiveLeftcardType(GameManager.instence.playersTurn);
            }
        }
        GameManager.instence.selectedCard = temp.GetComponent<Card>();
        InOuts.instence.PlayCardMultiplayerToo();
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
        GameManager.instence.StopSelectionVip();
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
        GameManager.instence.DontWantToExchange();
    }
}
