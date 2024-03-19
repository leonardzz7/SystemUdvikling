using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class HeartsGameManager : MonoBehaviour
{
    public static HeartsGameManager instence;
    public GameObject[] players;
    public GameObject[] ExchangeCardsArray;
    public Material ExchangeSelectedMat;
    public Material NormalMat;
    public GameObject[] tricks;
    public GameObject[] tempTricks;
    public GameObject[] playerPosition;
    public GameObject sepereteDeck;
    public GameObject ExchangeButton;
    public Vector3 sepereteDeckPosition;
    public GameObject startingDeck;
    public List<Card> cardsToExchange;
    public GameObject[] playersCanvas;
    public GameObject[] playersCanvastemp;

    public int exchangeTurn=0;
    public Deck deck;
    public int playersTurn;
    public Card selectedCard;
    public State state;
    public int ExchangeCardSelected;

    public GameObject trick;
    public Transform[] trickPositions;
    public Transform[] tempTrickPositions;

    public int numberOfTurns;
    public Deck.CardType cardtypeFortheRound;
    public Card[,] tricksWon;
    public bool posiblePartnerAce;
    public bool possibleSpecificCardPlay;
    public bool posibleTrumpNotAvailable;
    public int tempPlayerTurn;
    public Bidding.GameMode selectedGameMode;
    public Deck.CardType trump;
    public Deck.CardType partnerAce;
    public int selectedpartnerAce;
    public bool partnerWithSelf;

    public bool exchangAbleReady;
    public bool selectSpecificCard;
    public Card specificCard;
    public int vipSelectedCount;
    public int highestBider;
    int count;
    Card highestScored;
    public bool partnerRevealed;

    public GameObject deckPanel;
    public GameObject exchangePanel;

    public GameObject PhotonController;
    public MultiplayerController ForRpc;
    bool checkinternet;
    public GameObject CheckInternet;
    bool firstStart;

    float turnTimer = 0;
    int tempTurn = 0;

    bool timerCheck = false;

    public Card[] CurrentTrickCards = new Card[4];
    public int CurrentTrickPoints;
    public int TrickNumber = 0;
    int[] PointsTakenThisRound = new int[] { 0, 0, 0, 0 };
    int[] TotalPointsTaken = new int[] { 0, 0, 0, 0 };
    public Deck.CardType CurrentDominantSuite;
    public bool HeartsBroken;

    /// <summary>
    //Hearts Code
    public Deck.CardType ClubsSuite = Deck.CardType.Club;
    /// </summary>
    public int leadPlayernumber;
    public Card leadCard;
    public int cardplayed;
    public enum State
    {
        Animation,
        Biding,
        Selection,
        PLayable,
        Exchanging,
        Waititng
    }
    private void Awake()
    {
        if (instence == null)
        {
            instence = this;
        }
    }
    void Start()
    {
        cardplayed = 0;
        playersCanvastemp[0] = playersCanvas[0];
        playersCanvastemp[1] = playersCanvas[1];
        playersCanvastemp[2] = playersCanvas[2];
        playersCanvastemp[3] = playersCanvas[3];
        tempTrickPositions = new Transform[4];
        tempTrickPositions[0] = trickPositions[0];
        tempTrickPositions[1] = trickPositions[1];
        tempTrickPositions[2] = trickPositions[2];
        tempTrickPositions[3] = trickPositions[3];
        //if (persistantmanager.instence.multiplayer)
        //{

        TurnOpponentCardsOn();

        if (persistantmanager.instence.multiplayer && PhotonNetwork.IsMasterClient)
        {
            GameObject temp;
            temp = PhotonNetwork.InstantiateRoomObject(this.PhotonController.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            ForRpc = temp.GetComponent<MultiplayerController>();
        }
        if (persistantmanager.instence.multiplayer)
        {
            InvokeRepeating("CheckIfInternetIsBackOn", 1, 2);
        }
        Invoke("Updatenames", 1f);
        firstStart = true;
        Debug.Log("hereeeeeeeeeeeeeeeeeeeeeeeee");
        SettingsForStarting();
    }
    public bool TrickUpdated = false;
    private void Update()
    {
        TimerProcess();
        {
            Debug.Log("Collect");
            
            if( (cardplayed == 4)&&(TrickUpdated==false))
            {

                StartCoroutine(UpdateTrick());
                
            }
            
        }
    }
    IEnumerator UpdateTrick()
    {
        TrickUpdated = true;
        yield return new WaitForSeconds(.6f);
        for (int i = 0; i < CurrentTrickCards.Length; i++)
        {
            Debug.Log("@@@@@@@@@@@@@ " + tricks[leadPlayernumber].transform.name);
            //Debug.Break();
            if (CurrentTrickCards[i] != null)
            {
                CurrentTrickCards[i].transform.parent = tricks[leadPlayernumber].transform;
                CurrentTrickCards[i].gameObject.SetActive(false);
                CurrentTrickCards[i] = null;
            }

            tricks[leadPlayernumber].GetComponent<TrickScore>().CalculateScore();
            playersTurn = leadPlayernumber;
            playersTurnIndicator(playersTurn);
        }
        cardplayed = 0;
        TrickUpdated = false;
    }
    public void TimerProcess()
    {
        if (State.Exchanging == state)
        {

        }
        else if ((State.Biding == state || State.PLayable == state || State.Selection == state || State.Waititng == state) && !exchangAbleReady)
        {
            if (tempTurn == playersTurn)
            {
                if (turnTimer < 15)
                {
                    turnTimer += Time.deltaTime;
                    playersCanvas[playersTurn].transform.GetChild(5).GetComponent<Image>().fillAmount = turnTimer / 15;
                }
                else
                {
                    if (persistantmanager.instence.pNoNow == playersTurn && !timerCheck)
                    {
                        //OJ COMMENT
                        //Artificalplayers.instence.actionStarted = false;
                        //Artificalplayers.instence.WaitForStart();
                        //timerCheck = true;
                    }
                }

            }
            else
            {
                turnTimer = 0;
                playersCanvas[tempTurn].transform.GetChild(5).GetComponent<Image>().fillAmount = 1;
                tempTurn = playersTurn;
                timerCheck = false;

            }
        }
    }
    public void TurnOpponentCardsOFF()
    {
        int count = persistantmanager.instence.pNoNow;
        for (int x = 0; x < 4; x++)
        {
            //LeanTween.move(players[count], playerPosition[x].transform.position, 0.1f);
            //players[count].transform.LeanScale(playerPosition[x].transform.localScale, 1);
            playersCanvas[count] = playersCanvastemp[x];
            trickPositions[count] = tempTrickPositions[x];

            switch (x)
            {
                case 0:
                    players[count].transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 1:
                    players[count].transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 2:
                    players[count].transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 3:
                    players[count].transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }


            count = StepUp(count);

        }
    }
    public void TurnOpponentCardsOn()
    {
        int count = persistantmanager.instence.pNoNow;
        for (int x = 0; x < 4; x++)
        {
            LeanTween.move(players[count], playerPosition[x].transform.position, 0.1f);
            players[count].transform.LeanScale(playerPosition[x].transform.localScale, 1);
            playersCanvas[count] = playersCanvastemp[x];
            trickPositions[count] = tempTrickPositions[x];
            switch (x)
            {
                case 0:
                    players[count].transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 1:
                    players[count].transform.rotation = Quaternion.Euler(0, 180, 90);
                    break;
                case 2:
                    players[count].transform.rotation = Quaternion.Euler(0, 180, 180);
                    break;
                case 3:
                    players[count].transform.rotation = Quaternion.Euler(0, 180, -90);
                    break;
            }
            count = StepUp(count);
            //count = Bidding.instence.StepUp(count);
        }
    }
    public void Updatenames()
    {
        int turn = persistantmanager.instence.pNoNow;
        tempTricks[turn] = tricks[0];
        UpdatePlayerInfo.instence.names[0].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[0].transform);
        obj.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[0].picId0, persistantmanager.instence.players[0].picId1, persistantmanager.instence.players[0].picId2, persistantmanager.instence.players[0].picId3, persistantmanager.instence.players[0].picId4);
        //OJ_COMMENT
        //turn = Bidding.instence.StepUp(turn);
        //OJ_CODE
        turn = StepUp(turn);
        tempTricks[turn] = tricks[1];
        UpdatePlayerInfo.instence.names[1].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj1 = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[1].transform);
        obj1.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[1].picId0, persistantmanager.instence.players[1].picId1, persistantmanager.instence.players[1].picId2, persistantmanager.instence.players[1].picId3, persistantmanager.instence.players[1].picId4);
        //OJ_COMMENT
        //turn = Bidding.instence.StepUp(turn);
        //OJ_CODE
        turn = StepUp(turn);
        tempTricks[turn] = tricks[2];
        UpdatePlayerInfo.instence.names[2].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj2 = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[2].transform);
        obj2.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[2].picId0, persistantmanager.instence.players[2].picId1, persistantmanager.instence.players[2].picId2, persistantmanager.instence.players[2].picId3, persistantmanager.instence.players[2].picId4);
        //OJ_COMMENT
        //turn = Bidding.instence.StepUp(turn);
        //OJ_CODE
        turn = StepUp(turn);
        tempTricks[turn] = tricks[3];
        UpdatePlayerInfo.instence.names[3].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj3 = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[3].transform);
        obj3.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[3].picId0, persistantmanager.instence.players[3].picId1, persistantmanager.instence.players[3].picId2, persistantmanager.instence.players[3].picId3, persistantmanager.instence.players[3].picId4);
        tricks = tempTricks;
    }
    public int StepUp(int temp)
    {
        if (temp == 3)
        {
            temp = 0;
        }
        else
        {
            temp++;
        }
        return temp;
    }
    public int StepDown(int temp)
    {
        if (temp == 0)
        {
            temp = 3;
        }
        else
        {
            temp--;
        }
        return temp;
    }
    public void SettingsForStarting()
    {
        Debug.Log(" SettingsForStarting hereeeeeeeeeeeeeeeeeeeeeeeee");
        partnerWithSelf = false;

        ExchangeCardsArray = new GameObject[3];
        possibleSpecificCardPlay = false;

        intrationNotrickExchange = 0;
        selectSpecificCard = false;
        specificCard = null;
        partnerRevealed = false;
        vipSelectedCount = 0;
        exchangAbleReady = false;
        trump = Deck.CardType.NotSelected;
        partnerAce = Deck.CardType.NotSelected;
        selectedpartnerAce = -1;
        sepereteDeck.transform.LeanMove(sepereteDeckPosition, 0.01f);
        posibleTrumpNotAvailable = false;
        cardsToExchange = new List<Card>();
        tricksWon = new Card[4, 20];
        numberOfTurns = 0;
        foreach (var player in playersCanvas)
        {
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(4).gameObject.SetActive(false);
        }
        deck = new Deck(55);
        state = State.Animation;
        GiveAceHighestNumber();

        if (persistantmanager.instence.multiplayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ForRpc.SettingsForStartingMulti(firstStart);
                firstStart = false;
            }
        }
        else
        {
            SettingsForStartingMulti(Random.Range(0, 4));
        }
        Debug.Log(" SettingsForStarting closed hereeeeeeeeeeeeeeeeeeeeeeeee");
    }
    public void SettingsForStartingMulti(int rand)
    {
        playersTurn = rand;
        //OJ_COMMENT
        //Bidding.instence.gamestarted = false;
        state = State.Exchanging;
        playersCanvas[playersTurn].transform.GetChild(0).gameObject.SetActive(false); //  mkae the dealer visible here --------- here --------
        if (playersTurn == 3)
        {
            Debug.Log(CurrentTrickPoints + "..........CurrentTrickPoints..........");
            Debug.Break();
            playersTurn = 0;
        }
        else
        {
            playersTurn++;
        }
        //playersCanvas[playersTurn].transform.GetChild(1).gameObject.SetActive(true);
        //playersTurnIndicator(playersTurn);
        tempPlayerTurn = playersTurn;
        FirstNextPlayer();
        Invoke("invokeCallForDelay", 2f);
    }

    //-------------------------------card distribute-----------------------------------------

    public void invokeCallForDelay()
    {
        if (persistantmanager.instence.multiplayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                RandomDistribute(UnityEngine.Random.Range(0, CardObjects.instence.transform.childCount));
            }
        }
        else
        {
            RandomDistribute(UnityEngine.Random.Range(0, CardObjects.instence.transform.childCount));
        }
    }
    public void RandomDistribute(int x)
    {
        foreach (var player in playersCanvas)
        {
             player.transform.GetChild(1).gameObject.SetActive(false);
        }

        int total = CardObjects.instence.transform.childCount;
        Debug.Log("CHILD COUNT IS :" + CardObjects.instence.transform.childCount);
        if (total > 0)
        {
            selectedCard = CardObjects.instence.transform.GetChild(x).GetComponent<Card>();

            if (PhotonNetwork.IsMasterClient)
            {
                ForRpc.RandomDistributeMoveCard(selectedCard, playersTurn);
            }

            StartCoroutine(FirstWaitForOneCardToPlayer(0.15f));
        }
        else
        {
            ExchangeCards();
            playersTurn = 0;
            playersTurnIndicator(playersTurn);
        }
        SortCards();
    }
    public void ExchangeCards()
    {
        Debug.Log("exchange now......"+this.state);
        this.state = State.Exchanging;
    }

    public int exchangeWithPlayerNum = 0;



    public void OnExchangeButton()
    {
        
        ForRpc.ExchangeCards();

    }
    IEnumerator FirstWaitForOneCardToPlayer(float x)
    {
        yield return new WaitForSeconds(x);
        selectedCard.cardObject.transform.parent = players[playersTurn].transform;
        selectedCard.cardObject.transform.rotation = players[playersTurn].transform.rotation;
        FirstNextPlayer();
        invokeCallForDelay();
    }
    public void FirstNextPlayer()
    {
        //Debug.Log("Next Player");
        if (playersTurn >= 3)
        {
            playersTurn = 0;
        }
        else
        {
            playersTurn++;
        }
       //        Artificalplayers.instence.actionStarted = false;
    }


    //-------------------------------game plays-----------------------------------------------



    public void PlayCard(Card SelectedNowCard)
    {
        if (SelectedNowCard != null)
        {
            if (!HeartsBroken)
            {
                if (SelectedNowCard.type == Deck.CardType.Heart)
                {
                    HeartsBroken = true;
                    Debug.Log("Hearts Broken");
                }
            }
            if (numberOfTurns == 0)
            {
                //CurrentDominantSuite = SelectedNowCard.type;
                Debug.Log("Current Dominant Suite is :" + SelectedNowCard.type);
            }
            CurrentTrickCards[playersTurn] = SelectedNowCard;
            if (CurrentTrickCards[playersTurn].type == Deck.CardType.Heart)
            {
                CurrentTrickPoints++;
                Debug.Log("Hearts Point Added");
            }
            else if (CurrentTrickCards[playersTurn].cardNumber == 11)
            {
                if (CurrentTrickCards[playersTurn].type == Deck.CardType.Spade)
                {
                    CurrentTrickPoints = CurrentTrickPoints + 13;
                    Debug.Log("Queen Points Added");
                }
            }
            Debug.Log("Current Trick Card is for  : " + playersTurn + " is " + CurrentTrickCards[playersTurn]);
            PMoveCard(SelectedNowCard.cardObject, trickPositions[playersTurn].gameObject, 4, 0);
            SelectedNowCard.transform.LeanScale(new Vector3(150, 150, 150), 0.1f);
            SelectedNowCard.transform.parent = trick.transform;
            SelectedNowCard.playedBy = playersTurn;
        }
    }
     public void ProcessForNextPlayer()
    {
        //GAME-LOGIC-HEARTS//
        //if (numberOfTurns < 3)
        //{
            //numberOfTurns++;
            if (playersTurn >= 3)
            {

                Debug.Log(CurrentTrickPoints + "..........CurrentTrickPoints..........");
                Debug.Break();
                playersTurn = 0;
            }
            else
            {
                playersTurn++; 
            }
            playersTurnIndicator(playersTurn);
        //}
        //else
        //{
        //    Debug.Log("Trick Finished");
        //    int TrickHighestPlayer = CalculateThisTrickHighest();
        //    Debug.Log("Trick Highest Player Is :" + TrickHighestPlayer);
        //    PointsTakenThisRound[TrickHighestPlayer] = CurrentTrickPoints;
        //    TrickNumber++;
        //    Debug.Log("TRICK PARENT HAS: " + trick.transform.childCount);
        //    while (trick.transform.childCount > 0)
        //    {
        //        Debug.Log("CARD MOVED");
        //        //Hearts Logic Calculator// LINE TO CALCULATE HIGHEST CARD // MOVE CARD TO TRICK WINNER //
        //        //3??
        //        PMoveCard(trick.transform.GetChild(0).gameObject, tricks[TrickHighestPlayer], 3, TrickHighestPlayer);
        //    }

        //    if (TrickNumber < 13)
        //    {
        //        CurrentTrickPoints = 0;
        //        numberOfTurns = 0;
            
        //        playersTurn = TrickHighestPlayer;
        //        playersTurnIndicator(TrickHighestPlayer);
             
        //        Debug.Log("Trick Reset");  
        //    }
        //    else
        //    {
        //        CalculateScore();
        //    }
        //}

    }

    public void CalculateScore()
    {
        int ShootMoonIndex = 0;
        bool MoonShot = false;
        for (int i = 0; i <= 3; i++)
        {
            if (PointsTakenThisRound[i] >= 26)
            {
                ShootMoonIndex = i;
                MoonShot = true;
                PointsTakenThisRound[i] = 0;
                break;
            }
        }
        if (MoonShot)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (i == ShootMoonIndex)
                {
                    continue;
                }
                else
                {
                    PointsTakenThisRound[i] = 26;
                }
            }
        }
        for (int i = 0; i <= 3; i++)
        {
            TotalPointsTaken[i] = TotalPointsTaken[i] + PointsTakenThisRound[i];
        }
        
        bool WinnerPossible = false;
        for (int i = 0; i <= 3; i++)
        {
            if (TotalPointsTaken[i] >= 100)
            {
                WinnerPossible = true;
            }
        }
        if (WinnerPossible)
        {
            int LeastPoints = TotalPointsTaken[0];
            int CurrentWinnerIndex = 0;
            for (int i = 1; i <= 3; i++)
            {
                if (LeastPoints > TotalPointsTaken[i])
                {
                    LeastPoints = TotalPointsTaken[i];
                    CurrentWinnerIndex = i;
                }
            }
            DeclareWinner(CurrentWinnerIndex);
        }
        else
        {
            TrickNumber = 0;
            //RESHUFFLE ROUND//
        }
    }

    public void DeclareWinner (int WinnerIndex)
    {

    }

    public void playersTurnIndicator(int Pno)
    {
        int count = 0;
        foreach (var player in playersCanvas)
        {
            if (count == Pno)
            {
                player.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                player.transform.GetChild(1).gameObject.SetActive(false);
            }
            count++;
        }
        Debug.Log("TurnIncremented");
    }

    //---------------------------------Sounds--------------------------------------------
    public AudioSource playcard;
    public AudioSource buttonPress;
    public AudioSource Winning;
    public AudioSource Lossing;
    public AudioSource TrumpSound;
    public AudioSource PartnerAceSound;
    public void ButtonPressed()
    {
        buttonPress.Play();
    }
    public void WinSound()
    {
        Winning.Play();
    }
    public void lossSound()
    {
        Lossing.Play();
    }

    // --------------------------------game core-----------------------------------------

    public int intrationNotrickExchange = 0;
    public void MoveCardP(GameObject from, GameObject to, int parent, int chlid)
    {
        Debug.Log("MOVE_CARD_START");
        LeanTween.move(from, to.transform.position, 0.15f);
        StartCoroutine(SetPandR());
        IEnumerator SetPandR()
        {
            yield return new WaitForSeconds(0.15f);
            //Debug.Log("from.transform.parent..."+ from.transform.parent);
            Debug.Log("to.transform...."+ to.transform);
            from.transform.parent = to.transform;
            from.LeanScale(new Vector3(150, 150, 150), 0.01f);
            from.transform.rotation = to.transform.rotation;
            from.transform.Rotate(0, -0.05f, 0, Space.Self);
            from.GetComponent<Card>().positionParent = parent;
            from.GetComponent<Card>().positionChlid = chlid;

        }
        //OJ_Comment
        //playcard.Play();
        Debug.Log("MOVE_CARD_COMPLETE");
    }
    public void PMoveCard(GameObject from, GameObject to, int parent, int chlid)
    {
        from.transform.parent = to.transform;
        from.transform.rotation = to.transform.rotation;
        from.transform.Rotate(0, -0.05f, 0, Space.Self);
        from.GetComponent<Card>().positionParent = parent;
        from.GetComponent<Card>().positionChlid = chlid;
        LeanTween.move(from, to.transform.position, 0.15f);
        if(state!=State.Exchanging)
        ProcessForNextPlayer();
        Debug.Log("CARD PLAYED");
        //from.gameObject.LeanScale(new Vector3(150, 150, 150), 0.2f);
        //OJ_Comment
        //playcard.Play();
    }
    public void ResetDeck()
    {
        foreach (GameObject trick in tricks)
        {
            MoveMultipleCards(trick.transform, startingDeck, 1, 0);
        }
        foreach (GameObject player in players)
        {
            MoveMultipleCards(player.transform, startingDeck, 1, 0);
        }
        MoveMultipleCards(sepereteDeck.transform, startingDeck, 1, 0);
        MoveMultipleCards(trick.transform, startingDeck, 1, 0);
        SettingsForStarting();
    }
    public void MoveMultipleCards(Transform from, GameObject to, int parent, int chlid)
    {
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[from.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in from)
        {
            if (child.GetComponent<Card>() != null)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }
        }
        //Now move them
        foreach (GameObject child in allChildren)
        {
            if (child != null)
                PMoveCard(child, to, parent, chlid);
        }
    }
    public void MoveMultipleMultiplayer(Transform from, GameObject to, int objectNumber, int player)
    {
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[from.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in from)
        {
            if (child.GetComponent<Card>() != null)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }
        }

        //Now move them
        foreach (GameObject child in allChildren)
        {
            if (child != null)
                ForRpc.MutipleMoveCard(child.GetComponent<Card>(), objectNumber, player);
        }

    }
    public void SortCards()
    {
        for (int player = 0; player < 4; player++)
        {
            count = 1;
            SortAndPlace(player, Deck.CardType.Diamond);
            SortAndPlace(player, Deck.CardType.Spade);
            SortAndPlace(player, Deck.CardType.Heart);
            SortAndPlace(player, Deck.CardType.Club);
            SortAndPlace(player, Deck.CardType.Joker);
        }
        Debug.Log("Cards Sorted");
    }
    public void SortAndPlace(int player, Deck.CardType type)
    {
        Card[] arr = new Card[13];
        int countTemp = 0;
        Card temp;
        foreach (Transform child in players[player].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>().type == type)
                {
                    arr[countTemp] = child.GetComponent<Card>();
                    countTemp++;
                }
            }
        }
        for (int x = 0; x < countTemp - 1; x++)
        {
            for (int y = 0; y < countTemp - 1; y++)
            {
                if (arr[y].cardNumber > arr[y + 1].cardNumber)
                {
                    temp = arr[y + 1];
                    arr[y + 1] = arr[y];
                    arr[y] = temp;
                }
            }
        }
        for (int x = 0; x < countTemp; x++)
        {
            arr[x].transform.SetSiblingIndex(count);
            arr[x].transform.Rotate(0, -0.12f, 0, Space.Self);
            count++;
        }
        Debug.Log("Cards Placed");
    }

    public IEnumerator LeaveGame()
    {
        persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("playerLeft"), null, 0);
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).CustomProperties.Clear();
                PhotonNetwork.LocalPlayer.CustomProperties.Clear();
                //PhotonNetwork.LeaveLobby();
                Debug.Log("room Left");
            }
        }
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(0);
    }
    public void CheckIfInternetIsBackOn()
    {
        if (persistantmanager.instence.multiplayer)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("checking Internet Disconnected");
                checkinternet = true;
                CheckInternet.SetActive(true);
            }
            else
            {

                if (checkinternet)
                {
                    print("Reconnecting and rejoining");
                    if (PhotonNetwork.ReconnectAndRejoin())
                    {
                        checkinternet = false;
                        CheckInternet.SetActive(false);
                        ForRpc.StartBackToGame();
                    }
                }
            }
            //if (PhotonNetwork.CurrentRoom.PlayerCount < 4)
            //{
            //    // -------------------------------------------------------------------------------------------------------------- One player disconnected ----------------------------
            //}
        }
    }



    //---------------------------------checks-------------------------------------------

    public bool CheckIfPlayerhasType(int player, Deck.CardType type)
    {
        foreach (Transform child in players[player].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>().type == type)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public GameObject GetHighestPlayer(int player, Deck.CardType type)
    {
        GameObject tempCard = null;
        int tempnumber = 0;
        foreach (Transform child in players[player].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>().type == type)
                {
                    if (child.GetComponent<Card>().cardNumber >= tempnumber)
                    {
                        tempCard = child.gameObject;
                    }
                }
            }
        }
        return tempCard;
    }
    public GameObject GetLowestPlayer(int player, Deck.CardType type)
    {
        GameObject tempCard = null;
        int tempnumber = 30;
        foreach (Transform child in players[player].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>().type == type)
                {
                    if (child.GetComponent<Card>().cardNumber <= tempnumber)
                    {
                        tempCard = child.gameObject;
                    }
                }
            }
        }
        return tempCard;
    }
    public int CheckIfPlayerhasAce(Deck.CardType type)
    {
        for (int player = 0; player < 4; player++)
        {
            foreach (Transform child in players[player].transform)
            {
                if (child.GetComponent<Card>() != null)
                {
                    if (child.GetComponent<Card>().type == type)
                    {
                        if (child.GetComponent<Card>().cardNumber == 14)
                        {
                            return player;
                        }
                    }
                }
            }
        }
        return -1;
    }
    public int CheckIfPlayerhasAce(int player, Deck.CardType type)
    {
        foreach (Transform child in players[player].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>().type == type)
                {
                    if (child.GetComponent<Card>().cardNumber == 14)
                    {
                        return player;
                    }
                }
            }
        }
        return -1;
    }
    public bool CheckIfPlayerhasAnyCard(int player)
    {
        if (players[player].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            Debug.Log("No card Found ");
            return false;
        }
    }
    public bool CheckIfAnyPlayerhasAnyCard()
    {
        foreach (GameObject player in players)
        {
            if (player.transform.childCount > 0)
            {
                return true;
            }
        }
        Debug.Log("No card Found in all  ");
        return false;
    }
    public bool CheckIfDeckhasAnyCard()
    {
        if (trick.transform.childCount >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

  
     public int CheckIfPlayerHasTwoOfClubs()
    {
        for (int player = 0; player <= 3; player++)
        {
            foreach (Transform child in players[player].transform)
            {
                if (child.GetComponent<Card>() != null)
                {
                    if (child.GetComponent<Card>().type == Deck.CardType.Club)
                    {
                        if (child.GetComponent<Card>().cardNumber == 1)
                        {
                            return player;
                        }
                    }
                }
            }
        }
        return -1;
    }

    public bool CheckIfPlayerHasDominantSuite()
    {
        foreach (Transform child in players[playersTurn].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>().type == CurrentDominantSuite)
                {
                   return true;   
                }
            }
        }
        return false;
    }

    public int CalculateThisTrickHighest()
    {
        int HighestNumber = 0;
        int HighestIndex = -1;
        for (int i = 0; i <= 3; i++)
        {
            if (CurrentTrickCards[i].type == CurrentDominantSuite)
            {
                Debug.Log("Player " + i + " Has DominantSuite");
                Debug.Log("Power Of Card For " + i + " is " + CurrentTrickCards[i].cardNumber);
               if (CurrentTrickCards[i].cardNumber > HighestNumber)
               {
                    HighestNumber = CurrentTrickCards[i].cardNumber;
                    Debug.Log("Highest Number Is :" + HighestNumber);
                    HighestIndex = i;
               }
            }
        }
        Debug.Log("HighestIndex is " + HighestIndex);
        return HighestIndex;
    }

    public void GiveAceHighestNumber()
    {
        CardObjects.instence.cards[0].GetComponent<Card>().cardNumber = 13;
        CardObjects.instence.cards[1].GetComponent<Card>().cardNumber = 13;
        CardObjects.instence.cards[2].GetComponent<Card>().cardNumber = 13;
        CardObjects.instence.cards[3].GetComponent<Card>().cardNumber = 13;
        //GameManager.instence.SortCards();
    }


}
