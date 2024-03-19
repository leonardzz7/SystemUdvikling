
using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static HeartsGameManager;

public class MultiplayerController : MonoBehaviourPunCallbacks, IPunObservable
{

    bool startTimer = false;
    public double timerIncrementValue;

    public double timerIncrementValueTurn;
    double startTime;

    double turnTime;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public bool start;
    void Start()
    {
        start = true;
        HeartsGameManager.instence.ForRpc = this;
        if (photonView.IsMine)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
        }
        else
        {
            //startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }

        turnTime = startTime;

    }
    void updateTimer()
    {
        if (!startTimer) return;

        if (photonView.IsMine)
        {
            timerIncrementValue = PhotonNetwork.Time - startTime;
            timerIncrementValueTurn = PhotonNetwork.Time - turnTime;
        }

    }

    //----------------------------Rpc calls------------------------------------------------------------

    public void PlayCard(Card selCard)
    {
        int index;
        index = System.Array.IndexOf(CardObjects.instence.cards, selCard.gameObject);
        this.photonView.RPC("MoveCardRPC", RpcTarget.All, index);
    }
    public void SelectSepcificCard(Card selCard)
    {
        int index;
        index = System.Array.IndexOf(CardObjects.instence.cards, selCard.gameObject);
        this.photonView.RPC("MoveCardRPC", RpcTarget.All, index);
    }
    public void SettingsForStartingMulti(bool start)
    {
        if (start)
        {
            this.photonView.RPC("SettingsForStartRPC", RpcTarget.All, Random.Range(0, 4));
        }
        else
        {
            this.photonView.RPC("SettingsForStartRPC", RpcTarget.All, Bidding.instence.biddingStarter);
        }
        Debug.Log("rpc CALLED");
    }
    public void ExchangeCardSelected(int q, int w)
    {
        //HeartsGameManager.instence.ExchangeCardsArray[q] = CardObjects.instence.cards[w];
        this.photonView.RPC("ExchangeCardSelectedRPC", RpcTarget.All, q, w);
    }
    public void RandomDistributeMoveCard(Card selCard, int player)
    {
        int index;
        index = System.Array.IndexOf(CardObjects.instence.cards, selCard.gameObject);


        this.photonView.RPC("RandomDistributeRPC", RpcTarget.All, index, player);
    }
    public void ExchangeCards()
    {
        //if(photonView.IsMine==true)
        this.photonView.RPC("AntiClockwiseExchangeCardsrpc", RpcTarget.All);
    }
    public void MutipleMoveCard(Card selCard, int objectNummber, int player)
    {
        // object 0 is players, object 1 is startingDeck , object 2 is sepereteDeck  
        int index;
        index = System.Array.IndexOf(CardObjects.instence.cards, selCard.gameObject);
        this.photonView.RPC("MutipleMoveCardRPC", RpcTarget.All, index, objectNummber, player);
    }
    public void BidHighestDecleared(int mode, int number, int rank, int bidby)
    {
        this.photonView.RPC("BidHighestDeclearedRPC", RpcTarget.All, mode, number, rank, bidby);
    }
    public void BidWon(int mode, int number, int rank, int bidby)
    {
        this.photonView.RPC("BidWonRPC", RpcTarget.All, mode, number, rank, bidby);
    }
    public void Pass(bool x)
    {
        this.photonView.RPC("PassRPC", RpcTarget.All, x);
    }
    public void StartBidding()
    {
        this.photonView.RPC("startBiddingRPC", RpcTarget.All);
    }
    public void NotrickBidProcess(int mode, int number, int rank, int bidby, bool check)
    {
        this.photonView.RPC("NotrickBidProcessRPC", RpcTarget.All, mode, number, rank, bidby, check);
    }
    public void Selection1Process(int partnerAce, int trump)
    {
        this.photonView.RPC("Selection1ProcessRPC", RpcTarget.All, partnerAce, trump);
    }
    public void Selection2Process(int partnerAce, int trump)
    {
        this.photonView.RPC("Selection2ProcessRPC", RpcTarget.All, partnerAce, trump);
    }
    public void ProcessForStopSelectVio(Card selCard)
    {
        int index;
        index = System.Array.IndexOf(CardObjects.instence.cards, selCard.gameObject);
        this.photonView.RPC("processForStopSelectVioRPC", RpcTarget.All, index);
    }
    public void ExchangeCardProcess()
    {
        this.photonView.RPC("ExchangeCardProcessRPC", RpcTarget.All);
    }
    public void LeaveRoom()
    {
        this.photonView.RPC("LeaveRoomRPC", RpcTarget.All);
    }
    public void ProcessForSelectionVip()
    {
        this.photonView.RPC("processForSelectionVipRPC", RpcTarget.All);
    }
    public void StartBackToGame()
    {
        Debug.Log("Back to game post ");
        this.photonView.RPC("BackToGameRPC", RpcTarget.All, persistantmanager.instence.pNoNow);
    }
    public void BackToGame(int player)
    {
        Debug.Log("syncing all data ");
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Card card in GameManager.instence.deck.deck)
            {
                int index = System.Array.IndexOf(CardObjects.instence.cards, card.gameObject);
                this.photonView.RPC("SyncDataMoveRPC", RpcTarget.All, index, card.positionParent, card.positionChlid, player, GameManager.instence.playersTurn);

            }
        }
    }
    public void DisplayMessageToothers(string str)
    {
        this.photonView.RPC("Message", RpcTarget.Others, str);
    }


    // ------------------------------- RPC--------------------------------------------------------------

    [PunRPC]
    void SelectSepcificCard(int index)
    {
        Debug.Log("rpc RECIEVED");
        InOuts.instence.SelectSepcificCard(CardObjects.instence.cards[index]);
    }
    [PunRPC]
    void SyncDataRPC(int index, int objectNo, int player, int playerNo, int pTurn)
    {
        if (persistantmanager.instence.pNoNow == playerNo)
        {
            Debug.Log("Card Sync");
            GameObject temp = null;
            switch (objectNo)
            {
                case 0:
                    temp = GameManager.instence.players[player];
                    break;
                case 1:
                    temp = GameManager.instence.startingDeck;
                    break;
                case 2:
                    temp = GameManager.instence.sepereteDeck;
                    break;
                case 3:
                    temp = GameManager.instence.tricks[player];
                    break;
                case 4:
                    temp = GameManager.instence.trick;
                    break;
            }

            GameManager.instence.playersTurn = pTurn;
            GameManager.instence.playersTurnIndicator(pTurn);
            GameManager.instence.MoveCardP(CardObjects.instence.cards[index], temp, objectNo, player);
        }
    }
    [PunRPC]
    void MoveCardRPC(int index)
    {
        Debug.Log("rpc RECIEVED");
        HeartsGameManager.instence.PlayCard(CardObjects.instence.cards[index].GetComponent<Card>());
        Debug.Log("CARD PLAYED BY RPC");
    }
    [PunRPC]
    void SettingsForStartRPC(int rand)
    {
        Debug.Log("rpc RECIEVED");
        HeartsGameManager.instence.SettingsForStartingMulti(rand);
    }
    [PunRPC]
    void RandomDistributeRPC(int index, int player)
    {
        Debug.Log("rpc RECIEVED");
        // object 0 is players, object 1 is startingDeck , object 2 is sepereteDeck
        Debug.Log("DISTRIBUTE");
        Debug.Log(player);
        Debug.Log(index);
        Debug.Log(HeartsGameManager.instence.players[player]);
        Debug.Log(CardObjects.instence.cards[index]);
        //OJ_Comment
        HeartsGameManager.instence.MoveCardP(CardObjects.instence.cards[index], HeartsGameManager.instence.players[player], 0, player);
    }
    [PunRPC]
    void MutipleMoveCardRPC(int index, int objectNummber, int player)
    {

        GameObject temp = null;
        switch (objectNummber)
        {
            case 0:
                temp = HeartsGameManager.instence.players[player];
                break;
            case 1:
                temp = HeartsGameManager.instence.startingDeck;
                break;
            case 2:
                temp = HeartsGameManager.instence.sepereteDeck;
                break;
        }
        // object 0 is players, object 1 is startingDeck , object 2 is sepereteDeck  
        HeartsGameManager.instence.PMoveCard(CardObjects.instence.cards[index], temp, objectNummber, player);
    }
    [PunRPC]
    void BidHighestDeclearedRPC(int mode, int number, int rank, int bidby)
    {
        Bidding.instence.BidHighestDecleared(mode, number, rank, bidby);
    }
    [PunRPC]
    void BidWonRPC(int mode, int number, int rank, int bidby)
    {
        Bidding.instence.BidWon(mode, number, rank, bidby);
    }
    [PunRPC]
    void PassRPC(bool x)
    {
        Bidding.instence.ProcessForPass(x);
    }
    
    [PunRPC]
    void NotrickBidProcessRPC(int mode, int number, int rank, int bidby, bool check)
    {
        Bidding.instence.NotrickBidProcess(mode, number, rank, bidby, check);
    }
    [PunRPC]
    void Selection1ProcessRPC(int partnerAce, int trump)
    {
        selectionTP.instence.Selection1Process(partnerAce, trump);
    }
    [PunRPC]
    void Selection2ProcessRPC(int partnerAce, int trump)
    {
        selectionTP.instence.Selection2Process(partnerAce, trump);
    }
    [PunRPC]
    void processForStopSelectVioRPC(int index)
    {
        GameManager.instence.ProcessForStopSelectVio(CardObjects.instence.cards[index].GetComponent<Card>());
    }
    [PunRPC]
    void ExchangeCardProcessRPC()
    {
        GameManager.instence.ExchangeCardProcess();

    }
    [PunRPC]
    void LeaveRoomRPC()
    {
        HeartsGameManager.instence.StartCoroutine(HeartsGameManager.instence.LeaveGame());
    }
    [PunRPC]
    void processForSelectionVipRPC()
    {
        selectionTP.instence.processForSelectionVip();
    }
    [PunRPC]
    void BackToGameRPC(int player)
    {
        Debug.Log("Back to game post ");
        BackToGame(player);
    }
    [PunRPC]
    void Message(string str)
    {
        persistantmanager.instence.PopUpWakeUp(str, null, 0);
    }

    //------------------------------Timer Control via photon--------------------------------------

    private void OnApplicationQuit()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // throw new System.NotImplementedException(); 

        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(timerIncrementValue);
            stream.SendNext(timerIncrementValueTurn);
        }
        else
        {
            // Network player, receive data
            this.timerIncrementValue = (double)stream.ReceiveNext();
            this.timerIncrementValueTurn = (double)stream.ReceiveNext();
        }
    }
    [PunRPC]
    void ExchangeCardSelectedRPC(int q, int w)
    {
        if (HeartsGameManager.instence.ExchangeCardsArray[q] != null)
        {
            HeartsGameManager.instence.ExchangeCardsArray[q].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        }
        HeartsGameManager.instence.ExchangeCardsArray[q] = CardObjects.instence.cards[w];
        HeartsGameManager.instence.ExchangeCardsArray[q].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.ExchangeSelectedMat;
        HeartsGameManager.instence.ExchangeCardSelected++;
        if (HeartsGameManager.instence.ExchangeCardSelected == 3)
        {
            HeartsGameManager.instence.ExchangeCardSelected = 0;
            if (persistantmanager.instence.ArtificalPlayers[HeartsGameManager.instence.exchangeTurn] == true)
            {
                HeartsGameManager.instence.OnExchangeButton();
            }
            else
                HeartsGameManager.instence.ExchangeButton.SetActive(true);
            
        }
    }
    [PunRPC]
    void AntiClockwiseExchangeCardsrpc()
    {
        Debug.Log("rpc RECIEVED here in ExchangeCardsrpc(int ewp)");
        HeartsGameManager.instence.exchangeWithPlayerNum = HeartsGameManager.instence.exchangeTurn + 1;
        if (HeartsGameManager.instence.exchangeWithPlayerNum == 4)
            HeartsGameManager.instence.exchangeWithPlayerNum = 0;

        if (HeartsGameManager.instence.ExchangeCardsArray[0] != null)
        {
            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[0], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);

            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[1], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);

            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[2], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);
        }
        HeartsGameManager.instence.exchangeTurn++;
        HeartsGameManager.instence.ExchangeCardsArray[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[2].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[0] = null;
        HeartsGameManager.instence.ExchangeCardsArray[1] = null;
        HeartsGameManager.instence.ExchangeCardsArray[2] = null;
        if (HeartsGameManager.instence.exchangeTurn == 0)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 1)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 2)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 3)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 4)
        {
            Debug.Log("state now......" + HeartsGameManager.instence.state);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.CheckIfPlayerHasTwoOfClubs();
            Debug.Log(HeartsGameManager.instence.playersTurn + " HAS 2 OF CLUBS");
            HeartsGameManager.instence.state = State.PLayable;
            HeartsGameManager.instence.playersTurnIndicator(HeartsGameManager.instence.playersTurn);
        }

    }

    [PunRPC]
    void ClockwiseExchangeCardsrpc()
    {
        Debug.Log("rpc RECIEVED here in ExchangeCardsrpc(int ewp)");
        HeartsGameManager.instence.exchangeWithPlayerNum = HeartsGameManager.instence.exchangeTurn - 1;
        if (HeartsGameManager.instence.exchangeWithPlayerNum == -1)
            HeartsGameManager.instence.exchangeWithPlayerNum = 3;

        if (HeartsGameManager.instence.ExchangeCardsArray[0] != null)
        {
            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[0], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);

            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[1], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);

            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[2], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);
        }
        HeartsGameManager.instence.exchangeTurn--;
        if (HeartsGameManager.instence.exchangeTurn == -1)
            HeartsGameManager.instence.exchangeTurn = 3;
        HeartsGameManager.instence.ExchangeCardsArray[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[2].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[0] = null;
        HeartsGameManager.instence.ExchangeCardsArray[1] = null;
        HeartsGameManager.instence.ExchangeCardsArray[2] = null;
        //if (HeartsGameManager.instence.exchangeTurn == 0)
        //{
        //    HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
        //    HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        //}
        //else 
        if (HeartsGameManager.instence.exchangeTurn == 1)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 2)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 3)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 0)
        {
            Debug.Log("state now......" + HeartsGameManager.instence.state);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.CheckIfPlayerHasTwoOfClubs();
            Debug.Log(HeartsGameManager.instence.playersTurn + " HAS 2 OF CLUBS");
            HeartsGameManager.instence.state = State.PLayable;
            HeartsGameManager.instence.playersTurnIndicator(HeartsGameManager.instence.playersTurn);
        }

    }
    [PunRPC]
    void AcrossExchangeCardsrpc()
    {
        Debug.Log("rpc RECIEVED here in ExchangeCardsrpc(int ewp)");
        if(HeartsGameManager.instence.exchangeTurn<2)
            HeartsGameManager.instence.exchangeWithPlayerNum = HeartsGameManager.instence.exchangeTurn +2;
        else
            HeartsGameManager.instence.exchangeWithPlayerNum = HeartsGameManager.instence.exchangeTurn - 2;



        if (HeartsGameManager.instence.ExchangeCardsArray[0] != null)
        {
            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[0], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);

            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[1], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);

            HeartsGameManager.instence.MoveCardP(HeartsGameManager.instence.ExchangeCardsArray[2], HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].gameObject, 0, 0);
            HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.GetChild(HeartsGameManager.instence.players[HeartsGameManager.instence.exchangeWithPlayerNum].transform.childCount - 1).transform.DOScale(150, 1f);
        }
        HeartsGameManager.instence.exchangeTurn--;
        if (HeartsGameManager.instence.exchangeTurn == -1)
            HeartsGameManager.instence.exchangeTurn = 3;
        HeartsGameManager.instence.ExchangeCardsArray[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[2].transform.GetChild(0).GetComponent<MeshRenderer>().material = HeartsGameManager.instence.NormalMat;
        HeartsGameManager.instence.ExchangeCardsArray[0] = null;
        HeartsGameManager.instence.ExchangeCardsArray[1] = null;
        HeartsGameManager.instence.ExchangeCardsArray[2] = null;
        //if (HeartsGameManager.instence.exchangeTurn == 0)
        //{
        //    HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
        //    HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        //}
        //else 
        if (HeartsGameManager.instence.exchangeTurn == 1)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 2)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 3)
        {
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn - 1].transform.GetChild(1).gameObject.SetActive(false);
            HeartsGameManager.instence.playersCanvas[HeartsGameManager.instence.exchangeTurn].transform.GetChild(1).gameObject.SetActive(true);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.exchangeTurn;
        }
        else if (HeartsGameManager.instence.exchangeTurn == 0)
        {
            Debug.Log("state now......" + HeartsGameManager.instence.state);
            HeartsGameManager.instence.playersTurn = HeartsGameManager.instence.CheckIfPlayerHasTwoOfClubs();
            Debug.Log(HeartsGameManager.instence.playersTurn + " HAS 2 OF CLUBS");
            HeartsGameManager.instence.state = State.PLayable;
            HeartsGameManager.instence.playersTurnIndicator(HeartsGameManager.instence.playersTurn);
        }

    }
}
