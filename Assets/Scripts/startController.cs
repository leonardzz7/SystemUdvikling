using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;


public class startController : MonoBehaviour
{
    public static startController instance;
    public GameObject[] panels;
    public Slider slider;
    public Toggle toggle;

    public Transform parent;
    public GameObject playerImagePrefab;
    public GameObject PlayerPicturesObject;
    public GameObject PlayerDisplayerPicture;
    public GameObject tempCharacter;
    public GameObject CharacterPrefab;




    public void LoadHeartGame()
    {
        SceneManager.LoadScene(2);
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        instance = this;
        switchPanels(8);//6

        SpwanPlayerImages(0);
    }
    private void Start()
    {
        if (persistantmanager.instence.logedIn)
        {
            switchPanels(0);
            persistantmanager.instence.NoOfArtificalPlayers = 0;
            for (int x = 0; x < 4; x++)
            {
                persistantmanager.instence.ArtificalPlayers[x] = false;
            }
            PhotonNetwork.NickName = Player1.instance.user_name;
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                    //PhotonNetwork.LeaveLobby();
                    Debug.Log("room Left");

                }
            }
            SelectPicture();
            persistantmanager.instence.players[0].picId0 = PlayerPrefs.GetInt("picId0", 0);
            persistantmanager.instence.players[0].picId1 = PlayerPrefs.GetInt("picId1", 0);
            persistantmanager.instence.players[0].picId2 = PlayerPrefs.GetInt("picId2", 0);
            persistantmanager.instence.players[0].picId3 = PlayerPrefs.GetInt("picId3", 0);
            persistantmanager.instence.players[0].picId4 = PlayerPrefs.GetInt("picId4", 0);
        }
    }
    public void SinglePlayer()
    {
        persistantmanager.instence.players = new Myplayer[4];
        persistantmanager.instence.players[0] = new Myplayer();
        persistantmanager.instence.players[1] = new Myplayer();
        persistantmanager.instence.players[2] = new Myplayer();
        persistantmanager.instence.players[3] = new Myplayer();
        persistantmanager.instence.players[0].name = "player1";
        persistantmanager.instence.players[1].name = "player2";
        persistantmanager.instence.players[2].name = "player3";
        persistantmanager.instence.players[3].name = "player4";
        SceneManager.LoadScene(1);
        persistantmanager.instence.multiplayer = false;
    }
    public void MultiPlayer()
    {
        
        if (PhotonNetwork.InLobby)
        {
            persistantmanager.instence.multiplayer = true;
            switchPanels(1);
        }
        else
        {
            persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("pleasewait"), null, 0);
            
        }
    }
    public void TurnOnSettings()
    {
        switchPanels(7);
    }
    public void TurnOffSettings()
    {
        switchPanels(0);
    }
    public void RandomRoom()
    {
        persistantmanager.instence.customRoom = false;
        switchPanels(4);
    }
    public void CustomRoom()
    {
        persistantmanager.instence.customRoom = true;
        switchPanels(5);
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

    public void OnValueChange()
    {
        persistantmanager.instence.backGround = (int)slider.value;
    }
    public void OnValueChangeFlip()
    {
        persistantmanager.instence.CardTurn = toggle.isOn;
    }

    public void OpenPlayerPictures()
    {
        PlayerPicturesObject.LeanScale(Vector3.one, 0.2f);
    }
    public void ClosePlayerPictures()
    {
        PlayerPicturesObject.LeanScale(Vector3.zero, 0.2f);
    }
    List<GameObject> spawnedList = new List<GameObject>();
    public void SpwanPlayerImages(int x)
    {
        int count = 0;

        if (spawnedList.Count != 0)
        {
            foreach (var obj in spawnedList)
            {
                Destroy(obj);
            }
        }


        foreach (var obj in CharacterDataLoad.instence.hairs)
        {
            var ins = Instantiate(playerImagePrefab, parent);
            spawnedList.Add(ins);
            ins.GetComponent<playerImage>().instantiateit(count, x);

            count++;
        }

    }
    public void SelectPicture()
    {
        if (tempCharacter != null)
        {
            Destroy(tempCharacter);
        }
        tempCharacter = Instantiate(CharacterPrefab, PlayerDisplayerPicture.transform);

        tempCharacter.GetComponent<CharacterUpdate>().Updateit(PlayerPrefs.GetInt("picId0", 0), PlayerPrefs.GetInt("picId1", 0), PlayerPrefs.GetInt("picId2", 0), PlayerPrefs.GetInt("picId3", 0), PlayerPrefs.GetInt("picId4", 0));
       
        ClosePlayerPictures();
    }
}
