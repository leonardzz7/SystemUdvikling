using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Results : MonoBehaviour
{
    public GameObject[] panels;
    public TextMeshProUGUI[] texts;
    public TextMeshProUGUI[] totals;
    public TextMeshProUGUI[] points;
    public TextMeshProUGUI[] gameType;
    public GameObject win;
    public GameObject lose; 
    
    private void Start()
    {
        int count = 0; 
        foreach(Myplayer p in persistantmanager.instence.players)
        {
            texts[count].SetText( p.name);
            count++; 
        }
    }
    public void FillPoints(string str)
    {
        foreach (TextMeshProUGUI T in points)
        {
            T.text = str; 
        }
    }
    public void FillGameType(string str)
    {
        foreach (TextMeshProUGUI T in gameType)
        {
            T.text = str;
        }
    }
    public void ShowWin()
    {
        win.LeanScale(new Vector3(2,2,2), 0.5f).setEaseInBounce();
    }
    public void ShowLose()
    {
        lose.LeanScale(new Vector3(2, 2, 2), 0.5f).setEaseInBounce();
    }
    public void ResetShow()
    {
        win.LeanScale(Vector3.zero, 0.5f).setEaseInBounce();
        lose.LeanScale(Vector3.zero, 0.5f).setEaseInBounce();
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
}
