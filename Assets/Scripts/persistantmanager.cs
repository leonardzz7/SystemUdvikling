using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class persistantmanager : MonoBehaviour
{
    public static persistantmanager instence;
    public GameObject PopUpPanel;
    public bool customRoom;
    public bool multiplayer;
    public int pNoNow;

    public Myplayer[] players;
    public bool CardTurn;
    public int backGround;

    public bool logedIn; 

        
    public enum Lang
    {
        English, 
        Danish
    }

    public Lang lang = Lang.English; 
    public class PopUps 
    {
        public string str; 
        public Sprite spr; 
        public int SoundNo; 
    }
    public PopUps[] pops;
    
    public int inStack ;
    public bool popOpen;
    public bool[] ArtificalPlayers;
    public int NoOfArtificalPlayers;

    private void Awake()
    {
        NoOfArtificalPlayers = 0;
        pops = new PopUps[10];
        if (instence == null )
        {
            instence = this;
            players = new Myplayer[4];
            DontDestroyOnLoad(this);
            logedIn = false; 
        }
        else
        {
            Destroy(this);
        }
    }
    public void PopUpWakeUp(string str , Sprite spr , int SoundNo)
    {
        if (!popOpen) {
            GameObject parent = GameObject.Find("PopupParent");
            GameObject obj = Instantiate(PopUpPanel, parent.transform);
            obj.GetComponent<PopUp>().text.text = str;
            if (spr != null)
            {
                obj.GetComponent<PopUp>().image.sprite = spr;
                obj.GetComponent<PopUp>().image.gameObject.LeanScale(new Vector3(1, 1, 1), 0.1f);
            }
            else
            {
                obj.GetComponent<PopUp>().image.gameObject.LeanScale(new Vector3(0, 0, 0), 0.1f);
            }
            LeanTween.scale(obj, new Vector3(1f, 1f, 1f), 0.4f);
            StartCoroutine(WaitForClosing(obj));
        }
        else
        {
            inStack++;
            pops[inStack] = new PopUps();
            pops[inStack].str = str;
            pops[inStack].spr = spr;
            pops[inStack].SoundNo = SoundNo;
        }
    }
    IEnumerator WaitForClosing( GameObject obj)
    {
        popOpen = true;
        yield return new WaitForSeconds(2);
        LeanTween.scale(obj, new Vector3(0f, 0f, 0f), 0.4f);
        Destroy(obj , 0.4f);
        popOpen = false;
        if (inStack!= 0 )
        {
            PopUpWakeUp(pops[inStack].str , pops[inStack].spr , pops[inStack].SoundNo);
            inStack--; 
        }
    }
    public void addArtificalPlayer(int x)
    { 
        for(int i =0; i < 5; i++)
        {
            if(i == x)
            {
                ArtificalPlayers[i] = true;
                NoOfArtificalPlayers++; 
            }
        }
    }
}