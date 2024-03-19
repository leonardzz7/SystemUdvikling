using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomListEntry : MonoBehaviour
    {
        public TextMeshProUGUI RoomNameText;
        public TextMeshProUGUI RoomPlayersText;
        public Button JoinRoomButton;

        private string roomName;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }
                PhotonNetwork.NickName = Player1.instance.user_name;
                Hashtable hash = new Hashtable();
                hash.Add("Flag", 0);
                hash.Add("Picture0", PlayerPrefs.GetInt("picId0", 0));
                hash.Add("Picture1", PlayerPrefs.GetInt("picId1", 0));
                hash.Add("Picture2", PlayerPrefs.GetInt("picId2", 0));
                hash.Add("Picture3", PlayerPrefs.GetInt("picId3", 0));
                hash.Add("Picture4", PlayerPrefs.GetInt("picId4", 0));
                hash.Add("id", Player1.instance.id);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                PhotonNetwork.JoinRoom(roomName);
            });
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers)
        {
            roomName = name;

            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        }
    }
