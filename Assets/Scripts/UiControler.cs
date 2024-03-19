using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class UiControler : MonoBehaviour
{
    public static UiControler instence;
    public GameObject notfication;
    public GameObject[] panels;
    public TextMeshProUGUI partnerAce;
    public TextMeshProUGUI trump;
    public int[] tempPlayerNumbers; 

    public Transform[] detailsPositions;
    public bool detailsIsOn;
    public GameObject detailsObject;
    public TextMeshProUGUI[] detailsText;

    public GameObject ExitPanel;
    public GameObject Rules;
    public GameObject Language;
    public Results result;

    public GameObject[] rules; 
    private void Awake()
    {
        if(instence == null)
        {
            instence = this; 
        }
    }
    void Start()
    {
        trump.transform.parent.gameObject.SetActive(false);
        switchPanels(3);
        LeanTween.scale(notfication, new Vector3(0, 0, 0), 0.1f);
        detailsIsOn = false;
        int temp = persistantmanager.instence.pNoNow;
        tempPlayerNumbers[0] = temp; 
        temp = Bidding.instence.StepUp(temp);
        tempPlayerNumbers[1] = temp;
        temp = Bidding.instence.StepUp(temp);
        tempPlayerNumbers[2] = temp;
        temp = Bidding.instence.StepUp(temp);
        tempPlayerNumbers[3] = temp;
    }

    private void Update()
    {
        if (GameManager.instence.partnerAce != Deck.CardType.NotSelected )
        {
            partnerAce.transform.parent.gameObject.SetActive(true);
            if (GameManager.instence.partnerRevealed && GameManager.instence.selectedpartnerAce != -1)
            {
                partnerAce.text = Languages.instence.GetText("PartnerAce") + " : " + Languages.instence.GetText(GameManager.instence.partnerAce.ToString()) + "\n " + Languages.instence.GetText("Partner") +": " + persistantmanager.instence.players[(GameManager.instence.selectedpartnerAce)].name;
            }
            else
            {
                if (GameManager.instence.partnerWithSelf&& GameManager.instence.partnerRevealed)
                {
                    partnerAce.text = Languages.instence.GetText("PartnerAce")+" : " + Languages.instence.GetText(GameManager.instence.partnerAce.ToString()) + "\n " + Languages.instence.GetText("Partner") + " : " + Languages.instence.GetText("SelfPartner");
                }
                else
                {
                    partnerAce.text = Languages.instence.GetText("PartnerAce")+" : " + Languages.instence.GetText(GameManager.instence.partnerAce.ToString());
                }
            }
        }
        else
        {
            partnerAce.transform.parent.gameObject.SetActive(false);
        }
        if (GameManager.instence.trump != Deck.CardType.NotSelected)
        {
            trump.transform.parent.gameObject.SetActive(true);
            trump.text = Languages.instence.GetText("Trump") +" : " + Languages.instence.GetText(GameManager.instence.trump.ToString()) + "\n"+ Languages.instence.GetText("Bidwinner")+" : " + persistantmanager.instence.players[GameManager.instence.highestBider].name + "" ;
            if (GameManager.instence.selectedGameMode == Bidding.GameMode.Vip)
            {
                //trump.text = trump.text + " vip" + GameManager.instence.selectedChild; 
            }
        }
        else
        {
            trump.transform.parent.gameObject.SetActive(false);
        }
        if (GameManager.instence.state == GameManager.State.Biding || GameManager.instence.state == GameManager.State.Selection)
        {
            switchPanels(1);
        }
        else if (GameManager.instence.state == GameManager.State.PLayable || GameManager.instence.state == GameManager.State.Waititng)
        {
            switchPanels(0);
        }
        //if(GameManager.instence.selectedpartnerAce != -1 && GameManager.instence.partnerRevealed)
        //{
        //    tricks.text = "P:" + (Bidding.instence.highestCount + 1) + "  and partner P:" + (GameManager.instence.selectedpartnerAce + 1) + " has won " + (Scoring.instence.playerstrick[GameManager.instence.highestBider, Scoring.instence.roundNo] + Scoring.instence.playerstrick[GameManager.instence.selectedpartnerAce, Scoring.instence.roundNo]) + " tricks in this round";
        //}
        //else
        //{
        //    tricks.text = "P:" + (Bidding.instence.highestCount + 1) +" has won " + (Scoring.instence.playerstrick[GameManager.instence.highestBider, Scoring.instence.roundNo]) + " tricks in this round";
        //}

        detailsText[0].text = Languages.instence.GetText("Tricks") + ":"+ Scoring.instence.playerstrick[tempPlayerNumbers[0], Scoring.instence.roundNo]+ "";
        detailsText[1].text = Languages.instence.GetText("Tricks") + ":" + Scoring.instence.playerstrick[tempPlayerNumbers[1], Scoring.instence.roundNo] + "";
        detailsText[2].text = Languages.instence.GetText("Tricks") + ":" + Scoring.instence.playerstrick[tempPlayerNumbers[2], Scoring.instence.roundNo] + "";
        detailsText[3].text = Languages.instence.GetText("Tricks") + ":" + Scoring.instence.playerstrick[tempPlayerNumbers[3], Scoring.instence.roundNo] + "";
        
    }
    public void SettingsForStart()
    {
        //tricks.text = "";
        partnerAce.text = "";
        trump.transform.parent.gameObject.SetActive(false);
    }
    public IEnumerator WaitForResultShow()
    {
        GameManager.instence.state = GameManager.State.Animation;
        switchPanels(2);

        yield return new WaitForSeconds(10);

        switchPanels(3);

    }
    public void TurnOffResults()
    {
        switchPanels(3);
    }
    //public void ShowNotification(string text )
    //{
    //    LeanTween.scale(notfication, new Vector3(1, 1, 1), 0.1f);
    //    this.text.text = text;
    //    StartCoroutine(WaitForStop());
    //}
    //public IEnumerator WaitForStop()
    //{
    //    yield return new WaitForSeconds(1);
    //    LeanTween.scale(notfication, new Vector3(0, 0, 0), 0.1f);
    //}
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
  
    public void RestartGame()
    {
        LeanTween.scale(ExitPanel, new Vector3(1,1,1) , 0.05f);
    }
    public void Resume()
    {
        LeanTween.scale(ExitPanel, new Vector3(0, 0, 0), 0.05f);
    }
    public void ShowRules()
    {
        LeanTween.scale(Rules, new Vector3(1, 1, 1), 0.05f);
        if (persistantmanager.instence.lang == persistantmanager.Lang.English)
        {
            rules[0].SetActive(true);
            rules[1].SetActive(false);
        }
        else
        {
            rules[1].SetActive(true);
            rules[0].SetActive(false);
        }
    }
    public void BackFromRules()
    {
        LeanTween.scale(Rules, new Vector3(0, 0, 0), 0.05f);
    }
    public void SelectLanguage()
    {
        LeanTween.scale(Language, new Vector3(1, 1, 1), 0.05f);
    }
    public void selectEnglish()
    {
        LeanTween.scale(Language, new Vector3(0, 0, 0), 0.05f);
        persistantmanager.instence.lang = persistantmanager.Lang.English;
        Languages.instence.OnLanguageChangeHandler.Invoke();
    }
    public void selectDanish()
    {
        LeanTween.scale(Language, new Vector3(0, 0, 0), 0.15f);
        persistantmanager.instence.lang = persistantmanager.Lang.Danish;
        Languages.instence.OnLanguageChangeHandler.Invoke();
    }
    public void showResults()
    {
        result.gameObject.SetActive(true);
        result.switchPanels(0);
        Invoke("EndResults", 5);
    }

    public void EndResults()
    {
        result.switchPanels(3);
        result.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        if (persistantmanager.instence.multiplayer)
        {
            GameManager.instence.ForRpc.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
