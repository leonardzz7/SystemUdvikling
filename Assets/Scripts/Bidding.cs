using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Bidding : MonoBehaviour
{
    public static Bidding instence;
    public GameObject[] panels;
    
    public bid[] bids;
    public int itration;
    
    public int highestCount = 0;
    public bool[] passed;
    public int preHighestCount = 1;
    public int biddingStarter;
    public bool highestBidderDeclared;
    public bool checkwithPreviousPlayer;
    public int numberOfPlayerPassed;

    public int[] noTrickPlayers;
    public GameMode previousNotrickGame;
    public int notrickGameCounter;

    public TextMeshProUGUI HighestBid;
    public TextMeshProUGUI numberText;

    public Image[] buttonsNumbers;
    public Image[] buttonModes;

    public int numberSelected;
    public int modeSelected;

    public Sprite NumberSprON;
    public Sprite ModeSprON;

    public Sprite NumberSprOff;
    public Sprite ModeSprOff;

    public bool gamestarted = false;
    private void Awake()
    {
        if (instence == null)
        {
            instence = this;
        }
        noTrickPlayers = new int[4];
    }
    public enum GameMode
    {
        Ordinary,
        Clubs,
        Halfs,
        Sans,
        Vip,
        TrickGame1,
        noTrickGame,
        noTrickGameFaceUp
    }
    public class bid
    {
        public int number;
        public int rank;
        public GameMode mode;
        public int bidBy;
    }
    public void toStart()
    {
        UnselectAll();
        gamestarted = true;
        numberOfPlayerPassed = 0;
        notrickGameCounter = 0;
        previousNotrickGame = GameMode.Ordinary;
        populateIntWithEmpty();
        passed = new bool[4];
        highestBidderDeclared = false;
        checkwithPreviousPlayer = false;
        bids = new bid[4];
        bids[0] = new bid();
        bids[1] = new bid();
        bids[2] = new bid();
        bids[3] = new bid();
        highestCount = 0;
        preHighestCount = GameManager.instence.playersTurn;
        if (GameManager.instence.playersTurn == 0)
        {
            biddingStarter = 3;
        }
        else
        {
            biddingStarter = GameManager.instence.playersTurn - 1;
        }
        itration = biddingStarter;
        MultiPlayerIni(itration,0);
    }
    public void MultiPlayerIni(int player ,int ini)
    {
        if (persistantmanager.instence.multiplayer)
        {
            if(player == persistantmanager.instence.pNoNow)
            {
                switchPanels(ini);
            }
            else
            {
                switchPanels(5);
            }
        }
        else
        {
            switchPanels(ini);
        }
    }
    public void MultiPlayerIniEmpaty(int player, int ini)
    {
        if (persistantmanager.instence.multiplayer)
        {
            if (player == persistantmanager.instence.pNoNow)
            {
                switchPanels(ini);
            }
            else
            {
                switchPanels(4);
            }
        }
        else
        {
            switchPanels(ini);
        }
    }
    public void SelectNumber(int number)
    {
        numberSelected = number;
        for (int x =0 ; x<buttonsNumbers.Length ; x++)
        {
            if(x == number-8)
            {
                buttonsNumbers[x].sprite = NumberSprON;
            }
            else
            {
                buttonsNumbers[x].sprite = NumberSprOff;
            }
        }
    }
    public void SelectedMode(int mode)
    {
        modeSelected = mode;
        for (int x = 0; x < buttonModes.Length; x++)
        {
            if (x == mode)
            {
                buttonModes[x].sprite = ModeSprON;
            }
            else
            {
                buttonModes[x].sprite = ModeSprOff;
            }
        }
    }
    public void AutoBid(int number , int mode)
    {
        SelectNumber(number);
        SelectedMode(mode);
        Debug.Log("auto bid");
        Bid();
    }
    public void UnselectAll()
    {
        for (int x = 0; x < buttonModes.Length; x++)
        {
            buttonModes[x].sprite = ModeSprOff;
        }
        for (int x = 0; x < buttonsNumbers.Length; x++)
        {
            buttonsNumbers[x].sprite =NumberSprOff;
            
        }
    }
    public void CalculateHighestBid()
    {
        if (highestBidderDeclared)
        {
            if (!checkwithPreviousPlayer)
            {
                int count = 0;
                foreach (var bid in bids)
                {
                    if (bid != null)
                    {
                        if (bid.rank > bids[highestCount].rank)
                        {
                            highestCount = count;
                            GameManager.instence.highestBider = highestCount;
                        }
                    }
                    count++;
                }
            }
            else
            {
                if (bids[itration].rank >= bids[highestCount].rank)
                {
                    highestCount = itration;
                    GameManager.instence.highestBider = highestCount;
                    return;
                }
            }
        }
        else
        {
            highestBidderDeclared = true;
            highestCount = GameManager.instence.playersTurn;
            checkwithPreviousPlayer = true;
            GameManager.instence.highestBider = highestCount;
        }

    }
    private void Update()
    {
        if (bids[highestCount]!= null)
        {
            if (bids[highestCount].mode == GameMode.noTrickGame || bids[highestCount].mode == GameMode.noTrickGameFaceUp || bids[highestCount].mode == GameMode.TrickGame1)
            {
                Debug.Log("///////" + +GameManager.instence.selectedChild);
                HighestBid.text = "" + Languages.instence.GetText(bids[highestCount].mode.ToString());
            }
            else
            {
                Debug.Log("......."  + GameManager.instence.selectedChild);
                HighestBid.text = "" + bids[highestCount].number + " " + Languages.instence.GetText(bids[highestCount].mode.ToString());
                if (GameManager.instence.selectedChild > 0)
                {
                    HighestBid.text = HighestBid.text + " " + GameManager.instence.selectedChild;
                }
            }
        }

       
    }
    public void FirstPanelNext()
    {
        //itration = GameManager.instence.playersTurn;
        //bids[itration].number = (int)slider.value;
        switchPanels(1);
    }
    public void Bid()
    {
        UnselectAll();
        itration = GameManager.instence.playersTurn;
        bids[itration].number = numberSelected;
        switch (modeSelected)
        {
            case 0:
                bids[itration].mode = GameMode.Ordinary;
                bids[itration].rank = (bids[itration].number*10)  + 1;
                break;
            case 1:
                bids[itration].mode = GameMode.Clubs;
                bids[itration].rank = (bids[itration].number * 10)  + 3;
                break;
            case 2:
                bids[itration].mode = GameMode.Halfs;
                bids[itration].rank = (bids[itration].number * 10)  + 3;
                break;
            case 3:
                bids[itration].mode = GameMode.Sans;
                bids[itration].rank = (bids[itration].number * 10)  + 3;
                break;
            case 4:
                bids[itration].mode = GameMode.Vip;
                bids[itration].rank = (bids[itration].number * 10) + 3;
                break;
            case 5:
                bids[itration].mode = GameMode.TrickGame1;
                bids[itration].rank = 92;
                bids[itration].bidBy = itration;
                if (!NoBidMultiProcess(GameMode.TrickGame1 , true))
                {
                    return;
                }
                break;
            case 6:
                bids[itration].mode = GameMode.noTrickGame;
                bids[itration].rank = 102;
                bids[itration].bidBy = itration;
                if (!NoBidMultiProcess(GameMode.noTrickGame , true))
                {
                    return;
                }

                break;
            case 7:
                bids[itration].mode = GameMode.noTrickGameFaceUp;
                bids[itration].rank = 112;
                bids[itration].bidBy = itration;
                if (!NoBidMultiProcess(GameMode.noTrickGameFaceUp, true))
                {
                    return;
                }
                break;
        }
        bids[itration].bidBy = itration;

        CalculateHighestBid();
        if (!passed[preHighestCount])
        {
            if (highestCount == itration)
            {
                if (persistantmanager.instence.multiplayer)
                {
                    GameManager.instence.ForRpc.BidHighestDecleared((int)bids[itration].mode, bids[itration].number, bids[itration].rank, bids[itration].bidBy);
                }
                else
                {
                    BidHighestDecleared((int)bids[itration].mode, bids[itration].number, bids[itration].rank, bids[itration].bidBy);
                }
            }
            else
            {
                persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("bidHigher"), null, 0);
                switchPanels(0);
            }
        }
        else
        {
            MultiplayerCheckBidWon(true);
        }
    }
    int temp;
    public void BidHighestDecleared(int mode, int number, int rank , int bidby)
    {
        bids[itration].mode = (GameMode)mode;
        bids[itration].rank = rank;
        bids[itration].number = number;
        bids[itration].bidBy = bidby;
        SelectedMode(mode);
        SelectNumber(number);

        if(bids[itration].mode == GameMode.noTrickGame || bids[itration].mode == GameMode.noTrickGameFaceUp || bids[itration].mode == GameMode.TrickGame1)
        {
            if (previousNotrickGame == GameMode.noTrickGame || previousNotrickGame == GameMode.noTrickGameFaceUp || previousNotrickGame == GameMode.TrickGame1)
            {
                preHighestCount = FindFirstInLine();
                populateIntWithEmpty();
            }
            notrickGameCounter = 0;
            previousNotrickGame = (GameMode)mode;
            noTrickPlayers[notrickGameCounter] = GameManager.instence.playersTurn;
            notrickGameCounter = 1;
        }
        else
        {
            if (previousNotrickGame == GameMode.noTrickGame || previousNotrickGame == GameMode.noTrickGameFaceUp || previousNotrickGame == GameMode.TrickGame1)
            {
                Debug.Log("back from no bid");
                preHighestCount = FindFirstInLine();
                GameManager.instence.playersTurn = FindSecondInLine();
                if (preHighestCount == -1)
                {
                    preHighestCount = PrehighestCal(StepUp(GameManager.instence.playersTurn));
                }
                if (GameManager.instence.playersTurn== - 1)
                {
                    GameManager.instence.playersTurn = PrehighestCal(StepUp(preHighestCount));
                }
                previousNotrickGame = (GameMode)mode;
            }
            populateIntWithEmpty();
            notrickGameCounter = 0;
        }
        if (!highestBidderDeclared)
        {
            highestBidderDeclared = true;
            checkwithPreviousPlayer = true;
        }
        highestCount = itration;
        GameManager.instence.highestBider = itration; 
        temp = GameManager.instence.playersTurn;
        GameManager.instence.playersTurn = preHighestCount;
        GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
        preHighestCount = temp;
        itration = GameManager.instence.playersTurn;
        Artificalplayers.instence.actionStarted = false;
        MultiPlayerIni(itration,0);

        if (checkwithPreviousPlayer)
        {
            checkwithPreviousPlayer = false;
        }
        else
        {
            checkwithPreviousPlayer = true;
        }
        //DisplayBidsInlog();
    }
    public int PrehighestCal(int x)
    {
        if (!passed[x])
        {
            Debug.Log(x);
            return x;
        }
        else
        {
            return PrehighestCal(StepUp(x)); 
        }
    }
    public bool NoBidMultiProcess(GameMode mode , bool check)
    {
        if (previousNotrickGame != mode)
        {
            return true;
        }
        else
        {
            previousNotrickGame = mode;
            if (itration == biddingStarter && numberOfPlayerPassed + notrickGameCounter ==4)
            {
                MultiplayerCheckBidWon(false);
                return false;
            }
            else 
            {
                if (persistantmanager.instence.multiplayer && check)
                {
                    GameManager.instence.ForRpc.NotrickBidProcess((int)bids[itration].mode, bids[itration].number, bids[itration].rank, bids[itration].bidBy, check);
                }
                else
                {
                    NotrickBidProcess((int)bids[itration].mode, bids[itration].number, bids[itration].rank, bids[itration].bidBy , check);
                }
                    return false;
            }
        }
    }
    public void NotrickBidProcess(int mode, int number, int rank, int bidby , bool check )
    {
        if (check)
        {
            highestCount = itration;
            GameManager.instence.highestBider = highestCount;
            bids[itration].mode = (GameMode)mode;
            bids[itration].rank = rank;
            bids[itration].number = number;
            bids[itration].bidBy = bidby;
            noTrickPlayers[notrickGameCounter] = GameManager.instence.playersTurn;
            notrickGameCounter++;
        }
        temp = GameManager.instence.playersTurn;
        GameManager.instence.FirstNextPlayer();
        GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
        itration = GameManager.instence.playersTurn;

        if (!checkIfPlayerAlreadyInList(itration))
        {
            if (!passed[itration])
            {
                checkwithPreviousPlayer = false;
                MultiPlayerIni(itration,0);
            }
            else
            {
                NoBidMultiProcess((GameMode)mode, false);
            }
        }
        else
        {
            NoBidMultiProcess((GameMode)mode, false);
        }

    }
    public int FindFirstInLine()
    {
        int temp = biddingStarter;
        int first = -1; 
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if(temp == noTrickPlayers[j])
                {
                    first = temp;
                    return first; 
                }
            }
            temp = StepUp(temp);
        }
        return first; 
        
    }
    public int FindSecondInLine()
    {
        int temp = biddingStarter;
        int first = -1;
        int count = 0 ;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (temp == noTrickPlayers[j])
                {
                    if(count == 0)
                    {
                        count++;
                    }else
                    {
                        first = temp;
                        return first;
                    }
                }
            }
            temp = StepUp(temp);
        }
        return first;

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
    public bool checkIfPlayerAlreadyInList(int player)
    {
        foreach (int x in noTrickPlayers)
        {
            if (x == player)
            {
                return true;
            }
        }
        return false;
    }
    public void populateIntWithEmpty()
    {
        for (int x = 0; x < 4; x++)
        {
            noTrickPlayers[x] = -1;
        }
    }
    public void Pass(bool TRUE)
    {
        if (persistantmanager.instence.multiplayer)
        {
            GameManager.instence.ForRpc.Pass(TRUE);
        }
        else
        {
            ProcessForPass(TRUE);
        }
    }
    public void ProcessForPass(bool TRUE)
    {
        if (TRUE == true)
        {
            passed[itration] = true;
            GameManager.instence.playersCanvas[itration].transform.GetChild(3).gameObject.SetActive(true);
            numberOfPlayerPassed++;
        }
        checkwithPreviousPlayer = false;
        
        GameManager.instence.FirstNextPlayer();
        GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
        
        if (!highestBidderDeclared)
        {
            if (GameManager.instence.playersTurn < 3)
            {
                preHighestCount = GameManager.instence.playersTurn + 1;
            }
            else
            {
                preHighestCount = 0;
            }
        }


        itration = GameManager.instence.playersTurn;
       
        if (itration == biddingStarter)
        {
            if (bids[highestCount].number != 0  || notrickGameCounter > 0 )
            {
                MultiplayerCheckBidWon(true);
            }
            else
            {
                Scoring.instence.AddTextinResults("No body chosed to bid");
                Scoring.instence.iAmWinner = false;
                Scoring.instence.GameEnding();
                GameManager.instence.TurnOffPassedSigns();
            }
        }
        else if (bids[highestCount].number != 0 &&(highestCount == itration || passed[itration]))
        {
            if (highestBidderDeclared)
            {
                ProcessForPass(false);
            }
        }
        else
        {
            MultiPlayerIni(itration,0);
        }
    }
    public void switchPanels(int x)
    {
        int i = 0;
        foreach (var panel in panels)
        {
            if (i == x)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
            i++;
        }
    }
    public void MultiplayerCheckBidWon(bool check)
    {
        if (persistantmanager.instence.multiplayer && check)
        {
            GameManager.instence.ForRpc.BidWon((int)bids[highestCount].mode, bids[highestCount].number, bids[highestCount].rank, bids[highestCount].bidBy);
        }
        else
        {
            BidWon((int)bids[highestCount].mode, bids[highestCount].number, bids[highestCount].rank, bids[highestCount].bidBy);
        }
    }
    public void BidWon(int mode, int number, int rank, int bidby)
    {
        bids[itration].mode = (GameMode)mode;
        bids[itration].rank = rank;
        bids[itration].number = number;
        bids[itration].bidBy = bidby;
        GameManager.instence.playersTurn = highestCount;
        GameManager.instence.selectedGameMode = bids[highestCount].mode;
        GameManager.instence.playersTurnIndicator(GameManager.instence.playersTurn);
        selectionTP.instence.OnStart(bids[highestCount].mode);
        Debug.Log(mode);
        if (mode < 5)
        {
            MultiPlayerIni(highestCount, 2); 
            //switchPanels(2);
        }
        else
        {
            switchPanels(4);
            
        }
        GameManager.instence.TurnOffPassedSigns();
        if (persistantmanager.instence.multiplayer)
        {
            //GameManager.instence.ForRpc.DisplayMessageToothers(persistantmanager.instence.players[highestCount].name + " Won The Bid");
            if (GameManager.instence.selectedGameMode == GameMode.noTrickGame || GameManager.instence.selectedGameMode == GameMode.noTrickGameFaceUp || GameManager.instence.selectedGameMode == GameMode.TrickGame1)
            {
                persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[highestCount].name + Languages.instence.GetText("bidWon"), null, 0);
            }
            else
            {
                persistantmanager.instence.PopUpWakeUp(persistantmanager.instence.players[highestCount].name + Languages.instence.GetText("waitForPartnerAce"), null, 0);
            }
        }
        else
        {
            persistantmanager.instence.PopUpWakeUp("Player " + (highestCount + 1) + " Won The Bid", null, 0);
        }
    }
    public void DisplayBidsInlog()
    {
        foreach(var bid in bids)
        {
            Debug.Log("Number" +bid.rank );
            Debug.Log("Mode" + bid.mode);
            Debug.Log("number" + bid.number);
            Debug.Log("------------------");
        }
    }
}
