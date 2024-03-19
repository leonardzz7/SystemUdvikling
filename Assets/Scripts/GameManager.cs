using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instence;

    public GameObject[] players;
    public GameObject[] tricks;
    public GameObject[] tempTricks; 
    public GameObject[] playerPosition;
    public GameObject sepereteDeck;
    public Vector3 sepereteDeckPosition;
    public GameObject startingDeck;
    public List<Card> cardsToExchange;
    public GameObject[] playersCanvas;
    public GameObject[] playersCanvastemp;

    public Deck deck;
    public int playersTurn;
    public Card selectedCard;
    public State state;

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
    public photonController ForRpc;
    bool checkinternet;
    public GameObject CheckInternet;
    bool firstStart;
    
    float turnTimer = 0; 
    int tempTurn =0;
    
    bool timerCheck = false ;

    public Game game; 
    public enum Game
    {
        Hearts,
        Whists
    }
    public enum State
    {
        Animation,
        Biding,
        Selection,
        PLayable,
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
        if (persistantmanager.instence.CardTurn)
        {
            TurnOpponentCardsOFF();
        }
        else
        {
            TurnOpponentCardsOn();
        }

        if (persistantmanager.instence.multiplayer && PhotonNetwork.IsMasterClient)
        {
            GameObject temp;
            temp = PhotonNetwork.InstantiateRoomObject(this.PhotonController.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            ForRpc = temp.GetComponent<photonController>();
        }
        if (persistantmanager.instence.multiplayer)
        {
            InvokeRepeating("CheckIfInternetIsBackOn", 1, 2);
            
        }
        Invoke("Updatenames", 1f);
        firstStart = true;
        SettingsForStarting();
    }
    private void Update()
    {
        TimerProcess();
    }
    public void TimerProcess()
    {
        if ((State.PLayable == state  || State.Waititng == state )&&!exchangAbleReady)
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
                        Artificalplayers.instence.actionStarted = false; 
                        Artificalplayers.instence.WaitForStart();
                        timerCheck = true; 
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
        else if(State.Biding == state || State.Selection == state)
        {
            if (tempTurn == playersTurn)
            {
                if (turnTimer < 120)
                {
                    turnTimer += Time.deltaTime;
                    playersCanvas[playersTurn].transform.GetChild(5).GetComponent<Image>().fillAmount = turnTimer / 120;
                }
                else
                {
                    if (persistantmanager.instence.pNoNow == playersTurn && !timerCheck)
                    {
                        Artificalplayers.instence.actionStarted = false;
                        Artificalplayers.instence.WaitForStart();
                        timerCheck = true;
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
            //players[count].transform.LeanScale( playerPosition[x].transform.localScale , 1 );
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
                    players[count].transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 3:
                    players[count].transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }


            count = Bidding.instence.StepUp(count);

        }
    }
    public void TurnOpponentCardsOn()
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
                    players[count].transform.rotation = Quaternion.Euler(0, 180, 90);
                    break;
                //case 2:
                //    players[count].transform.rotation = Quaternion.Euler(0, 180, 180);
                //    break;
                case 3:
                    players[count].transform.rotation = Quaternion.Euler(0, 180, -90);
                    break;
            }

            count = Bidding.instence.StepUp(count);
        }
    }
    public void Updatenames()
    {
        int turn = persistantmanager.instence.pNoNow;
        tempTricks[turn] = tricks[0]; 
        UpdatePlayerInfo.instence.names[0].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[0].transform);
        obj.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[0].picId0, persistantmanager.instence.players[0].picId1, persistantmanager.instence.players[0].picId2, persistantmanager.instence.players[0].picId3, persistantmanager.instence.players[0].picId4);
        turn = Bidding.instence.StepUp(turn);
        tempTricks[turn] = tricks[1];
        UpdatePlayerInfo.instence.names[1].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj1 = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[1].transform);
        obj1.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[1].picId0, persistantmanager.instence.players[1].picId1, persistantmanager.instence.players[1].picId2, persistantmanager.instence.players[1].picId3, persistantmanager.instence.players[1].picId4);
        turn = Bidding.instence.StepUp(turn);
        tempTricks[turn] = tricks[2];
        UpdatePlayerInfo.instence.names[2].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj2 = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[2].transform);
        obj2.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[2].picId0, persistantmanager.instence.players[2].picId1, persistantmanager.instence.players[2].picId2, persistantmanager.instence.players[2].picId3, persistantmanager.instence.players[2].picId4);
        turn = Bidding.instence.StepUp(turn);
        tempTricks[turn] = tricks[3];
        UpdatePlayerInfo.instence.names[3].text = "" + persistantmanager.instence.players[turn].name;
        GameObject obj3 = Instantiate(CharacterDataLoad.instence.character, UpdatePlayerInfo.instence.characterParents[3].transform);
        obj3.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[3].picId0, persistantmanager.instence.players[3].picId1, persistantmanager.instence.players[3].picId2, persistantmanager.instence.players[3].picId3, persistantmanager.instence.players[3].picId4);
        tricks = tempTricks; 
    }
    public void SettingsForStarting()
    {
        partnerWithSelf = false;
        ResetNotrickList();
        possibleSpecificCardPlay = false;
        TurnOffPassedSigns();
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
    }
    public void SettingsForStartingMulti(int rand) 
    {
        playersTurn = rand; 
        Bidding.instence.gamestarted = false;
        playersCanvas[playersTurn].transform.GetChild(0).gameObject.SetActive(false); //  mkae the dealer visible here --------- here --------
        if (playersTurn == 3)
        {
            playersTurn = 0;
        }
        else
        {
            playersTurn++;
        }
        playersCanvas[playersTurn].transform.GetChild(1).gameObject.SetActive(true);
        playersTurnIndicator(playersTurn);
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
        int total = CardObjects.instence.transform.childCount;
        if (total > 3)
        {
            selectedCard = CardObjects.instence.transform.GetChild(x).GetComponent<Card>();
            if (persistantmanager.instence.multiplayer)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    ForRpc.RandomDistributeMoveCard(selectedCard, playersTurn);
                }
            }
            else
            {
                MoveCardP(selectedCard.cardObject, players[playersTurn], 0, playersTurn);
                selectedCard.cardObject.LeanScale(new Vector3(150 ,150,150) , 0.2f);
            }
            StartCoroutine(FirstWaitForOneCardToPlayer(0.15f));
        }
        else
        {
            if (persistantmanager.instence.multiplayer)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    // object 0 is players, object 1 is startingDeck , object 2 is sepereteDeck  
                    MoveMultipleMultiplayer(CardObjects.instence.transform, sepereteDeck, 2, 0);
                    ForRpc.StartBidding();
                }
            }
            else
            {
                MoveMultipleCards(CardObjects.instence.transform, sepereteDeck, 2, 0);
                StartBidding();
            }
        }

        SortCards();
    }
    public void StartBidding()
    {
        state = State.Biding;
        Bidding.instence.toStart();
        playersTurn = tempPlayerTurn;
        selectedCard = null;
        SortCards();
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
        Artificalplayers.instence.actionStarted = false;
    }
    public void GiveAceHighestNumber()
    {
        CardObjects.instence.cards[0].GetComponent<Card>().cardNumber = 14;
        CardObjects.instence.cards[1].GetComponent<Card>().cardNumber = 14;
        CardObjects.instence.cards[2].GetComponent<Card>().cardNumber = 14;
        CardObjects.instence.cards[3].GetComponent<Card>().cardNumber = 14;
        //GameManager.instence.SortCards();
    }

    //-------------------------------game plays-----------------------------------------------

    public void NextPlayer()
    {
        ProcessForNextPlayer();
    }
    public void ProcessForNextPlayer()
    {
        //GAME-LOGIC-HEARTS//
        posiblePartnerAce = false;
        posibleTrumpNotAvailable = false;
        possibleSpecificCardPlay = false;

        Artificalplayers.instence.actionStarted = false; 
        if (numberOfTurns < 3)
        {
            numberOfTurns++; //1 //2
            if (playersTurn >= 3)
            {
                playersTurn = 0;
            }
            else
            {
                playersTurn++; // 1 //2 ////GAME-LOGIC-HEARTS// // THIS GIVES ACCESS TO NEXT PLAYER.
            }
            playersTurnIndicator(playersTurn);
            //GAME-LOGIC-HEARTS//

            if (CheckIfPlayerhasAnyCard(playersTurn))
            {
                if (!CheckIfPlayerhasType(playersTurn, cardtypeFortheRound))
                {
                    if (CheckIfPlayerhasSpecificCard(playersTurn))
                    {
                        if (cardtypeFortheRound == partnerAce)
                        {
                            possibleSpecificCardPlay = true;
                        }
                        else
                        {
                            Debug.Log("nA 1");
                            posibleTrumpNotAvailable = true;
                        }
                    }
                    else
                    {
                        Debug.Log("nA 2");
                        posibleTrumpNotAvailable = true;
                    }
                }
                else
                {
                    if (cardtypeFortheRound == partnerAce)
                    {
                        if (CheckIfPlayerhasAce(playersTurn, partnerAce) != -1)
                        {
                            posiblePartnerAce = true;
                        }
                    }
                    else if (CheckIfDeckhasSpecificCard())
                    {
                        if (CheckIfPlayerhasAce(playersTurn, partnerAce) != -1)
                        {
                            posiblePartnerAce = true;
                        }
                    }
                }

            }
            else
            {
                //GAME-LOGIC-HEARTS//
                if (CheckIfAnyPlayerhasAnyCard())
                {
                    ProcessForNextPlayer();
                }
                else
                {
                    
                    CheckScores(true);
                }

            }
        }
        else
        {
            //Hearts Logic Calculator// //GAME-LOGIC-HEARTS//
            StartCoroutine(AnalyeScore());
        }
        if (selectedCard != null)
        {
            selectedCard.gameObject.LeanScale(new Vector3(150, 150, 150), 0.2f);
            selectedCard = null;
        }
    }
    public IEnumerator AnalyeScore()
    {
        state = State.Animation;
        yield return new WaitForSeconds(1f);
        highestScored = null;
        bool t = false;
        //GAME-LOGIC-HEARTS//
        foreach (Transform child in trick.transform)
        {
            if (child.GetComponent<Card>().type == cardtypeFortheRound && cardtypeFortheRound != Deck.CardType.Joker && !t)
            {
                CheckIfHighestCard(child);
            }
            else if (child.GetComponent<Card>().type == trump)
            {
                CheckIfHighestCard(child);
                t = true;
            }
            else if (child.GetComponent<Card>().type == Deck.CardType.Joker && cardtypeFortheRound != Deck.CardType.Joker && !t)
            {
                CheckIfHighestCard(child);
            }
        }
        if (highestScored != null)
        {
            //GAME-LOGIC-HEARTS// //TURN SWITCH IN HEARTS BY SCORE
            TrickWonBy(highestScored);
        }
        else
        {
            TrickWonBy(trick.transform.GetChild(0).GetComponent<Card>());
        }
    }
    public void CheckIfHighestCard(Transform card)
    {
        if (highestScored == null)
        {
            highestScored = card.GetComponent<Card>();
        }
        else
        {
            if (card.GetComponent<Card>().type == highestScored.type)
            {
                if (highestScored.cardNumber < card.GetComponent<Card>().cardNumber)
                {
                    highestScored = card.GetComponent<Card>();
                }
            }
            else if (card.GetComponent<Card>().type == trump)
            {
                highestScored = card.GetComponent<Card>();
                GameManager.instence.Trumped();
            }
        }
    }
    public void TrickWonBy(Card highestScored)
    {
        while (trick.transform.childCount > 0)
        {
            //Hearts Logic Calculator// LINE TO CALCULATE HIGHEST CARD// MOVE CARD TO TRICK WINNER //
            PMoveCard(trick.transform.GetChild(0).gameObject, tricks[highestScored.playedBy], 3, highestScored.playedBy);
        }
        persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[(highestScored.playedBy)].name + Languages.instence.GetText("wonTrick"), null, 0);
        state = State.PLayable;
        //GAME-LOGIC-HEARTS// // Move At End
        playersTurn = highestScored.playedBy;
        Scoring.instence.playerstrick[playersTurn, Scoring.instence.roundNo]++;
        CheckScores(false);
        if (CheckIfPlayerhasAnyCard(playersTurn))
        {
            playersTurnIndicator(playersTurn);
            //GAME-LOGIC-HEARTS// //Turn Reset // If Number of tricks is 13 // Don't Run
            //numberOfTurns = 4; //Before Reaching Here
            numberOfTurns = 0;
        }
        else
        {
            if (CheckIfAnyPlayerhasAnyCard())
            {
                if (highestScored.playedBy >= 3)
                {
                    highestScored.playedBy = 0;
                }
                else
                {
                    highestScored.playedBy++;
                }
                TrickWonBy(highestScored);
            }
            else
            {
                // game over analyse score //GAME-LOGIC-HEARTS// // Score
                CheckScores(true);
            }
            turnTimer = 0; 
        }
    }
    public void PlayCard(Card SelectedNowCard)
    {
        if (SelectedNowCard != null)
        {
            PMoveCard(SelectedNowCard.cardObject, trickPositions[playersTurn].gameObject, 4, 0);
            SelectedNowCard.transform.LeanScale(new Vector3(150, 150, 150), 0.1f);
            SelectedNowCard.transform.parent = trick.transform; 
            SelectedNowCard.playedBy = playersTurn;
            //GAME-LOGIC-HEARTS// // current player card power//
            if (SelectedNowCard.type == partnerAce)
            {
                if (SelectedNowCard.cardNumber == 14)
                {
                    partnerRevealed = true;
                    PartnerAceRevealed();
                }
            }
            if (numberOfTurns == 0)
            {
                if (SelectedNowCard == specificCard)
                {
                    cardtypeFortheRound = partnerAce;
                }
                else
                {
                    cardtypeFortheRound = SelectedNowCard.type;
                }
            }
            NextPlayer();
        }
    }
    public void Pass()
    {
        ProcessForNextPlayer();
        if (!CheckIfDeckhasAnyCard())
        {
            numberOfTurns = 0;
        }
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
    public void PartnerAceRevealed()
    {
        PartnerAceSound.Play();
    }
    bool trumpbool = true; 
    public void Trumped()
    {
        if (trumpbool)
        {
            TrumpSound.Play();
            trumpbool = false; 
        }
    }
    // --------------------------------game core-----------------------------------------

    public int intrationNotrickExchange = 0;
    public void MoveCardP(GameObject from, GameObject to, int parent, int chlid)
    {
        LeanTween.move(from, to.transform.position, 0.15f);
        StartCoroutine(SetPandR());
        IEnumerator SetPandR()
        {
            yield return new WaitForSeconds(0.15f);
            from.transform.parent = to.transform;
            from.LeanScale(new Vector3(150, 150, 150), 0.01f);
            from.transform.rotation = to.transform.rotation;
            from.transform.Rotate(0, -0.05f, 0, Space.Self);
            from.GetComponent<Card>().positionParent = parent;
            from.GetComponent<Card>().positionChlid = chlid;
            
        }
        playcard.Play();
    }
    public void PMoveCard(GameObject from, GameObject to, int parent, int chlid)
    {
        from.transform.parent = to.transform;
        from.transform.rotation = to.transform.rotation;
        from.transform.Rotate(0, -0.05f, 0, Space.Self);
        from.GetComponent<Card>().positionParent = parent;
        from.GetComponent<Card>().positionChlid = chlid;
        LeanTween.move(from, to.transform.position, 0.15f);
        //from.gameObject.LeanScale(new Vector3(150, 150, 150), 0.2f);
        playcard.Play();
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
        for (int x = 0; x < countTemp-1; x++)
        {
            for (int y = 0; y < countTemp-1; y++)
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
    }
    public void TurnOffPassedSigns()
    {
        foreach (GameObject player in playersCanvas)
        {
            player.transform.GetChild(3).gameObject.SetActive(false);
        }
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
    private bool showingCards = false;
    public void ShowCards()
    {
        if (!showingCards) {
            players[persistantmanager.instence.pNoNow].LeanMove(new Vector3(0f, -10f, 60f), 0.2f);
            players[persistantmanager.instence.pNoNow].LeanScale(new Vector3(2f, 2f, 2f), 0.2f);
            showingCards = true; 
            StartCoroutine(WaitForShowCards());
        }
    }
    IEnumerator WaitForShowCards()
    {
        yield return new WaitForSeconds(4);
        BackFromShow();
    }
    public void BackFromShow()
    {
        players[persistantmanager.instence.pNoNow].LeanMove(playerPosition[0].transform.position, 0.2f);
        players[persistantmanager.instence.pNoNow].LeanScale(new Vector3(2.3f, 2.3f, 2.3f), 0.2f);
        showingCards = false;
    }

    //------------------------------------exchange Cards ------------------------------

    public void ReadyToexchangeDeckCard()
    {
        if (persistantmanager.instence.multiplayer)
        {
            ForRpc.DisplayMessageToothers(persistantmanager.instence.players[Bidding.instence.highestCount].name + " is now exchanging cards, please wait");
        }
        persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("selectExchange"), null, 0);
        cardFromSepereateDeck = new Transform[3];
        cardFromSepereateDeck[0] = sepereteDeck.transform.GetChild(0);
        cardFromSepereateDeck[1] = sepereteDeck.transform.GetChild(1);
        cardFromSepereateDeck[2] = sepereteDeck.transform.GetChild(2);
        deckPanel.SetActive(false);
        Invoke("ModeTopPlay" , 2f);
        if(selectedGameMode == Bidding.GameMode.Vip)
        {
            Artificalplayers.instence.ExchangeCardsAI();
        }
    }
    public void ModeTopPlay()
    {
        exchangAbleReady = true;
        state = State.PLayable;
    }
    public Transform[] cardFromSepereateDeck;
    IEnumerator waitForExchangedCardsDisplay()
    {
        state = State.Animation;
        foreach (Transform card in cardFromSepereateDeck)
        {
            card.LeanScale(new Vector3(200, 200, 200), 0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        foreach (Transform card in cardFromSepereateDeck)
        {
            card.LeanScale(new Vector3(150, 150, 150), 0.1f);
        }
        state = State.PLayable;
        Artificalplayers.instence.actionStarted = false; 
    }
    public void SelectionExchangeDone()
    {
        ExchangecardsNow();
        exchangePanel.SetActive(false);
    }
    public void SelectionExchangeAgain()
    { 
        foreach (Card card in cardsToExchange)
        {
            card.transform.LeanScale(new Vector3(150, 150, 150), 0.2f);
        }
        vipSelectedCount = 0;
        ReadyToexchangeDeckCard();
        exchangePanel.SetActive(false);
    }
    public void ExchangecardsNow()
    {
        if (persistantmanager.instence.multiplayer)
        {
            // object 0 is players, object 1 is startingDeck , object 2 is sepereteDeck  
            MoveMultipleMultiplayer(sepereteDeck.transform, players[playersTurn], 0, playersTurn);
            foreach (Card card in cardsToExchange)
            {
                //PMoveCard(card.cardObject, sepereteDeck);
                ForRpc.MutipleMoveCard(card, 2, 0);
                card.GetComponent<Card>().playedBy = playersTurn;
            }
            ForRpc.ExchangeCardProcess();
        }
        else
        {
            MoveMultipleCards(sepereteDeck.transform, players[playersTurn], 0, playersTurn);
            foreach (Card card in cardsToExchange)
            {
                PMoveCard(card.cardObject, sepereteDeck, 2, 0);
                card.GetComponent<Card>().playedBy = playersTurn;
            }
            ExchangeCardProcess();
        }
        Invoke("SortCards" , 0.1f);
        StartCoroutine(waitForExchangedCardsDisplay());
    }
    public void messageForExchangeCards()
    {
        persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("choseEchange"), null, 0);
    }
    public void DontWantToExchange()
    {
        if (persistantmanager.instence.multiplayer)
        {
            ForRpc.ExchangeCardProcess();
            ForRpc.DisplayMessageToothers(persistantmanager.instence.players[Bidding.instence.highestCount].name + " chose not to exchange cards"); 
        }
        else
        {
            ExchangeCardProcess();
        }
    }
    public void ExchangeCardProcess()
    {
        if (intrationNotrickExchange < Bidding.instence.notrickGameCounter - 1)
        {
            //Bidding.instence.notrickGameCounter--;
            intrationNotrickExchange++;
            playersTurn = Bidding.instence.noTrickPlayers[intrationNotrickExchange];
            vipSelectedCount = 0;
            playersTurnIndicator(playersTurn);

            if (persistantmanager.instence.multiplayer)
            {
                if (persistantmanager.instence.pNoNow == GameManager.instence.playersTurn)
                {
                    deckPanel.SetActive(true);
                }
                else
                {
                    deckPanel.SetActive(false);
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    Artificalplayers.instence.ExchangeCardsAI();
                }
            }
            else
            {
                deckPanel.SetActive(true);
                Artificalplayers.instence.ExchangeCardsAI();
            }
        }
        else
        {
            exchangAbleReady = false;
            state = State.PLayable;
            sepereteDeck.transform.LeanMove(UiControler.instence.detailsPositions[1].position, 0.01f);
            deckPanel.SetActive(false);
            playersTurn = Bidding.instence.biddingStarter;
            playersTurnIndicator(playersTurn);
            Debug.Log(" given to starter");
            Artificalplayers.instence.StartCoroutine(Artificalplayers.instence.ResetProcess());

            //if (CheckIfPlayerhasType(highestBider, partnerAce) || selectedGameMode == Bidding.GameMode.noTrickGame || selectedGameMode == Bidding.GameMode.noTrickGameFaceUp || selectedGameMode == Bidding.GameMode.TrickGame1)
            //{
            //    playersTurn = Bidding.instence.biddingStarter;
            //    playersTurnIndicator(playersTurn);
            //    Debug.Log(" given to starter");
            //    Artificalplayers.instence.StartCoroutine(Artificalplayers.instence.ResetProcess());
            //}
            //else
            //{
            //    persistantmanager.instence.PopUpWakeUp("please select a cards for Partner Ace", null, 0);
            //    selectSpecificCard = true;
            //    // ----------------------------------To Do ----------------------------------
            //    // ----------------------------------Ai SepecificCard ------------------------------
            //}
        }
    }

    //------------------------------------vip selection----------------------------------

    GameObject selChild;
    Card TempVipSelectCard;
    public int selectedChild = 0; // needs setup For Multiplayer
    public void SelectionVip(int x, bool checkTrump)
    {
        if (selChild != null)
        {
            selChild.LeanScale(new Vector3(150, 150, 150), 0.2f);
            //selChild.LeanRotate(new Vector3(0, 0, 0), 0.2f);
        }
        selChild = sepereteDeck.transform.GetChild(x).gameObject;
        selChild.LeanScale(new Vector3(200, 200, 200), 0.2f);
        selChild.LeanRotate(new Vector3(0, 180, 0), 0.2f);
        if (checkTrump)
        {
            Debug.Log("trump selected ");
            selectedChild = x + 1;
            trump = selChild.GetComponent<Card>().type;
            TempVipSelectCard = selChild.GetComponent<Card>();
        }
    }
    public void UnselectVip()
    {
        if (selChild != null)
        {
            //if (selChild.GetComponent<Card>().type != Deck.CardType.Joker)
            //{
            //    trump = selChild.GetComponent<Card>().type;
            //}
            //else
            //{
            //    trump = Deck.CardType.Sans;
            //}
            selChild.LeanScale(new Vector3(150, 150, 150), 0.2f);
            selChild.LeanRotate(new Vector3(0, 0, 0), 0.2f);
        }
    }
    public void StopSelectionVip()
    {
        for (int x = 2; x > selectionTP.instence.VipCount; x--)
        {
            SelectionVip(x, false);
        }

        if (persistantmanager.instence.multiplayer)
        {
            ForRpc.ProcessForStopSelectVio(TempVipSelectCard);
        }
        else
        {
            sepereteDeck.transform.LeanMove(sepereteDeckPosition, 0.01f);
            UnselectVip();
            ReadyToexchangeDeckCard();
        }
    }
    public void ProcessForStopSelectVio(Card card)
    {
        trump = card.type;
        sepereteDeck.transform.LeanMove(sepereteDeckPosition, 0.01f);
        UnselectVip();
        selectionTP.instence.DiplayVipDone(selectionTP.instence.VipCount , trump);
        if (persistantmanager.instence.pNoNow == GameManager.instence.playersTurn)
        {
            ReadyToexchangeDeckCard();
        }
        if(persistantmanager.instence.multiplayer && PhotonNetwork.IsMasterClient)
        {
            ReadyToexchangeDeckCard();
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
        int tempnumber = 30 ; 
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
    public void CheckScores(bool check)
    {
        int highestNumber = Bidding.instence.bids[Bidding.instence.highestCount].number;
        switch (selectedGameMode)
        {
            case Bidding.GameMode.Ordinary:
            case Bidding.GameMode.Sans:
            case Bidding.GameMode.Clubs:
            case Bidding.GameMode.Halfs:
            case Bidding.GameMode.Vip:
                if (check)
                {
                    if (selectedpartnerAce != -1)
                    {
                        Scoring.instence.GiveScores(true, highestBider, selectedpartnerAce, highestNumber);

                    }
                    else
                    {
                        Scoring.instence.GiveScores(false, highestBider, selectedpartnerAce, highestNumber);
                    }

                    Scoring.instence.FillUpLists();
                    Scoring.instence.GameEnding();
                }
                break;

            case Bidding.GameMode.TrickGame1:
            case Bidding.GameMode.noTrickGameFaceUp:
            case Bidding.GameMode.noTrickGame:
                if (!check)
                {
                    int temp  = CheckLoseingNotrickGame();
                    Debug.Log(temp + ":" + Bidding.instence.notrickGameCounter);
                    if (temp  >= Bidding.instence.notrickGameCounter)
                    {
                        Scoring.instence.noOfNotrickWinners = Bidding.instence.notrickGameCounter - CheckLoseingNotrickGame(); 
                        Scoring.instence.GiveScoresNotrick();
                        Scoring.instence.FillUpLists();
                        Scoring.instence.GameEnding();
                    }
                }
                else
                {
                    Scoring.instence.noOfNotrickWinners = Bidding.instence.notrickGameCounter - CheckLoseingNotrickGame(); 
                    Scoring.instence.GiveScoresNotrick();
                    Scoring.instence.FillUpLists();
                    Scoring.instence.GameEnding();
                }

                break;
        }

    }
    public int CheckLoseingNotrickGame()
    {
        bool Notrick = true; 
        int temp2 = 0;
        if(selectedGameMode == Bidding.GameMode.TrickGame1)
        {
            Notrick = false;
        }
        for (int x = 0; x < Bidding.instence.notrickGameCounter; x++)
        {
            if (Notrick)
            {
                if (CheckIfNoTrick(Bidding.instence.noTrickPlayers[x]))
                {
                    temp2++;
                    Scoring.instence.noTrickWinList[x] = false;
                }
            }
            else
            {
                if (CheckIfOneTrick(Bidding.instence.noTrickPlayers[x]))
                {
                    temp2++;
                    Scoring.instence.noTrickWinList[x] = false;
                }
            }
        }
        return temp2; 
    }
    public void ResetNotrickList()
    {
        for(int x =0; x < 4; x++)
        {
            Scoring.instence.noTrickWinList[x] = true;
        }
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
    public bool CheckIfDeckhasSpecificCard()
    {
        foreach (Transform child in trick.transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                if (child.GetComponent<Card>() == specificCard)
                {
                    //Specific Card//
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckIfPlayerhasSpecificCard(int player)
    {
        Card temp = null;
        Debug.Log("spec for " + player);
        if (playersCanvas[player].transform.GetChild(2).childCount != 0)
        {
            temp = playersCanvas[player].transform.GetChild(2).GetChild(0).GetComponent<Card>();
            //Specfic Card Check For Player//
        }
        if (temp != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckIfNoTrick(int x)
    {
        Debug.Log("No trick  " +( Scoring.instence.playerstrick[x, Scoring.instence.roundNo] >= 1));
        return Scoring.instence.playerstrick[x, Scoring.instence.roundNo] >= 1;
    }
    public bool CheckIfOneTrick(int x)
    {
        return Scoring.instence.playerstrick[x, Scoring.instence.roundNo] > 1;
    }
    public GameObject GiveLeftcardType(int player)
    {
        GameObject temp = null; 
        foreach (Transform child in players[player].transform)
        {
            if (child.GetComponent<Card>() != null)
            {
                temp =  child.gameObject; 
            }
        }
        return temp; 
    }
}
