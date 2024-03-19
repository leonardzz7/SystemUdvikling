using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class HeartPlayer
{
    public string name;
    public List<CardDescription> playerHand = new List<CardDescription>();
    public Transform playerHandPos;
    public bool isUp;
    public bool isRotate;
    public List<CardDescription> selectedCards = new List<CardDescription>();

}



public class DeckSpawner : MonoBehaviour
{
     public Transform gameHandFinishPos;
    [SerializeField] Text gameDescriptionText;

    public static DeckSpawner Instance;


    [SerializeField] List<HeartPlayer> players;


    [SerializeField] CardDescription cardPrefab;
    [SerializeField] Transform parent;

    //[SerializeField] Transform player1Parent;

    //[SerializeField] Transform player2Parent;

    // [SerializeField] Transform player3Parent;

    // [SerializeField] Transform player4Parent;



    [SerializeField] List<Sprite> club;
    [SerializeField] List<Sprite> diamond;
    [SerializeField] List<Sprite> heart;
    [SerializeField] List<Sprite> spade;
    [SerializeField] List<Sprite> jokers;

    public List<CardDescription> cardDeck;

    //public List<CardDescription> player1Hand;
    // public List<CardDescription> player2Hand;
    // public List<CardDescription> player3Hand;
    //  public List<CardDescription> player4Hand;

