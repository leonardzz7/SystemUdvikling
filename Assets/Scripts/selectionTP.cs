using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class selectionTP : MonoBehaviour
{
    public static selectionTP instence;
    private void Awake()
    {
        if (instence == null)
        {
            instence = this;
        }
    }

    public Image[] buttonsTrump;
    public Image[] buttonsPartnerAce;

    public int trumpSelected;
    public int partnerAceSelected;

    public Sprite sprON;
    public Sprite sprOff;

    public GameObject trumpSelection;
    public GameObject partnerAceSelection;

    public GameObject buttonSelection1;
    public GameObject buttonSelection2;

    public Bidding.GameMode mode;

    int tempPlayer;
    public int VipCount;

    public void OnStart(Bidding.GameMode mode)
    {
        GameManager.instence.state = GameManager.State.Selection; 
        SelectTrump(0);
        SelectPartnerAce(0);
        buttonSelection1.SetActive(true);
        buttonSelection2.SetActive(false);
        VipCount = 0; 
        this.mode = mode;
        Debug.Log(mode);
        switch (mode)
        {
            case Bidding.GameMode.Ordinary:
                trumpSelection.SetActive(true);
                partnerAceSelection.SetActive(true);
                break;
            case Bidding.GameMode.Clubs:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(true);
                break;
            case Bidding.GameMode.Halfs:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(true);
                break;
            case Bidding.GameMode.Sans:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(true);
                break;
            case Bidding.GameMode.TrickGame1:
            case Bidding.GameMode.noTrickGame:
            case Bidding.GameMode.noTrickGameFaceUp:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(false);
                showWhoAreNoBidders();
                Selection1Done();
                break;
            case Bidding.GameMode.Vip:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(true);
                //checkPartnerAce(partnerAce.value);
                Selection1Done();
                break;
        }
        Bidding.instence.MultiPlayerIni(GameManager.instence.playersTurn, 2);
    }
    public void showWhoAreNoBidders()
    {
        foreach(int player in Bidding.instence.noTrickPlayers)
        {
            if(player!= -1)
            GameManager.instence.playersCanvas[player].transform.GetChild(4).gameObject.SetActive(true);
        }
    }
    public void Selection1Done()
    {
        if (persistantmanager.instence.multiplayer)
        {
            GameManager.instence.ForRpc.Selection1Process(partnerAceSelected, trumpSelected);
        }
        else
        {
            Selection1Process(partnerAceSelected, trumpSelected);
        }
    }
    public void Selection1Process(int partnerAce , int trump)
    {
        SelectTrump(trump);
        SelectPartnerAce(partnerAce);
        switch (mode)
        {
            case Bidding.GameMode.Ordinary:
                if (trump != partnerAce)
                {
                    trumpSelection.SetActive(false);
                    partnerAceSelection.SetActive(false);
                    GameManager.instence.trump = (Deck.CardType)trump;
                    checkPartnerAce(partnerAce);
                    if (GameManager.instence.selectedpartnerAce == GameManager.instence.playersTurn)
                    {
                        Debug.Log("same partner");
                        GameManager.instence.selectedpartnerAce = -1;
                        GameManager.instence.partnerWithSelf = true;
                    }
                    DisplayPartnerTrump();
                    Selection2Done();
                }
                else
                {
                    persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("cannotBeSame"), null, 0);
                }
                break;
            case Bidding.GameMode.Clubs:
                if (trump != partnerAce)
                {
                    trumpSelection.SetActive(false);
                    partnerAceSelection.SetActive(false);
                    GameManager.instence.trump = Deck.CardType.Club;
                    checkPartnerAce(partnerAce);
                    if (GameManager.instence.selectedpartnerAce == GameManager.instence.playersTurn)
                    {
                        Debug.Log("same partner");
                        GameManager.instence.selectedpartnerAce = -1;
                        GameManager.instence.partnerWithSelf = true;
                    }
                    DisplayPartnerTrump();
                    Selection2Done();
                }
                else
                {
                    persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("cannotBeSame"), null, 0);
                }
                break;
            case Bidding.GameMode.Halfs:
                trumpSelection.SetActive(true);
                partnerAceSelection.SetActive(false);
                buttonSelection1.SetActive(false);
                buttonSelection2.SetActive(true);
                checkPartnerAce(partnerAce);
                
                if (GameManager.instence.selectedpartnerAce != -1  )
                {
                    GameManager.instence.partnerRevealed = true;
                    tempPlayer = GameManager.instence.playersTurn;
                    GameManager.instence.playersTurn = (int)GameManager.instence.selectedpartnerAce;
                    GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
                    Bidding.instence.MultiPlayerIni(GameManager.instence.playersTurn, 2);
                    DisplayPartnerHalfs();
                }
                else
                {
                    Bidding.instence.MultiPlayerIni(GameManager.instence.playersTurn, 2);
                    //----------------------------------------------------------------------------------------to do if same partner
                }
                break;
            case Bidding.GameMode.Sans:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(false);
                checkPartnerAce(partnerAce);
                if (GameManager.instence.selectedpartnerAce == GameManager.instence.playersTurn)
                {
                    Debug.Log("same partner");
                    GameManager.instence.selectedpartnerAce = -1;
                    GameManager.instence.partnerWithSelf = true;
                }
                DisplayPartnerTrump();
                Selection2Done();
                break;
            case Bidding.GameMode.TrickGame1:
            case Bidding.GameMode.noTrickGame:
            case Bidding.GameMode.noTrickGameFaceUp:
                Selection2Done();
                GameManager.instence.selectedpartnerAce = -1;
                break;
            case Bidding.GameMode.Vip:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(true);
                buttonSelection1.SetActive(false);
                buttonSelection2.SetActive(true);
                break;
        }
        partnerAceSelected = (int)partnerAce;
        trumpSelected = (int)trump;
        GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);

    }
    public void Selection2Done()
    {
        if (persistantmanager.instence.multiplayer)
        {
            GameManager.instence.ForRpc.Selection2Process(partnerAceSelected, trumpSelected);
        }
        else
        {
            Selection2Process(partnerAceSelected, trumpSelected);
        }
    }
    public void Selection2Process(int partnerAce, int trump)
    {
        SelectTrump(trump);
        SelectPartnerAce(partnerAce);
        switch (mode)
        {
            case Bidding.GameMode.Ordinary:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(false);
                Bidding.instence.notrickGameCounter = 0;
                TurnExchangePanelOn();
                GameManager.instence.state = GameManager.State.Waititng;
                break;
            case Bidding.GameMode.Clubs:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(false);
                Bidding.instence.notrickGameCounter = 0;

                TurnExchangePanelOn();
                GameManager.instence.state = GameManager.State.Waititng;
                break;
            case Bidding.GameMode.Halfs:
                if (trump != partnerAce)
                {
                    trumpSelection.SetActive(false);
                    partnerAceSelection.SetActive(false);
                    GameManager.instence.trump = (Deck.CardType)trump;
                    GameManager.instence.playersTurn = tempPlayer;
                    GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
                    DisplayTtrump();
                    //GameManager.instence.state = GameManager.State.PLayable;
                    TurnExchangePanelOn();
                    Bidding.instence.notrickGameCounter = 0;
                    Bidding.instence.switchPanels(4);
                }
                else
                {
                    persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("cannotBeSame"), null, 0);
                }
                GameManager.instence.state = GameManager.State.Waititng;
                break;
            case Bidding.GameMode.Sans:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(false);
                TurnExchangePanelOn();
                GameManager.instence.trump = Deck.CardType.NotSelected;
                Bidding.instence.notrickGameCounter = 0;
                GameManager.instence.state = GameManager.State.Waititng;
                break;
            case Bidding.GameMode.TrickGame1:
            case Bidding.GameMode.noTrickGame:
            case Bidding.GameMode.noTrickGameFaceUp:
                GameManager.instence.intrationNotrickExchange = 0;
                GameManager.instence.playersTurn = Bidding.instence.noTrickPlayers[0];
                GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
                TurnExchangePanelOn();
                GiveAceLowestNumber();
                GameManager.instence.state = GameManager.State.Waititng;
                break;
            case Bidding.GameMode.Vip:
                trumpSelection.SetActive(false);
                partnerAceSelection.SetActive(false);
                checkPartnerAce(partnerAce);
                if (GameManager.instence.selectedpartnerAce == GameManager.instence.playersTurn)
                {
                    Debug.Log("same partner");
                    GameManager.instence.selectedpartnerAce = -1;
                    GameManager.instence.partnerWithSelf = true;
                }
                DisplayPartner();
                Bidding.instence.notrickGameCounter = 0;
                processForVip();
                break;
        }
    }
    public void processForVip()
    {
        DisplayVIPstart();
        GameManager.instence.sepereteDeck.transform.LeanMove(new Vector3(0, 0, 100), 0.2f);
        GameManager.instence.SelectionVip(VipCount, true);
        if (persistantmanager.instence.multiplayer)
        {
            Bidding.instence.MultiPlayerIniEmpaty(GameManager.instence.playersTurn,3);
        }
        else
        {
            transform.parent.GetComponent<Bidding>().switchPanels(3);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            Artificalplayers.instence.VIPCardsAI();
        }
    }
    public void GiveAceLowestNumber()
    {
        CardObjects.instence.cards[0].GetComponent<Card>().cardNumber = 0;
        CardObjects.instence.cards[1].GetComponent<Card>().cardNumber = 0;
        CardObjects.instence.cards[2].GetComponent<Card>().cardNumber = 0;
        CardObjects.instence.cards[3].GetComponent<Card>().cardNumber = 0;
        GameManager.instence.SortCards();
    }
    public void checkPartnerAce(int value)
    {
        GameManager.instence.partnerAce = (Deck.CardType)value;
        GameManager.instence.selectedpartnerAce = GameManager.instence.CheckIfPlayerhasAce(GameManager.instence.partnerAce);
        Debug.Log(GameManager.instence.selectedpartnerAce);
    }
    public void selectionOfVipNext()
    {
        if (persistantmanager.instence.multiplayer)
        {
            GameManager.instence.ForRpc.ProcessForSelectionVip();
        }
        else
        {
            processForSelectionVip();
        }
    }
    public void processForSelectionVip()
    {
       
        if (VipCount< 1)
        {
            VipCount++;
            GameManager.instence.SelectionVip(VipCount, true);
        }
        else
        {
            GameManager.instence.SelectionVip(VipCount, true );
            
            GameManager.instence.UnselectVip();
            //GameManager.instence.sepereteDeck.transform.LeanMove(GameManager.instence.sepereteDeckPosition, 0.01f);
            Invoke("DisplayVipDone" , 1);
        }
    }
    public void DisplayVipDone()
    {
        DiplayVipDone(VipCount, GameManager.instence.trump);
        GameManager.instence.ReadyToexchangeDeckCard();
    }
    public void TurnExchangePanelOn()
    {
        if (persistantmanager.instence.multiplayer)
        {
            if (persistantmanager.instence.pNoNow == GameManager.instence.playersTurn)
            {
                GameManager.instence.deckPanel.SetActive(true);
            }
            else
            {
                GameManager.instence.deckPanel.SetActive(false);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                Artificalplayers.instence.ExchangeCardsAI();
            }
        }
        else
        {
            GameManager.instence.deckPanel.SetActive(true);
            Artificalplayers.instence.ExchangeCardsAI();
        }
        
    }

    //----------------------------------------pop ups -------------------------------------------------------------------------

    public void DisplayPartner()
    {
        persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[Bidding.instence.highestCount].name + Languages.instence.GetText("chose1") + Languages.instence.GetText(GameManager.instence.partnerAce.ToString()) + Languages.instence.GetText("aceAs"), null, 0);
    }
    public void DisplayTtrump()
    {
        persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[Bidding.instence.highestCount].name + Languages.instence.GetText("chose1") + Languages.instence.GetText(GameManager.instence.trump.ToString()) + Languages.instence.GetText("trumpSuit"), null, 0);
    }
    public void DisplayPartnerTrump()
    {
        persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[Bidding.instence.highestCount].name + Languages.instence.GetText("chose1") +Languages.instence.GetText(GameManager.instence.partnerAce.ToString()) + Languages.instence.GetText("aceAs") + "\n " + persistantmanager.instence.players[Bidding.instence.highestCount].name + Languages.instence.GetText("chose1") + Languages.instence.GetText(GameManager.instence.trump.ToString()) + Languages.instence.GetText("trumpSuit"), null, 0);
    }
    public void DisplayPartnerHalfs()
    {
        persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[GameManager.instence.selectedpartnerAce].name + Languages.instence.GetText("isPartner") + persistantmanager.instence.players[GameManager.instence.selectedpartnerAce].name + Languages.instence.GetText("chossingTrump"), null, 0);
    }
    public void DisplayVIPstart()
    {
        persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("Viping"), null, 0);
    }
    public void DiplayVipDone(int x , Deck.CardType Trump)
    {
        persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("vipStoped") + x + Languages.instence.GetText("and") + Languages.instence.GetText(Trump.ToString()) + Languages.instence.GetText("suitAsTrump"), null, 0);
    }

    //----------------------------------------Select -------------------------------------------------------------------------

    public void SelectTrump(int number)
    {
        trumpSelected = number;
        for (int x = 0; x < buttonsTrump.Length; x++)
        {
            if (x == number )
            {
                buttonsTrump[x].sprite = sprON;
            }
            else
            {
                buttonsTrump[x].sprite = sprOff;
            }
        }
    }
    public void SelectPartnerAce(int mode)
    {
        partnerAceSelected = mode;
        for (int x = 0; x < buttonsPartnerAce.Length; x++)
        {
            if (x == mode)
            {
                buttonsPartnerAce[x].sprite = sprON;
            }
            else
            {
                buttonsPartnerAce[x].sprite = sprOff;
            }
        }
    }
}
