using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickScore : MonoBehaviour
{
    public int score=0;
    public static TrickScore instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalculateScore()
    {
        score = 0;
        for(int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<Card>().type == Deck.CardType.Heart)
            {
                score++;
                Debug.Log("score is " + score);
                Debug.Log("this.gameObject.transform.childCount " + this.gameObject.transform.childCount);
            }
            else if (this.gameObject.transform.GetChild(i).name== "PlayingCards_QSpades")
            {
                score+=13;
            }
        }
    }
}