    public int totalSelected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }


    public void SetGameDesciptionText(string val)
    {
        gameDescriptionText.text = val;

    }

    public List<HeartPlayer> GetHeartPlayers()
    {
        return players;
    }

    private void Start()
    {
      

    }

    void SpawnClubs()
    {
        for (int i = 0; i < club.Count; i++)
        {
            var card = Instantiate(cardPrefab, parent);
            if (i == 0)

                card.SetCardDescription(Deck.CardType.Club, "Ace");
            else if (i > 0 && i < 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Club, val.ToString());
            }
            else if (i == 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Club, "Queen");
            }
            else if (i == 11)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Club, "King");
            }
            else if (i == 12)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Club, "Joker");
            }

            card.SetFrontImage(club[i]);
            cardDeck.Add(card);
        }
    }

    void SpawnDiamonds()
    {

        for (int i = 0; i < diamond.Count; i++)
        {
            var card = Instantiate(cardPrefab, parent);
            if (i == 0)
                card.SetCardDescription(Deck.CardType.Diamond, "Ace");
            else if (i > 0 && i < 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Diamond, val.ToString());
            }
            else if (i == 10)
            {
                int val = i + 1;

                card.SetCardDescription(Deck.CardType.Diamond, "Queen");
            }
            else if (i == 11)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Diamond, "King");
            }
            else if (i == 12)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Diamond, "Joker");
            }

            card.SetFrontImage(diamond[i]);
            cardDeck.Add(card);
        }

    }


    void SpawnHeart()
    {
        for (int i = 0; i < heart.Count; i++)
        {
            var card = Instantiate(cardPrefab, parent);
            if (i == 0)
                card.SetCardDescription(Deck.CardType.Heart, "Ace");
            else if (i > 0 && i < 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Heart, val.ToString());
            }
            else if (i == 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Heart, "Queen");
            }
            else if (i == 11)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Heart, "King");
            }
            else if (i == 12)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Heart, "Joker");
            }

            card.SetFrontImage(heart[i]);
            cardDeck.Add(card);
        }

    }


    void SpawnSpade()
    {
        for (int i = 0; i < spade.Count; i++)
        {
            var card = Instantiate(cardPrefab, parent);
            if (i == 0)
                card.SetCardDescription(Deck.CardType.Spade, "Ace");
            else if (i > 0 && i < 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Spade, val.ToString());
            }
            else if (i == 10)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Spade, "Queen");
            }
            else if (i == 11)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Spade, "King");
            }
            else if (i == 12)
            {
                int val = i + 1;
                card.SetCardDescription(Deck.CardType.Spade, "Joker");
            }

            card.SetFrontImage(spade[i]);
            cardDeck.Add(card);

        }

    }
    public void SpawnCards()
    {
        cardDeck = new List<CardDescription>();
        SpawnSpade();
        SpawnHeart();
        SpawnDiamonds();
        SpawnClubs();
       
        Shuffle();

    }


    public void Shuffle()
    {


        System.Random rng = new System.Random();
        int n = cardDeck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardDescription card = cardDeck[k];

        

            cardDeck[k] = cardDeck[n];
            cardDeck[n] = card;
        }

        //  ShuffleCard(cardDeck);
      // DistributeCards();


        //
          StartCoroutine(PlayCardShuffleAnimation(cardDeck));
    }


   IEnumerator PlayCardShuffleAnimation(List<CardDescription> cards)
    {
        int i = 0;
        bool playingAnimation = false ;

        while (i < 5)
        {
            playingAnimation = true;

            var card = cards[i];
            Transform orgPos = card.transform;

            card.transform.DOLocalMove(new Vector3(card.transform.localPosition.x, -200, card.transform.localPosition.z), 0.5f).OnComplete(() =>
            {
                card.transform.DOLocalMove(new Vector3(card.transform.localPosition.x,0, card.transform.localPosition.z), 0.5f).OnComplete(() =>
                {
                    i++;
                    playingAnimation = false ;
                  
                });

            });

            yield return null;
        }

      
            Debug.Log("Shuffled");
        Invoke("DistributeCards", 0.5f);

           // DistributeCards();
        
      

    }

    void ShuffleCard(List<CardDescription> cardList)
    {
        int i = 0;
        bool playingAnimation;

        while (i < cardList.Count)
        {
            var card = cardList[i];
            Transform orgPos = card.transform;

            card.transform.DOLocalMove(new Vector3(card.transform.localPosition.x, -200, card.transform.localPosition.z), 0.5f).OnComplete(() =>
            {
                card.transform.DOLocalMove(orgPos.localPosition, 0.5f).OnComplete(()=>
                {
                    i++;

                });
             
            });
        }
       
    }

    public List<CardDescription> SortCards(List<CardDescription> hand)
    {
        // Sort the cards by rank and suit using LINQ
        var sortedHand = hand.OrderBy(card => card.rank).ThenBy(card => card.cardType).ToList();
      
        return sortedHand;
    }


    public void DistributeCards()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerHand = Deal();
            players[i].playerHand=SortCards(players[i].playerHand);


            SetPlayerforCards(players[i].playerHand, i);

            SetParentofPlayerCards(players[i].playerHand, players[i].playerHandPos, players[i].isRotate, 0, players[i].isUp, false);

        }


    

    }

    int posVal;
    bool cardDistributed;

    void SetParentofPlayerCards(List<CardDescription> list, Transform parent, bool rotate, int index, bool isUp, bool passingCards)
    {
        if (index < list.Count)
        {

            cardMoving = true;
            Vector3 pos;
            posVal += 20;
            if (rotate)
            {
                list[index].transform.rotation = Quaternion.Euler(0, 0, 90);
                if (isUp)
                    pos = new Vector3(parent.position.x, parent.position.y - posVal, parent.position.z);
                else
                    pos = new Vector3(parent.position.x, parent.position.y + posVal, parent.position.z);

            }
            else
            {
                pos = new Vector3(parent.position.x + posVal, parent.position.y, parent.position.z);

            }
           

            list[index].transform.DOMove(pos, 0.5f).OnComplete(() =>
            {
                SetParentofCards(list[index].transform, parent);
                cardMoving = false;


                index++;


                if (index == list.Count)
                {
                    if (!cardDistributed && !passingCards)
                    {
                        cardDistributed = true;
                        SetGameDesciptionText("Select 3 Cards to Pass");
                        Debug.Log("Cards Distributed");
                    }
                  // 

                    for (int i = 0; i < list.Count; i++)
                    {

                        ShowCards();

                    }

                }



                SetParentofPlayerCards(list, parent, rotate, index, isUp, passingCards);
            });


        }
        else
        {
            if (passingCards)
            {
              
                HeartGameManager.Instance.passingVal++;

                if (HeartGameManager.Instance.passingVal == 3)
                {
                    HeartGameManager.passCardsCompleteAction?.Invoke();
                    Debug.Log("Passing Completed");
                }
               

                ClearCards(list);

            }
        }


    }


    void ClearCards(List<CardDescription> cards)
    {

        cards.Clear();

    }

    void AttachTheLastToParent(List<CardDescription> cards,Transform parent)
    {

        for (int i = 0; i < cards.Count; i++){


            cards[i].transform.SetParent(parent);
            AlignCards(parent);
        }
    }

    void SetParentofCards(Transform card, Transform parent)
    {
        card.SetParent(parent);
        AlignCards(parent);

    }

    public List<CardDescription> Deal()
    {
        List<CardDescription> hand = new List<CardDescription>();
        for (int i = 0; i < 13; i++)
        {
            hand.Add(cardDeck[0]);
            cardDeck.RemoveAt(0);
        }
        return hand;
    }

    bool cardMoving;

    public IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.localPosition;
        while (elapsedTime < seconds)
        {

            cardMoving = true;
            objectToMove.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        cardMoving = false;
    }



    float shuffleSpeed, shuffleRotationSpeed = 10;
    IEnumerator ShuffleCardsAnimation(List<CardDescription> cardTransforms)
    {
        // Shuffle the cards by moving them to random positions and rotating them randomly.
        for (int i = 0; i < cardTransforms.Count; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f));

            while (cardTransforms[i].gameObject.transform.position != randomPosition || cardTransforms[i].gameObject.transform.rotation != randomRotation)
            {
                cardTransforms[i].gameObject.transform.position = Vector3.MoveTowards(cardTransforms[i].gameObject.transform.position, randomPosition, shuffleSpeed * Time.deltaTime);
                cardTransforms[i].gameObject.transform.rotation = Quaternion.RotateTowards(cardTransforms[i].gameObject.transform.rotation, randomRotation, shuffleRotationSpeed * Time.deltaTime);

                yield return null;
            }
        }

        // Return the cards to their original positions and rotations.
        for (int i = 0; i < cardTransforms.Count; i++)
        {
            Vector3 originalPosition = new Vector3(0f, i * 0.1f, 0f);
            Quaternion originalRotation = Quaternion.Euler(0f, 0f, 0f);

            while (cardTransforms[i].gameObject.transform.position != originalPosition || cardTransforms[i].gameObject.transform.rotation != originalRotation)
            {
                cardTransforms[i].gameObject.transform.position = Vector3.MoveTowards(cardTransforms[i].gameObject.transform.position, originalPosition, shuffleSpeed * Time.deltaTime);
                cardTransforms[i].gameObject.transform.rotation = Quaternion.RotateTowards(cardTransforms[i].gameObject.transform.rotation, originalRotation, shuffleRotationSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }


    float rotationOnShowHand = 50;
    float originalScale;
    public void ShowCards()
    {
      

        for (int i = 0; i < players[0].playerHand.Count; i++)
        {
            players[0].playerHand[i].ShowCard();

            players[0].playerHand[i].gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        }

        SelectCardsForPlayers();
    }




    public void AddSelectedCards(List<CardDescription> playerHand, List<CardDescription> selectedCards)
    {
        playerHand.AddRange(selectedCards);
    }




    public void RemoveSelectedCards(List<CardDescription> playerHand, List<CardDescription> selectedCards)
    {
        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (playerHand.Contains(selectedCards[i]))
                playerHand.Remove(selectedCards[i]);
        }
        //  selectedCards.Clear();
    }


    void ResetCards(List<CardDescription> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].HideCard();
            cards[i].gameObject.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
            cards[i].gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void PassCards(int currentIndex, int nextIndex)
    {
        //  gameDescriptionText.text = "";

        // SetLayout(true, nextIndex);

        //if (currentIndex == 0)
        //{

        //    for (int i = 0; i < players[currentIndex].playerHand.Count; i++)
        //    {
        //        if (players[currentIndex].playerHand[i].selected)
        //        {
        //            players[currentIndex].selectedCards.Add(players[currentIndex].playerHand[i]);
        //        }
        //    }


        //}



        AddSelectedCards(players[nextIndex].playerHand, players[currentIndex].selectedCards);

        SetPlayerforCards(players[nextIndex].playerHand, nextIndex);

        RemoveSelectedCards(players[currentIndex].playerHand, players[currentIndex].selectedCards);

        ResetCards(players[currentIndex].selectedCards);

        if (nextIndex == 0)
        {
            for(int i=0;i< players[currentIndex].selectedCards.Count; i++)
            {
                var val = players[currentIndex].selectedCards[i];
                val.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            }
        }

        SetParentofPlayerCards(players[currentIndex].selectedCards, players[nextIndex].playerHandPos, players[nextIndex].isRotate, 0, players[nextIndex].isUp, true);


    }


    void SetPlayerforCards(List<CardDescription> cards, int index)
    {

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].player = index;
           // SetParentofCards(cards[i].transform, players[index].playerHandPos);
        }
    }

    void AlignCards(Transform card)
    {
        // card.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = true;
        //  players[next].playerHandPos.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

    public void SetLayout(bool status, int index)
    {
        if(status)
        players[index].playerHandPos.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = true;
        else
        {
            players[index].playerHandPos.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }
    }



    public List<CardDescription> SelectRandomCards(List<CardDescription> hand)
    {
        List<CardDescription> selectedCards = new List<CardDescription>();

        System.Random rng = new System.Random();

        // Ensure the hand has at least 3 cards
        if (hand.Count >= 3)
        {
            // Select 3 random indices from the hand
            HashSet<int> indices = new HashSet<int>();
            while (indices.Count < 3)
            {
                indices.Add(rng.Next(hand.Count));
            }

            // Add the corresponding cards to the selectedCards list
            foreach (int index in indices)
            {
                //hand[index].selected = true;
                selectedCards.Add(hand[index]);
            }
        }

        return selectedCards;
    }


    void SelectCardsForPlayers()
    {

        for (int i = 1; i < players.Count; i++)
        {
            players[i].selectedCards = SelectRandomCards(players[i].playerHand);
        }
    }



    public CardDescription ChecktheLeadingCard()
    {
        CardDescription card = null;

        for (int i = 0; i < players.Count; i++)
        {

            for (int j = 0; j < players[i].playerHand.Count; j++)
            {
                if (players[i].playerHand[j].cardType == Deck.CardType.Club && players[i].playerHand[j].rank == "2")
                {
                    card = players[i].playerHand[j];
                    HeartGameManager.Instance.CurrentPlayerIndex = i;
                    break;
                }
            }
        }
        return card;
    }



    public void MoveLeadingCardAndStartNextTurn(CardDescription card, int i)
    {
        //Move the Leading Card 

        players[i].playerHand.Remove(card);

        HeartGameManager.Instance.gameHand.Add(card);

        // Moving dat card 
        card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        card.ShowCard();
        card.transform.DOMove(parent.transform.position, 0.5f).OnComplete(() =>
        {
            HeartGameManager.Instance.currentTurnValue++;

            HeartGameManager.Instance.CurrentPlayerIndex++;

            if (HeartGameManager.Instance.CurrentPlayerIndex > 3)
            {
                HeartGameManager.Instance.CurrentPlayerIndex = 0;
            }
            StartTurns();

        });

        HeartGameManager.Instance.startingPlayerIndex = i;


    }


    int turnCompletedVal;
   

    public void Player1CardSelection()
    {
        TurnOffCards(players[0].playerHand);
    }

    CardDescription StartPlayerTurnAI(int i)
    {

        return SelectCardToPlayAI((players[i]).playerHand,HeartGameManager.Instance.leadingCard.cardType,HeartGameManager.Instance.heartbroken);
    }

  

    void TurnOffCards(List<CardDescription> cards)
    {
        for(int i=0;i< cards.Count; i++)
        {
            cards[i].TurnOffCard();
        }

    }

    void TurnOnCards(List<CardDescription> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].TurnOnCard();
        }

    }


    public CardDescription SelectCardToPlayAI(List<CardDescription> hand, Deck.CardType leadingSuit, bool heartsBroken)
    {
        // Check if the player only has cards of the leading suit
        List<CardDescription> matchingCards = hand.FindAll(card => card.cardType == leadingSuit);
        if (matchingCards.Count > 0)
        {
            // Play the highest card of the leading suit
            CardDescription highestCard = matchingCards.OrderByDescending(card => GetCardValue(card, heartsBroken)).First();
            hand.Remove(highestCard);
            return highestCard;
        }
        else
        {
            // Check if the player has any hearts
            List<CardDescription> hearts = hand.FindAll(card => card.cardType == Deck.CardType.Heart);
            if (hearts.Count > 0)
            {
                // Play the lowest heart
                CardDescription lowestHeart = hearts.OrderBy(card => GetCardValue(card, heartsBroken)).First();
                HeartGameManager.Instance.heartbroken = true;
                hand.Remove(lowestHeart);
                return lowestHeart;
            }
            else if (heartsBroken)
            {
                // Play the lowest card of any suit
                CardDescription lowestCard = hand.OrderBy(card => GetCardValue(card, heartsBroken)).First();
                hand.Remove(lowestCard);
                return lowestCard;
            }
            else
            {
                // Play the lowest non-heart card
                List<CardDescription> nonHearts = hand.FindAll(card => card.cardType != Deck.CardType.Heart);
                CardDescription lowestNonHeart = nonHearts.OrderBy(card => GetCardValue(card, heartsBroken)).First();
                hand.Remove(lowestNonHeart);
                return lowestNonHeart;
            }
        }
    }


    public void SelectCardToPlay(List<CardDescription> hand, Deck.CardType leadingSuit, bool heartsBroken)
    {
        // Check if the player only has cards of the leading suit
        List<CardDescription> matchingCards = hand.FindAll(card => card.cardType == leadingSuit);
        if (matchingCards.Count > 0)
        {
            // Select all the matching Cards 
            TurnOnCards(matchingCards);
           
        }
        else
        {
            // Check if the player has any hearts
            List<CardDescription> hearts = hand.FindAll(card => card.cardType == Deck.CardType.Heart);
            if (hearts.Count > 0)
            {
                // Play the lowest heart
                CardDescription lowestHeart = hearts.OrderBy(card => GetCardValue(card, heartsBroken)).First();
                HeartGameManager.Instance.heartbroken = true;
                lowestHeart.TurnOnCard();
            }
            else if (heartsBroken)
            {
                // Play the lowest card of any suit
                CardDescription lowestCard = hand.OrderBy(card => GetCardValue(card, heartsBroken)).First();
                lowestCard.TurnOnCard();
            }
            else
            {
                // Play the lowest non-heart card
                List<CardDescription> nonHearts = hand.FindAll(card => card.cardType != Deck.CardType.Heart);
                TurnOnCards(nonHearts);
            }
        }
    }





    public int GetCardValue(CardDescription card, bool heartsBroken)
    {
        string rank = card.GetRank();
        switch (rank)
        {
            case "2":
                return 1;
            case "3":
                return 2;
            case "4":
                return 3;
            case "5":
                return 4;
            case "6":
                return 5;
            case "7":
                return 6;
            case "8":
                return 7;
            case "9":
                return 8;
            case "10":
                return 9;
            case "Jack":
                return 10;
            case "Queen":
                return 11;
            case "King":
                return 12;
            case "Ace":
                return 13;
            default:
                return 0;
        }


    }


    public void StartTurns()
    {
        if (HeartGameManager.Instance.currentTurnValue <= 3)
        {
            var index = HeartGameManager.Instance.CurrentPlayerIndex;
          

            if (index == 0)
            {
                Player1CardSelection();
                SelectCardToPlay((players[index]).playerHand, HeartGameManager.Instance.leadingCard.cardType, HeartGameManager.Instance.heartbroken);

            }
            else
            {
                TurnOffCards(players[0].playerHand);
                var card = StartPlayerTurnAI(index);
                players[index].playerHand.Remove(card);
                HeartGameManager.Instance.gameHand.Add(card);

                // Moving dat card 
                card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                card.ShowCard();
                card.transform.DOMove(parent.transform.position, 0.5f).OnComplete(() =>
                {
                    HeartGameManager.Instance.CurrentPlayerIndex++;
                    if (HeartGameManager.Instance.CurrentPlayerIndex > 3)
                    {
                        HeartGameManager.Instance.CurrentPlayerIndex = 0;
                    }

                    HeartGameManager.Instance.currentTurnValue++;

                    StartTurns();
                });

            }

        }

        else
        {
            HeartGameManager.Instance.turnsCompleteAction?.Invoke();
        }


    }



    CardDescription playerTurnCard;

    public void SetPlayerTurnCard(CardDescription player)
    {
        playerTurnCard = player;


        players[0].playerHand.Remove(playerTurnCard);
        HeartGameManager.Instance.gameHand.Add(playerTurnCard);
        playerTurnCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);


        if (playerTurnCard.cardType == Deck.CardType.Heart)
        {
            HeartGameManager.Instance.heartbroken = true;
        }
        playerTurnCard.ShowCard();
        playerTurnCard.transform.DOMove(parent.transform.position, 0.5f).OnComplete(() =>
        {
            HeartGameManager.Instance.currentTurnValue++;
            HeartGameManager.Instance.CurrentPlayerIndex++;

            StartTurns();

        });



    }












    public int CalculatePlayerScore(int playerIndex, int[] playerTrickPoints)
    {
        int score = 0;

        // Add up the points from the player's trick cards.
        for (int i = 0; i < 13; i++)
        {
            int cardValue = playerTrickPoints[(playerIndex + i) % 4];
            if (cardValue == 1)
            {
                score++;
            }
            else if (cardValue == 13)
            {
                score += 13;
            }
            else if (cardValue == 10 || cardValue == 11 || cardValue == 12)
            {
                score += 1;
            }
        }

        // Check if the player has taken any penalty cards (i.e., the Queen of Spades or any hearts).
        for (int i = 0; i < 13; i++)
        {
            int cardValue = playerTrickPoints[(playerIndex + i) % 4];
            if (cardValue == 12)
            {
                score += 13;
            }
            else if (cardValue == 1)
            {
                score++;
            }
        }

        return score;
    }










}
