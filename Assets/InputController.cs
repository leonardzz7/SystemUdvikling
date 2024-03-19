using System;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public static InputController instence;
    Transform tempCardTransform;
    public int exchangeAbleCount;
    public float swipeThreshold = 100f;
    public float timeThreshold = 0.3f;
    private Vector2 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;

    private void Awake()
    {
        exchangeAbleCount = 0;
        if (instence == null)
        {
            instence = this;
        }
    }
    void Update()
    {
        HandleInputs();
    }
    public void HandleInputs()
    {
        if (persistantmanager.instence.multiplayer)
        {
            if (HeartsGameManager.instence.playersTurn != persistantmanager.instence.pNoNow)
            {
                Debug.Log("HeartsGameManager.instence.playersTurn____________________" + HeartsGameManager.instence.playersTurn);
                return;
            }
        }
        //#if UNITY_EDITOR
        if (Application.isEditor)
        {
            if (HeartsGameManager.instence.state == HeartsGameManager.State.Exchanging)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log(hit.transform.name);
                        if (hit.collider != null)
                        {
                            HeartsGameManager.instence.ForRpc.ExchangeCardSelected(HeartsGameManager.instence.ExchangeCardSelected, Array.IndexOf(CardObjects.instence.cards, hit.collider.gameObject));
                            //HeartsGameManager.instence.ExchangeCardSelected++;
                            //if (HeartsGameManager.instence.ExchangeCardSelected == 3)
                            //{
                            //    HeartsGameManager.instence.ExchangeCardSelected = 0;
                            //    HeartsGameManager.instence.ExchangeButton.SetActive(true);
                            //    //make rpc call
                            //    //multiplayer 133
                            //}
                        }
                    }
                }
                Debug.Log("INSIDE INPUT");
            }
                if (HeartsGameManager.instence.state == HeartsGameManager.State.PLayable)
                {
                    if ((Input.GetMouseButton(0)))
                    {
                        Ray ray;
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Debug.Log(hit.transform.name);
                            if (hit.collider != null)
                            {
                                interProcessForSelect(hit);
                            }
                        }

                        if (DoubleClick())
                        {
                            PlayCardMultiplayerToo();
                        }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        this.fingerDown = Input.mousePosition;
                        this.fingerUp = Input.mousePosition;
                        this.fingerDownTime = DateTime.Now;

                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        this.fingerDown = Input.mousePosition;
                        this.fingerUpTime = DateTime.Now;
                        this.CheckSwipe();

                    }
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            this.fingerDown = touch.position;
                            this.fingerUp = touch.position;
                            this.fingerDownTime = DateTime.Now;
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            this.fingerDown = touch.position;
                            this.fingerUpTime = DateTime.Now;
                            this.CheckSwipe();
                        }
                    }
                }
            }

            if (Application.platform == RuntimePlatform.Android)
            {
            if (HeartsGameManager.instence.state == HeartsGameManager.State.Exchanging)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log(hit.transform.name);
                        if (hit.collider != null)
                        {
                            HeartsGameManager.instence.ForRpc.ExchangeCardSelected(HeartsGameManager.instence.ExchangeCardSelected, Array.IndexOf(CardObjects.instence.cards, hit.collider.gameObject));
                            //HeartsGameManager.instence.ExchangeCardsArray[HeartsGameManager.instence.ExchangeCardSelected] = CardObjects.instence.cards[hit.collider.gameObject.GetComponent<Card>().cardNumber];
                            //HeartsGameManager.instence.ExchangeCardSelected++;
                            //if (HeartsGameManager.instence.ExchangeCardSelected == 3)
                            //{
                            //    HeartsGameManager.instence.ExchangeCardSelected = 0;
                            //    HeartsGameManager.instence.ExchangeButton.SetActive(true);
                            //}
                        }
                    }
                }
                Debug.Log("INSIDE INPUT");
            }
            if (HeartsGameManager.instence.state == HeartsGameManager.State.PLayable)
                {
                    if ((Input.touchCount > 0))
                    {
                        Ray ray;
                        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Debug.Log(hit.transform.name);
                            if (hit.collider != null)
                            {
                                interProcessForSelect(hit);
                            }
                        }

                        if (DoubleClick())
                        {
                            PlayCardMultiplayerToo();
                        }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        this.fingerDown = Input.mousePosition;
                        this.fingerUp = Input.mousePosition;
                        this.fingerDownTime = DateTime.Now;

                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        this.fingerDown = Input.mousePosition;
                        this.fingerUpTime = DateTime.Now;
                        this.CheckSwipe();
                    }
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            this.fingerDown = touch.position;
                            this.fingerUp = touch.position;
                            this.fingerDownTime = DateTime.Now;
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            this.fingerDown = touch.position;
                            this.fingerUpTime = DateTime.Now;
                            this.CheckSwipe();
                        }
                    }
                }
            }
        }
    
    public void interProcessForSelect(RaycastHit hit)
    {
        if (hit.transform.parent.name == HeartsGameManager.instence.playersTurn.ToString())
        {
                if (hit.transform.gameObject.tag == "Card")
                {
                    hit.transform.LeanScale(new Vector3(200, 200, 200), 0.1f);
                    hit.transform.Rotate(0, 0f, 0, Space.Self);
                    HeartsGameManager.instence.selectedCard = hit.transform.GetComponent<Card>();

                }
        }
          
    }
 
    private void CheckSwipe()
    {
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;

        float deltaY = fingerDown.y - fingerUp.y;
        if (Mathf.Abs(deltaY) > this.swipeThreshold)
        {

            if (deltaY > 0 && HeartsGameManager.instence.playersTurn == 0)
            {
                PlayCardMultiplayerToo();
            }
            if (deltaY < 0 && HeartsGameManager.instence.playersTurn == 2)
            {
                PlayCardMultiplayerToo();
            }
        }
        if (Mathf.Abs(deltaX) > this.swipeThreshold)
        {

            if (deltaX > 0 && HeartsGameManager.instence.playersTurn == 1)
            {
                PlayCardMultiplayerToo();
            }
            if (deltaX < 0 && HeartsGameManager.instence.playersTurn == 3)
            {
                PlayCardMultiplayerToo();
            }
        }

        this.fingerUp = this.fingerDown;
    }
    // -------------------------------double click-----------------------------

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    bool DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        return false;
    }

    public void PlayCardMultiplayerToo()
    {
        Debug.Log("cardplayed +++++++++++++++++++++++++++++ ");
        HeartsGameManager.instence.cardplayed++;
        Debug.Log("PlayCardMultiplayerToo() and playerTurn is "+HeartsGameManager.instence.playersTurn);
        Debug.Log("CurrentDominantSuite is " + HeartsGameManager.instence.CurrentDominantSuite.ToString());
        int selectedCardIndex = 0;
        bool played = false;
        while (played == false)
        {
            if (HeartsGameManager.instence.players[HeartsGameManager.instence.playersTurn].transform.childCount!=0)
            HeartsGameManager.instence.selectedCard = HeartsGameManager.instence.players[HeartsGameManager.instence.playersTurn].transform.GetChild(selectedCardIndex).gameObject.GetComponent<Card>();
            else
            {
                //display result
            }
            if (HeartsGameManager.instence.trick.transform.childCount == 0)
            {
                if (HeartsGameManager.instence.CheckIfPlayerHasTwoOfClubs()== HeartsGameManager.instence.playersTurn)
                {
                    HeartsGameManager.instence.selectedCard = CardObjects.instence.cards[4].GetComponent<Card>();
                    
                    HeartsGameManager.instence.leadPlayernumber = HeartsGameManager.instence.playersTurn;
                    HeartsGameManager.instence.leadCard = HeartsGameManager.instence.selectedCard;

                    Debug.Log(" HeartsGameManager.instence.leadPlayernumber " + HeartsGameManager.instence.leadPlayernumber);
                    Debug.Log(" HeartsGameManager.instence.leadPlayercard " + HeartsGameManager.instence.leadCard);
                    Debug.Log("Here0**********************");
                    HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
                    played = true;
                }
                else if (HeartsGameManager.instence.selectedCard.type == Deck.CardType.Heart)
                {
                    if (HeartsGameManager.instence.HeartsBroken == true)
                    {
                        HeartsGameManager.instence.CurrentDominantSuite = HeartsGameManager.instence.selectedCard.type;
                        Debug.Log("1CurrentDominantSuite is " + HeartsGameManager.instence.CurrentDominantSuite.ToString());
                        
                        if(HeartsGameManager.instence.selectedCard.rank> HeartsGameManager.instence.leadCard.rank)
                        {
                            HeartsGameManager.instence.leadCard = HeartsGameManager.instence.selectedCard;
                            HeartsGameManager.instence.leadPlayernumber = HeartsGameManager.instence.playersTurn;
                            Debug.Log(" HeartsGameManager.instence.leadPlayernumber " + HeartsGameManager.instence.leadPlayernumber);
                            Debug.Log(" HeartsGameManager.instence.leadPlayercard " + HeartsGameManager.instence.leadCard);
                        }
                        HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
                        Debug.Log("2CurrentDominantSuite is " + HeartsGameManager.instence.CurrentDominantSuite.ToString());
                        Debug.Log("Here1**********************");
                        played = true;
                    }
                }
                else
                {
                    HeartsGameManager.instence.CurrentDominantSuite = HeartsGameManager.instence.selectedCard.type;
                    
                    HeartsGameManager.instence.leadCard = HeartsGameManager.instence.selectedCard;
                    HeartsGameManager.instence.leadPlayernumber = HeartsGameManager.instence.playersTurn;
                    Debug.Log(" HeartsGameManager.instence.leadPlayernumber " + HeartsGameManager.instence.leadPlayernumber);
                    Debug.Log(" HeartsGameManager.instence.leadPlayercard " + HeartsGameManager.instence.leadCard);

                    HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
                    Debug.Log("Here2**********************");
                    played = true;

                }

            }
            else
            {
                if (HeartsGameManager.instence.CheckIfPlayerHasDominantSuite())
                {
                    if (HeartsGameManager.instence.selectedCard.type == HeartsGameManager.instence.CurrentDominantSuite)
                    {
                       

                        if (HeartsGameManager.instence.selectedCard.rank > HeartsGameManager.instence.leadCard.rank)
                        {
                            HeartsGameManager.instence.leadCard = HeartsGameManager.instence.selectedCard;
                            HeartsGameManager.instence.leadPlayernumber = HeartsGameManager.instence.playersTurn;
                            Debug.Log(" HeartsGameManager.instence.leadPlayernumber " + HeartsGameManager.instence.leadPlayernumber);
                            Debug.Log(" HeartsGameManager.instence.leadPlayercard " + HeartsGameManager.instence.leadCard);
                        }
                        Debug.Log("Here3**********************");
                        HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
                        played = true;
                    }
                }
                else
                {
                    

                    if (HeartsGameManager.instence.selectedCard.rank > HeartsGameManager.instence.leadCard.rank)
                    {
                        HeartsGameManager.instence.leadCard = HeartsGameManager.instence.selectedCard;
                        HeartsGameManager.instence.leadPlayernumber = HeartsGameManager.instence.playersTurn;
                        Debug.Log(" HeartsGameManager.instence.leadPlayernumber " + HeartsGameManager.instence.leadPlayernumber);
                        Debug.Log(" HeartsGameManager.instence.leadPlayercard " + HeartsGameManager.instence.leadCard);
                    }
                    if (HeartsGameManager.instence.selectedCard.type== Deck.CardType.Heart)
                    {
                        HeartsGameManager.instence.HeartsBroken = true;
                    }
                    HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
                    Debug.Log("Here3**********************");
                    played = true;
                }
            }
            selectedCardIndex++;
        }
        //HeartsGameManager.instence.selectedCard = HeartsGameManager.instence.players[persistantmanager.instence.pNoNow].transform.GetChild(0).gameObject.GetComponent<Card>();
        //if (HeartsGameManager.instence.numberOfTurns > 0)
        //{
        //    if (HeartsGameManager.instence.selectedCard.type == HeartsGameManager.instence.CurrentDominantSuite)
        //    {
        //        HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
        //    }
        //    else if (HeartsGameManager.instence.CheckIfPlayerHasDominantSuite())
        //    {
        //        Debug.Log("Player has Suite, Cant play other card");
        //        return;
        //    }
        //    else
        //    {
        //        Debug.Log("Player Played a Different Suite");
        //        HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);
        //    }
        //}
        //else
        //{
        //    if (!HeartsGameManager.instence.HeartsBroken)
        //    {
        //        if (HeartsGameManager.instence.selectedCard.type == Deck.CardType.Heart)
        //        {
        //            Debug.Log("Cant start turn with hearts until hearts are broken");

        //        }
        //    }
        //    HeartsGameManager.instence.ForRpc.PlayCard(HeartsGameManager.instence.selectedCard);

        //}

    }

}

