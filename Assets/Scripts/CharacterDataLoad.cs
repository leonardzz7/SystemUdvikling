using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataLoad : MonoBehaviour
{
    public static CharacterDataLoad instence;
    public GameObject[] eyes;
    public GameObject[] hairs;
    public GameObject[] bottoms;
    public GameObject[] tops;
    public GameObject[] shoes;

    public GameObject character; 
    private void Awake()
    {
        if (instence== null)
        {
            instence = this;
            DontDestroyOnLoad(this);
        }
        eyes = Resources.LoadAll<GameObject>("eyes");
        hairs = Resources.LoadAll<GameObject>("hairs");
        tops = Resources.LoadAll<GameObject>("tops");
        bottoms = Resources.LoadAll<GameObject>("botoms");
        shoes = Resources.LoadAll<GameObject>("shoes");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
