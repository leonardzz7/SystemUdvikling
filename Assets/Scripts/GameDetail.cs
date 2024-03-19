using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Round
{
    public List<CardDescription> CardsPlayed { get; set; }
    public bool HeartsBroken { get; set; }
    public int WinnerIndex { get; set; }

    public Round(List<CardDescription> cardsPlayed, bool heartsBroken, int winnerIndex)
    {
        CardsPlayed = cardsPlayed;
        HeartsBroken = heartsBroken;
        WinnerIndex = winnerIndex;
    }
}


public class PlayerDetails
{

    public int score;
    public string name;
    public int tricksTaken;
    public PlayerDetails( string nameval)
    {
      
        name = nameval;
    }

}




public class GameDetail : MonoBehaviour
{
    public static GameDetail Instance;

    public List<Round> roundDetails = new List<Round>();
    public List<PlayerDetails> playerDetails = new List<PlayerDetails>();

    public int roundNumber=1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }



    }


    private void Start()
    {
        InitializePlayers();
    }


    void InitializePlayers()
    {
        for (int i=0;i< DeckSpawner.Instance.GetHeartPlayers().Count;i++)
        {
            var info = DeckSpawner.Instance.GetHeartPlayers()[i];

            PlayerDetails player = new PlayerDetails( info.name);
            playerDetails.Add(player);
        }



    }

}