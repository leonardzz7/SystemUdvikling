using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllplayerImages : MonoBehaviour
{
    public static AllplayerImages Instance;
    public GameObject[] playerImages; 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
