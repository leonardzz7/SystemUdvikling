using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoring : MonoBehaviour
{
    public static Scoring instence;

    public float[,] playerstrick;
    public float[] totalPlayersScores;

    public GameObject[,] trickText;
    private int NoOfRounds;
    public int roundNo;
    public GameObject textPrefab;
    public Transform[] parentResults;

    // Scoring Points
    public int[] basePoints;
    public int[] extraTricks;
    public int[] notEnoughTricks;

    public int[] pointsModeToNumber;

    public int[,] totalScores;

    public int noOfNotrickWinners;
    public bool[] noTrickWinList;

    public Results result;
    public bool iAmWinner = false;

    private void Awake()
    {
        totalPlayersScores = new float[4];
        if (instence == null)
        {
            instence = this;
        }
    }
    void Start()
    {
        NoOfRounds = 3;
        roundNo = 0;
        playerstrick = new float[4, NoOfRounds + 1];
        totalScores = new int[4, NoOfRounds + 1];
        trickText = new GameObject[4, NoOfRounds + 1];
        result.switchPanels(3);
       
    }

    public void AddTextinResults(string s)
    {
        for (int x = 0; x < 4; x++)
        {
            trickText[x, roundNo] = Instantiate(textPrefab, parentResults[x]);
            trickText[x, roundNo].GetComponent<TextMeshProUGUI>().text = s;

        }
        result.FillGameType(Languages.instence.GetText(GameManager.instence.selectedGameMode.ToString()));
        result.FillPoints("Points : 0");
    }
    public void AnalyseScoreFortheRound(bool check, int player)
    {
        trickText[player, roundNo] = Instantiate(textPrefab, parentResults[player]);
        trickText[player, roundNo].GetComponent<TextMeshProUGUI>().text = GameManager.instence.selectedGameMode + " " + totalScores[player, roundNo] + "";
        totalPlayersScores[player] = (totalPlayersScores[player] + totalScores[player, roundNo]);
        result.totals[player].text = "Total : " + totalPlayersScores[player];
        if (player == persistantmanager.instence.pNoNow)
        {
            result.FillGameType(Languages.instence.GetText(GameManager.instence.selectedGameMode.ToString()));
            result.FillPoints("Points : " + totalScores[player, roundNo]);
        }
    }
    public void GameEnding()
    {
        result.switchPanels(3);
        if (roundNo < NoOfRounds)
        {
            roundNo++;
            StartCoroutine(WaitForWinPage(iAmWinner));
            UiControler.instence.StartCoroutine(UiControler.instence.WaitForResultShow());
            GameManager.instence.ResetDeck();
            
        }
        else
        {
            StartCoroutine(WaitForWinPage(iAmWinner));
            GameManager.instence.state = GameManager.State.Animation;
            UiControler.instence.switchPanels(2);
            roundNo = 0;
            restartButton.SetActive(true);
            // button for restart the game .
        }

    }
    public IEnumerator WaitForWinPage(bool check)
    {
        if (check)
        {
            result.switchPanels(1);
            result.ShowWin();
            GameManager.instence.WinSound();
        }
        else
        {
            result.switchPanels(2);
            result.ShowLose();
            GameManager.instence.lossSound();
        }
        yield return new WaitForSeconds(5);
        result.ResetShow();
        result.switchPanels(0);
    }
    public GameObject restartButton;

    public void GiveScores(bool selectedPartnerAce, int player, int partner, int highestNumber)
    {
        if (selectedPartnerAce)
        {
            int mode = (int)GameManager.instence.selectedGameMode;
            float total = playerstrick[player, roundNo] + playerstrick[partner, roundNo];
            float diference = total - highestNumber;
            int number = highestNumber - 8;
            int scoringNumber = (pointsModeToNumber[mode] * 5) + number;
            Debug.Log(diference);
            if (diference == 0)
            {
                FillingTotalScores(player, partner, basePoints[scoringNumber], selectedPartnerAce);
            }
            else if (diference > 0)
            {
                FillingTotalScores(player, partner, (basePoints[scoringNumber] + (int)Mathf.Abs(diference * extraTricks[scoringNumber])), selectedPartnerAce);
            }
            else
            {
                FillingTotalScores(player, partner, (-(int)Mathf.Abs(diference * notEnoughTricks[scoringNumber])), selectedPartnerAce);
            }

        }
        else
        {
            int mode = (int)GameManager.instence.selectedGameMode;
            float total = playerstrick[player, roundNo];
            float diference = total - highestNumber;
            int number = highestNumber - 8;
            Debug.Log(diference);
            int scoringNumber = (pointsModeToNumber[mode] * 5) + number;
            if (diference == 0)
            {
                FillingTotalScores(player, partner, basePoints[scoringNumber] ,selectedPartnerAce);
            }
            else if (diference > 0)
            {
                FillingTotalScores(player, partner, (basePoints[scoringNumber] + (int)Mathf.Abs(diference * extraTricks[scoringNumber])), selectedPartnerAce);
            }
            else
            {
                FillingTotalScores(player, partner, (-(int)Mathf.Abs(diference * notEnoughTricks[scoringNumber])), selectedPartnerAce);
            }
        }
    }
    public void DeclearWiners()
    {
        if (noOfNotrickWinners == Bidding.instence.notrickGameCounter)
        {
            for (int x = 0; x < 4; x++)
            {
                bool temp = noTrickWinList[x];
                for (int y = 0; y < Bidding.instence.notrickGameCounter; y++)
                {
                    if (x != Bidding.instence.noTrickPlayers[y])
                    {
                        noTrickWinList[x] = false;
                    }
                    else
                    {
                        noTrickWinList[x] = temp;
                        break;
                    }
                }
            }
        }
        else if (noOfNotrickWinners == 1)
        {
            if (Bidding.instence.notrickGameCounter == 2)
            {
                for (int x = 0; x < 4; x++)
                {
                    bool temp = noTrickWinList[x];
                    for (int y = 0; y < Bidding.instence.notrickGameCounter; y++)
                    {
                        if (x != Bidding.instence.noTrickPlayers[y])
                        {
                            noTrickWinList[x] = false;
                        }
                        else
                        {
                            noTrickWinList[x] = temp;
                            break;
                        }
                    }
                }
            }
            else if (Bidding.instence.notrickGameCounter == 3)
            {
                for (int x = 0; x < 4; x++)
                {
                    bool temp = noTrickWinList[x];
                    for (int y = 0; y < Bidding.instence.notrickGameCounter; y++)
                    {
                        if (x != Bidding.instence.noTrickPlayers[y])
                        {
                            noTrickWinList[x] = true;
                        }
                        else
                        {
                            noTrickWinList[x] = temp;
                            break;
                        }
                    }
                }
            }

        }
        else if (noOfNotrickWinners == 0)
        {
            for (int x = 0; x < 4; x++)
            {
                bool temp = noTrickWinList[x];
                for (int y = 0; y < Bidding.instence.notrickGameCounter; y++)
                {
                    if (x != Bidding.instence.noTrickPlayers[y])
                    {
                        noTrickWinList[x] = true;

                    }
                    else
                    {
                        noTrickWinList[x] = false;
                        break;
                    }
                }
            }
        }
    }
    public void GiveScoresNotrick()
    {
        DeclearWiners();
        switch (GameManager.instence.selectedGameMode)
        {
            case Bidding.GameMode.TrickGame1:
                FillingTotalScoresNoTrick(300);
                break;
            case Bidding.GameMode.noTrickGame:
                FillingTotalScoresNoTrick(600);
                break;
            case Bidding.GameMode.noTrickGameFaceUp:
                FillingTotalScoresNoTrick(1200);
                break;
        }
        iAmWinner = noTrickWinList[persistantmanager.instence.pNoNow];
    }
    public void FillingTotalScoresNoTrick(int baseScore)
    {
        switch (Bidding.instence.notrickGameCounter)
        {
            case 1:
                switch (noOfNotrickWinners)
                {
                    case 0:
                        FeedingWinners((baseScore / 3) * 2, baseScore * 2);
                        break;
                    case 1:
                        FeedingWinners(baseScore, baseScore / 3);
                        break;
                }
                break;
            case 2:
                switch (noOfNotrickWinners)
                {
                    case 0:
                        FeedingWinners(baseScore * 2, baseScore * 2);
                        break;
                    case 1:
                        // corect 
                        FeedingWinners(baseScore, baseScore);
                        FeedingZero();
                        break;
                    case 2:
                        FeedingWinners(baseScore, baseScore);
                        break;
                }
                break;
            case 3:
                switch (noOfNotrickWinners)
                {
                    case 0:
                        FeedingWinners(baseScore * 3, baseScore);
                        break;
                    case 1:
                        FeedingWinners(baseScore, baseScore);
                        break;
                    case 2:
                        FeedingWinners(baseScore, baseScore);
                        break;
                    case 3:
                        FeedingWinners(baseScore, baseScore * 3);
                        break;
                }
                break;
            case 4:
                switch (noOfNotrickWinners)
                {
                    case 0:
                        FeedingZeros();
                        break;
                    case 1:
                        FeedingWinners(baseScore * 3, baseScore);
                        break;
                    case 2:
                        FeedingWinners(baseScore, baseScore);
                        break;
                    case 3:
                        FeedingWinners(baseScore, baseScore * 3);
                        break;
                }
                break;
        }
    }
    public void FillUpLists()
    {
        for (int x = 0; x < 4; x++)
        {
            AnalyseScoreFortheRound(noTrickWinList[x], x);
        }
    }
    public void FillUpListsnoTrick()
    {
        for (int x = 0; x < 4; x++)
        {
            AnalyseScoreFortheRound(noTrickWinList[x], x);
        }
    }
    public void FeedingZeros()
    {
        for (int x = 0; x < 4; x++)
        {
            totalScores[x, roundNo] = 0;
        }
    }
    public void FeedingZero()
    {
        for (int x = 0; x < 4; x++)
        {
            bool cache = false;
            for (int y = 0; y < Bidding.instence.notrickGameCounter; y++)
            {
                if (x == Bidding.instence.noTrickPlayers[y])
                {
                    cache = true;
                    //totalScores[x, roundNo] = 0;
                    break;
                }

            }
            if (!cache)
            {
                totalScores[x, roundNo] = 0;
            }
        }
    }
    public void FeedingWinners(int winScore, int loseScore)
    {
        for (int x = 0; x < 4; x++)
        {
            if (noTrickWinList[x])
            {
                totalScores[x, roundNo] = winScore;
            }
            else
            {
                totalScores[x, roundNo] = -loseScore;
            }

        }
    }
    public void FillingTotalScores(int winner1, int winner2, int points , bool selectedpartner)
    {
        int divid = 0;
        if (!selectedpartner)
        {
            divid = points / 3;
        }
        for (int x = 0; x < 4; x++)
        {
            if (x == winner1 || x == winner2)
            {
                totalScores[x, roundNo] = points;
                if (x == persistantmanager.instence.pNoNow)
                {
                    if (totalScores[x, roundNo] < 0)
                    {
                        iAmWinner = false;
                    }
                    else
                    {
                        iAmWinner = true;
                    }
                }
            }
            else
            {
                if (!selectedpartner)
                {
                    totalScores[x, roundNo] = -divid;
                }
                else
                {
                    totalScores[x, roundNo] = -points; 
                }
                if (x == persistantmanager.instence.pNoNow)
                {
                    if (totalScores[x, roundNo] < 0)
                    {
                        iAmWinner = false;
                    }
                    else
                    {
                        iAmWinner = true;
                    }
                }
            }
        }
    }
}
