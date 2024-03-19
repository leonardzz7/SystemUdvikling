using System;
using UnityEngine;
using UnityEngine.Events;

public class InOuts : MonoBehaviour
{
    public static InOuts instence;
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
            if (GameManager.instence.playersTurn != persistantmanager.instence.pNoNow)
            {
                return;
            }
        }
        //#if UNITY_EDITOR
        if (Application.isEditor)
        {
            if (GameManager.instence.state == GameManager.State.PLayable)
            {
                if ((Input.GetMouseButton(0) && !GameManager.instence.exchangAbleReady) || (Input.GetMouseButtonDown(0) && GameManager.instence.exchangAbleReady))
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

                    if (DoubleClick() && !GameManager.instence.exchangAbleReady)
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
            if (GameManager.instence.state == GameManager.State.PLayable)
            {
                if ((Input.touchCount > 0 && !GameManager.instence.exchangAbleReady) || (Input.GetTouch(0).phase == TouchPhase.Began && GameManager.instence.exchangAbleReady))
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

                    if (DoubleClick() && !GameManager.instence.exchangAbleReady)
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
    public void SelectSepcificCard(GameObject hit)
    {
        GameManager.instence.PMoveCard(hit.transform.gameObject, GameManager.instence.playersCanvas[GameManager.instence.playersTurn].transform.GetChild(2).gameObject, 0, GameManager.instence.playersTurn);
        GameManager.instence.specificCard = hit.transform.GetComponent<Card>();
        GameManager.instence.selectSpecificCard = false;
    }
    public void interProcessForSelect(RaycastHit hit)
    {
        if (hit.transform.parent.name == GameManager.instence.playersTurn.ToString())
        {
            if (GameManager.instence.selectSpecificCard)
            {
                if (hit.transform.gameObject.tag == "Card")
                {

                    if (persistantmanager.instence.multiplayer)
                    {
                        GameManager.instence.ForRpc.DisplayMessageToothers(persistantmanager.instence.players[GameManager.instence.playersTurn].name + "has selected a card for partner ace");
                        GameManager.instence.ForRpc.SelectSepcificCard(hit.transform.GetComponent<Card>());
                    }
                    else
                    {
                        GameManager.instence.PMoveCard(hit.transform.gameObject, GameManager.instence.playersCanvas[GameManager.instence.playersTurn].transform.GetChild(2).gameObject, 0, GameManager.instence.playersTurn);
                        GameManager.instence.specificCard = hit.transform.GetComponent<Card>();
                        GameManager.instence.selectSpecificCard = false;
                    }
                }
            }
            else if (GameManager.instence.exchangAbleReady)
            {
                if (hit.transform.gameObject.tag == "Card")
                {
                    if (CheckIfCardAlreadySelected(hit.transform.gameObject))
                    {
                        hit.transform.LeanScale(new Vector3(150, 150, 150), 0.1f);
                        GameManager.instence.cardsToExchange.Remove(hit.transform.GetComponent<Card>());
                        GameManager.instence.vipSelectedCount--;
                        GameManager.instence.exchangePanel.SetActive(false);
                    }
                    else if (GameManager.instence.vipSelectedCount < 3)
                    {
                        hit.transform.LeanScale(new Vector3(200, 200, 200), 0.1f);
                        GameManager.instence.cardsToExchange.Add(hit.transform.GetComponent<Card>());
                        GameManager.instence.vipSelectedCount++;
                        if (GameManager.instence.vipSelectedCount == 3) GameManager.instence.exchangePanel.SetActive(true);
                    }
                }
            }
            else if (GameManager.instence.numberOfTurns == 0 || GameManager.instence.possibleSpecificCardPlay && GameManager.instence.specificCard == hit.transform.GetComponent<Card>() || GameManager.instence.cardtypeFortheRound == Deck.CardType.Joker || GameManager.instence.cardtypeFortheRound == hit.transform.GetComponent<Card>().type && !GameManager.instence.posiblePartnerAce || (GameManager.instence.posiblePartnerAce && hit.transform.GetComponent<Card>().cardNumber == 14 && hit.transform.GetComponent<Card>().type == GameManager.instence.partnerAce) || GameManager.instence.posibleTrumpNotAvailable)
            {
                if (tempCardTransform != null && tempCardTransform != hit.transform)
                {
                    tempCardTransform.LeanScale(new Vector3(150, 150, 150), 0.2f);
                    tempCardTransform.transform.Rotate(0, -0.12f, 0, Space.Self);
                }
                if (hit.transform.gameObject.tag == "Card")
                {
                    hit.transform.LeanScale(new Vector3(200, 200, 200), 0.1f);
                    hit.transform.Rotate(0, 0f, 0, Space.Self);
                    GameManager.instence.selectedCard = hit.transform.GetComponent<Card>();
                    tempCardTransform = hit.transform;
                }
            }
        }
    }
    public bool CheckIfCardAlreadySelected(GameObject hit)
    {
        foreach (Card var in GameManager.instence.cardsToExchange)
        {
            if (hit.transform.GetComponent<Card>() == var)
            {
                return true;
            }
        }
        return false;
    }
    private void CheckSwipe()
    {
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;

        float deltaY = fingerDown.y - fingerUp.y;
        if (Mathf.Abs(deltaY) > this.swipeThreshold)
        {

            if (deltaY > 0 && GameManager.instence.playersTurn == 0)
            {
                PlayCardMultiplayerToo();
            }
            if (deltaY < 0 && GameManager.instence.playersTurn == 2)
            {
                PlayCardMultiplayerToo();
            }
        }
        if (Mathf.Abs(deltaX) > this.swipeThreshold)
        {

            if (deltaX > 0 && GameManager.instence.playersTurn == 1)
            {
                PlayCardMultiplayerToo();
            }
            if (deltaX < 0 && GameManager.instence.playersTurn == 3)
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
        if (persistantmanager.instence.multiplayer)
        {
            GameManager.instence.ForRpc.PlayCard(GameManager.instence.selectedCard);
        }
        else
        {
            GameManager.instence.PlayCard(GameManager.instence.selectedCard);
        }
    }

}

