using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeartGameManager : MonoBehaviour
{

    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject gamePanel;

    [SerializeField] Text roundNumberText;

    [SerializeField] Text winnerText;

    [SerializeField] List<Text> playerNameListText;

    [SerializeField] List<Text> playerScoreListText;

    [SerializeField] List<Text> playerNameListTextScoreScreen;

    [SerializeField] List<Text> playerScoreListTextScoreScreen;


    [SerializeField] GameObject playerScores;


    
    public int currentRoundNumber, totalRoundNumbers;
    public static Action cardsSelectedAction;
    public static Action passCardsCompleteAction;

    public Action turnsCompleteAction;


    public Action roundCompleted;


    public bool selection;

    public Round round;

    public List<CardDescription> cardsPlayed = new List<CardDescription>();
    public bool heartbroken;

    public static HeartGameManager Instance;

    public List<CardDescription> gameHand = new List<CardDescription>();
    public CardDescription leadingCard;

    public int startingPlayerIndex;



    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");

    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //  DontDestroyOnLoad(this);
        }



    }
    private void Start()
    {
        //Initialize();
        //InitializePlayerNames(playerNameListText);


    }

    void UpdatePlayerScores(List<Text> playerScoreListText)
    {

        for (int i = 0; i < GameDetail.Instance.playerDetails.Count; i++)
        {

            var val = GameDetail.Instance.playerDetails[i].score;
            playerScoreListText[i].text = val.ToString();
        }
    }
    void InitializePlayerNames(List<Text> playerNameListText)
    {
        for (int i = 0; i < DeckSpawner.Instance.GetHeartPlayers().Count; i++)
        {
            var val = DeckSpawner.Instance.GetHeartPlayers()[i].name;
            playerNameListText[i].text = val;
        }
    }

    public void Initialize()
    {
        //DeckSpawner.Instance.SpawnCards();
        //cardsSelectedAction += PassCards;
        //heartPlayersList = DeckSpawner.Instance.GetHeartPlayers();

        //totalRoundNumbers = 52 / heartPlayersList.Count;

        //turnsCompleteAction += TurnCompletedMethod;
        //passCardsCompleteAction += StartThePlay;
        //roundNumber = GameDetail.Instance.roundNumber;
        //roundNumberText.text = roundNumber.ToString();

        //rounds = GameDetail.Instance.roundDetails;

    }


    List<HeartPlayer> heartPlayersList;


    public int roundNumber;

    public void PassCards()
    {



        int passDirection = 1;

        // Rotate passing direction every round
        if (roundNumber % 4 == 0)
        {
            passDirection = 1;
        }
        else if (roundNumber % 4 == 1)
        {
            passDirection = 2;
        }
        else if (roundNumber % 4 == 2)
        {
            passDirection = 3;
        }
        else
        {
            passDirection = 4;
        }
        var players = DeckSpawner.Instance.GetHeartPlayers();

        // Prompt each player to select and pass cards
        for (int i = 0; i < players.Count; i++)
        {

            int nextPlayerIndex = (i + passDirection - 1) % players.Count;
            DeckSpawner.Instance.PassCards(i, nextPlayerIndex);

        }


    }



    private void OnDestroy()
    {
        cardsSelectedAction -= PassCards;

        turnsCompleteAction -= TurnCompletedMethod;
        passCardsCompleteAction -= StartThePlay;

    }


    public int winnerIndex;
    public int trickScore;

    public int gameRound;
    public int currentTurnValue;


    public int CurrentPlayerIndex;
    public CardDescription twoOfClubs;


    public void StartThePlay()
    {


        if (gameRound < 13)
        {

            if (gameRound == 0)
            {
                playerScores.SetActive(true);
                InitializePlayerNames(playerNameListText);
                UpdatePlayerScores(playerScoreListText);


                leadingCard = DeckSpawner.Instance.ChecktheLeadingCard();
                leadingCard.player = CurrentPlayerIndex;
                twoOfClubs = leadingCard;

                DeckSpawner.Instance.MoveLeadingCardAndStartNextTurn(leadingCard, leadingCard.player);

            }
            else
            {
                CurrentPlayerIndex = leadingCard.player;

                if (leadingCard.player != 0)
                {
                    DeckSpawner.Instance.MoveLeadingCardAndStartNextTurn(leadingCard, leadingCard.player);
                }
                else
                {
                    DeckSpawner.Instance.StartTurns();
                }

            }


        }
        else
        {

            GameDetail.Instance.roundNumber++;
            //  gameRound = 0;
            //  passingVal = 0;
            //   selection = false;
            if (GameDetail.Instance.roundNumber > 13)
            {
                scorePanel.SetActive(true);
                gamePanel.SetActive(false);

                InitializePlayerNames(playerNameListTextScoreScreen);
                UpdatePlayerScores(playerScoreListTextScoreScreen);
                winnerText.text = "Congratulations! " + DeckSpawner.Instance.GetHeartPlayers()[CalculateWinner(GameDetail.Instance.playerDetails, heartbroken)].name + " You are the Winner !!!";
            }
            else
            {
                SceneManager.LoadScene("HeartGame");
            }


        }
    }

    public int passingVal;
    private List<Round> rounds;

    void TurnCompletedMethod()
    {
        Round round = new Round(gameHand, heartbroken, CalculateTrickWinner(leadingCard.cardType));
        rounds.Add(round);

        trickScore = CalculateTrickScore(gameHand);
        winnerIndex = CalculateTrickWinner(leadingCard.cardType);
        startingPlayerIndex = winnerIndex;
        GameDetail.Instance.playerDetails[winnerIndex].tricksTaken++;

        GameDetail.Instance.playerDetails[winnerIndex].score += trickScore;
        UpdatePlayerScores(playerScoreListText);

        if (gameRound < 12)
        {
            leadingCard = SelectLeadingCardForOtherPlayers(heartPlayersList[winnerIndex]);
            leadingCard.player = winnerIndex;
        }



        MoveGameCards();


    }

    void MoveGameCards()
    {
        int Count = 0;

        for (int i = 0; i < gameHand.Count; i++)
        {
            gameHand[i].transform.DOMove(DeckSpawner.Instance.gameHandFinishPos.position, 0.5f).OnComplete(() =>
            {
                Count++;
                //  gameHand[i].transform.SetParent(DeckSpawner.Instance.gameHandFinishPos);
                if (Count == 3)
                {
                    currentTurnValue = 0;
                    gameHand.Clear();
                    gameRound++;
                    StartThePlay();

                }
            });

        }


    }
    public void EndRound()
    {
        // Determine winner and add round to list of rounds
        // ...

        Dictionary<int, int> scores = CalculateScores(rounds);
        // Display scores to players
        Console.WriteLine("Current scores:");
        foreach (int playerIndex in scores.Keys)
        {
            Console.WriteLine($"Player {playerIndex}: {scores[playerIndex]}");
        }
        // Check for winner
        int maxScore = scores.Values.Max();
        if (maxScore >= 100)
        {
            int winnerIndex = scores.FirstOrDefault(x => x.Value == maxScore).Key;
            Console.WriteLine($"Player {winnerIndex} wins!");
            // End game or prompt to play again, etc.
            // ...
        }
    }



    public Dictionary<int, int> CalculateScores(List<Round> rounds)
    {
        Dictionary<int, int> scores = new Dictionary<int, int>();
        for (int i = 0; i < rounds.Count; i++)
        {
            Round round = rounds[i];
            Deck.CardType suitLed = round.CardsPlayed[0].cardType;
            bool heartsBroken = round.HeartsBroken;
            int roundScore = 0;
            foreach (CardDescription card in round.CardsPlayed)
            {
                if (card.cardType == Deck.CardType.Heart)
                {
                    roundScore += 1;
                }
                else if (card.cardType == Deck.CardType.Spade && card.rank == "Queen")
                {
                    roundScore += 13;
                }
            }
            int winnerIndex = round.WinnerIndex;
            if (scores.ContainsKey(winnerIndex))
            {
                scores[winnerIndex] += roundScore;
            }
            else
            {
                scores.Add(winnerIndex, roundScore);
            }
            if (i == rounds.Count - 1)
            {
                for (int j = 0; i < scores.Count; j++)
                {
                    if (scores.ContainsKey(j))
                    {

                        int playerScore = scores[j];
                        if (playerScore == 26)
                        {
                            scores[j] = 0;
                        }
                        else
                        {
                            scores[j] += 26;
                        }
                    }
                }
            }
        }
        return scores;
    }


    public int CalculateTrickWinner(Deck.CardType trumpSuit)
    {
        Deck.CardType leadingSuit = gameHand[0].cardType;
        CardDescription highestCard = gameHand[0];
        int highestCardIndex = 0;

        for (int i = 1; i < gameHand.Count; i++)
        {
            CardDescription currentCard = gameHand[i];

            if (currentCard.cardType == leadingSuit && currentCard.GetCardValue() > highestCard.GetCardValue())
            {
                highestCard = currentCard;
                highestCardIndex = i;
            }
            else if (currentCard.cardType == trumpSuit && highestCard.cardType != trumpSuit)
            {
                highestCard = currentCard;
                highestCardIndex = i;
            }
        }

        return highestCardIndex;
    }


    public int CalculateTrickScore(List<CardDescription> trickCards)
    {
        int score = 0;
        foreach (CardDescription card in trickCards)
        {
            if (card.cardType == Deck.CardType.Heart)
            {
                score++;
            }
            else if (card.cardType == Deck.CardType.Spade && card.GetRank() == "Queen")
            {
                score += 13;
            }
        }
        return score;
    }



    public CardDescription SelectLeadingCardForOtherPlayers(HeartPlayer player)
    {
        // Check if player has any cards of the suit that was led in the previous trick
        if (gameHand != null && gameHand.Count > 0)
        {
            Deck.CardType previousSuit = gameHand[0].cardType;
            List<CardDescription> cardsInSuit = player.playerHand.Where(c => c.cardType == previousSuit).ToList();
            if (cardsInSuit.Count > 0)
            {
                // Player has at least one card of the suit that was led in the previous trick
                // Select the highest card of that suit
                return cardsInSuit.OrderByDescending(c => c.cardType).First();
            }
        }

        // Player does not have any cards of the suit that was led in the previous trick
        // Select the lowest non-heart card from the player's hand
        List<CardDescription> nonHeartCards = player.playerHand.Where(c => c.cardType != Deck.CardType.Heart).ToList();
        if (nonHeartCards.Count > 0)
        {
            return nonHeartCards.OrderBy(c => c.GetCardValue()).First();
        }

        // Player only has heart cards, select the lowest heart card
        return player.playerHand.OrderBy(c => c.GetCardValue()).First();
    }

    public int CalculateWinner(List<PlayerDetails> playerScores, bool heartsBroken)
    {
        int minScore = 100;
        int lastPlayer = -1;
        int winner = -1;

        // Check if any player has reached or exceeded 100 points.
        for (int i = 0; i < 4; i++)
        {
            if (playerScores[i].score >= 100)
            {
                return i;
            }
        }

        // If no player has reached or exceeded 100 points, find the player with the lowest score.
        for (int i = 0; i < 4; i++)
        {
            if (playerScores[i].score < minScore)
            {
                minScore = playerScores[i].score;
                lastPlayer = i;
            }
        }

        // If hearts have not been broken, the player with the Two of Clubs is the winner.
        if (!heartsBroken)
        {
            // winner = playerScores[lastPlayer].
            winner = twoOfClubs.player;

        }
        else
        {
            // If hearts have been broken, the winner is the player with the lowest score.
            winner = lastPlayer;
        }
        return winner;
    }
}
