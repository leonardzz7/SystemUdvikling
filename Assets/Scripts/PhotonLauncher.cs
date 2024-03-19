using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PhotonLauncher : MonoBehaviourPunCallbacks, IMatchmakingCallbacks

{
	public static PhotonLauncher instence;
	private void Awake()
	{
		if (instence == null)
		{
			instence = this;
		}
		PhotonNetwork.ConnectUsingSettings();
	}
	public string regionCode;

	public TextMeshProUGUI count;
	[SerializeField]
	public TextMeshProUGUI feedbackText;
	public TMP_InputField NameroomCustom;

	public TextMeshProUGUI Player1Name;
	public TextMeshProUGUI Player2Name;
	public TextMeshProUGUI Player3Name;
	public TextMeshProUGUI Player4Name;
	public GameObject[] PlayerImage;

	public GameObject[] Player;
	public GameObject[] PlayerOff;
	public Vector3[] temp;

	public bool flag;
	public bool flag1;
	public bool flag2;
	public bool flag3;
	#region Private Fields
	public Text ddebug;
	bool isConnecting;

	string gameVersion = "1";
	private TypedLobby customLobby = new TypedLobby("OnlyLobby", LobbyType.Default);
	float t = 6;
	bool b = false;


	public float startedTime = 0;
	bool isStarted = false;
	#endregion
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
	private Dictionary<int, GameObject> playerListEntries;

	public int playerImagecount = 0;

	public AudioSource UiSound;
	public AudioSource PlayerAddedSound;

	public void PlayUISound()
	{
		UiSound.Play();
	}
	public void PlayAddedSounds()
	{
		PlayerAddedSound.Play();
	}
	
	void Start()
	{
		playerImagecount = 0; 
		Startsetting();
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.GameVersion = this.gameVersion;
		PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
		b = false;

		temp = new Vector3[4];
		int x = 0;
		foreach (GameObject player in Player)
		{
			temp[x] = player.transform.position;
			x++;
		}
		int y = 0; 
		foreach (GameObject player in Player)
		{
			LeanTween.move(player, PlayerOff[y].transform.position, 0.8f).setEaseInBounce();
			y++;
		}
	}
	public void Startsetting()
	{
		flag = true;
		flag1 = true;
		flag2 = false;
		flag3 = false;
		for (int i = 1; i <= 4; i++)
		{

			persistantmanager.instence.players[i - 1] = new Myplayer();
		}
		if (!PhotonNetwork.InLobby)
		{
			if (PhotonNetwork.IsConnected)
			{
				JoinLobyNow();
				Debug.Log("Join loby");
			}
			else
			{
				PhotonNetwork.ConnectUsingSettings();
			}
		}
	}
	void Update()
	{
		if (isStarted)
		{
			startedTime = startedTime + Time.deltaTime;
			if(PhotonNetwork.IsMasterClient)
			CheckNeedForAiPlayer();
			
		}
		if (check)
		{
			t = t - Time.deltaTime;
			count.text = "" + (int)t;
		}
		if (PhotonNetwork.InRoom)
		{
			ProcessForAddingPlayers();
		}
		
	}
	public void ProcessForAddingPlayers()
	{
		if (!check) CheckIFAiPlayerAdded();
		if ((PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers) >= 1 && flag1)
		{
			playerImagecount=1; 
			Player1Name.text = PhotonNetwork.CurrentRoom.GetPlayer(playerImagecount).NickName;
			UpdatePlayer(1,1);
			GameObject obj = Instantiate(CharacterDataLoad.instence.character, PlayerImage[0].transform);
			obj.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[0].picId0, persistantmanager.instence.players[0].picId1, persistantmanager.instence.players[0].picId2, persistantmanager.instence.players[0].picId3, persistantmanager.instence.players[0].picId4);
			StartCoroutine(TurnPlayerOn(1, playerImagecount) );
			flag1 = false;
			flag2 = true;
		}
		else if ((PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers) >= 2 && flag2)
		{
			if (persistantmanager.instence.ArtificalPlayers[1])
			{
				Player2Name.text = PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).CustomProperties["AI2Name"].ToString();

				UpdatePlayerAi(2);
			}
			else
			{
				playerImagecount = 2; 
				Player2Name.text = PhotonNetwork.CurrentRoom.GetPlayer(playerImagecount).NickName;
				UpdatePlayer(2,2- persistantmanager.instence.NoOfArtificalPlayers);
			}
			GameObject obj = Instantiate(CharacterDataLoad.instence.character, PlayerImage[1].transform);
			obj.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[1].picId0, persistantmanager.instence.players[1].picId1, persistantmanager.instence.players[1].picId2, persistantmanager.instence.players[1].picId3, persistantmanager.instence.players[1].picId4);
			StartCoroutine(TurnPlayerOn(2, playerImagecount));
			flag2 = false;
			flag3 = true;
		}
		else if ((PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers) >= 3 && flag3)
		{
			if (persistantmanager.instence.ArtificalPlayers[2])
			{
				Player3Name.text = PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).CustomProperties["AI3Name"].ToString();

				UpdatePlayerAi(3);
			}
			else
			{
				if(persistantmanager.instence.NoOfArtificalPlayers < 1)
				{
					playerImagecount = 2;
				}
				else
				{
					playerImagecount = 1;
				}
				playerImagecount++; 
				Player3Name.text = PhotonNetwork.CurrentRoom.GetPlayer(playerImagecount).NickName;
				UpdatePlayer(3,3- persistantmanager.instence.NoOfArtificalPlayers);
			}

			GameObject obj = Instantiate(CharacterDataLoad.instence.character, PlayerImage[2].transform);
			obj.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[2].picId0, persistantmanager.instence.players[2].picId1, persistantmanager.instence.players[2].picId2, persistantmanager.instence.players[2].picId3, persistantmanager.instence.players[2].picId4);
			StartCoroutine(TurnPlayerOn(3, playerImagecount));
			flag3 = false;
			flag = false;
		}
		else if ((PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers) >= 4 && !flag)
		{
			if (persistantmanager.instence.ArtificalPlayers[3])
			{
				Player4Name.text = PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).CustomProperties["AI4Name"].ToString();

				UpdatePlayerAi(4);
			}
			else
			{
				if (persistantmanager.instence.NoOfArtificalPlayers < 1)
				{
					playerImagecount = 3;
				}
				else if (persistantmanager.instence.NoOfArtificalPlayers < 2)
				{
					playerImagecount = 2;
				}
				else
				{
					playerImagecount = 1; 
				}
				Player4Name.text = PhotonNetwork.CurrentRoom.GetPlayer(playerImagecount).NickName;

				UpdatePlayer(4,4- persistantmanager.instence.NoOfArtificalPlayers);
			}
			GameObject obj = Instantiate(CharacterDataLoad.instence.character, PlayerImage[3].transform);
			obj.GetComponent<CharacterUpdate>().Updateit(persistantmanager.instence.players[3].picId0, persistantmanager.instence.players[3].picId1, persistantmanager.instence.players[3].picId2, persistantmanager.instence.players[3].picId3, persistantmanager.instence.players[3].picId4);
			StartCoroutine(TurnPlayerOn(4, playerImagecount));
			//Invoke("UpdatePlayer", 4.8f);
			isStarted = false;
			flag = true;

			check = true;

			Debug.Log("here 4");

			if (PhotonNetwork.IsMasterClient && !b)
			{
				PhotonNetwork.CurrentRoom.IsOpen = false;
				StartCoroutine(WaitForstart());

				b = true;
			}
		}
	}

	public void BackToLoby()
	{
		persistantmanager.instence.pNoNow = 0; 
	}


	// --------------------------------- AI -----------------------------------------------

	public void AddAiPlayer(int x)
	{
		Debug.Log("Ai player " + x);
		persistantmanager.instence.ArtificalPlayers[x] = true;
		persistantmanager.instence.NoOfArtificalPlayers++;
	}
	public void CheckNeedForAiPlayer()
	{
		Hashtable hash = new Hashtable();

		if (startedTime > 4f && PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers == 1 && startedTime < 8f)
		{
			if (!persistantmanager.instence.ArtificalPlayers[1])
			{
				hash.Add("AI2", true);
				hash.Add("AI2Name", AiNameLists[Random.Range(0, AiNameLists.Length)]);
				PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).SetCustomProperties(hash);
			}
		}
		else if (startedTime > 8f && PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers == 2 && startedTime < 12f)
		{
			if (!persistantmanager.instence.ArtificalPlayers[2])
			{
				hash.Add("AI3", true);
				hash.Add("AI3Name", AiNameLists[Random.Range(0, AiNameLists.Length)]);
				PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).SetCustomProperties(hash);
			}
		}
		else if (startedTime > 12f && PhotonNetwork.CurrentRoom.PlayerCount + persistantmanager.instence.NoOfArtificalPlayers == 3)
		{
			if (!persistantmanager.instence.ArtificalPlayers[3])
			{
				hash.Add("AI4", true);
				hash.Add("AI4Name", AiNameLists[Random.Range(0, AiNameLists.Length)]);
				PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).SetCustomProperties(hash);
			}
		}
	}
	public void CheckIFAiPlayerAdded()
	{
		if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
		{
			for (int x = 1; x < 4; x++)
			{
				if (!persistantmanager.instence.ArtificalPlayers[x])
				{

					if (PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).CustomProperties["AI" + (x + 1)] != null)
					{
						persistantmanager.instence.players[x].name = (string)PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).CustomProperties["AI" + (x + 1)+"Name" ];
						AddAiPlayer(x);
					}
				}
			}
		}
	}
	public void UpdatePlayer(int i , int iPlayer)
	{
			
			persistantmanager.instence.players[i - 1].name = PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).NickName.ToString();
		persistantmanager.instence.players[i - 1].flagId = (int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Flag"];
		Debug.Log((int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Flag"]);
			persistantmanager.instence.players[i - 1].picId0 = (int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Picture0"];
		persistantmanager.instence.players[i - 1].picId1 = (int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Picture1"];
		persistantmanager.instence.players[i - 1].picId2 = (int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Picture2"];
		persistantmanager.instence.players[i - 1].picId3 = (int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Picture3"];
		persistantmanager.instence.players[i - 1].picId4 = (int)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["Picture4"];
		persistantmanager.instence.players[i - 1].id = int.Parse((string)PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).CustomProperties["id"]);
			persistantmanager.instence.players[i - 1].id = i - 1;
			if (PhotonNetwork.CurrentRoom.GetPlayer(iPlayer).IsLocal)
			{
				persistantmanager.instence.pNoNow = i - 1;
			}
			persistantmanager.instence.ArtificalPlayers[i - 1] = false;
	   
	}
	public string[] AiNameLists;

	public void UpdatePlayerAi(int i)
	{

		persistantmanager.instence.players[i-1].flagId = 1;
		persistantmanager.instence.players[i-1].picId0 = i+1;
		persistantmanager.instence.players[i - 1].picId1 = i + 1;
		persistantmanager.instence.players[i - 1].picId2 = i + 1;
		persistantmanager.instence.players[i - 1].picId3 = i + 1;
		persistantmanager.instence.players[i - 1].picId4 = i + 1;
		persistantmanager.instence.players[i-1].id = i+1;

	}

	// --------------------------------Room -------------------------------------------

	public void JoinLobyNow()
	{
		PhotonNetwork.JoinLobby(customLobby);
		Debug.Log("JoinLoby");
	}
	public void ConnectToRandomRoom()
	{
		persistantmanager.instence.customRoom = false;

		feedbackText.text = "";

		isConnecting = true;

		if (PhotonNetwork.IsConnected)
		{
			LogFeedback("Joining Room...");
			PhotonNetwork.NickName = Player1.instance.user_name;
			Hashtable hash = new Hashtable();
			hash.Add("Flag", 5);
			hash.Add("Picture0", PlayerPrefs.GetInt("picId0", 0));
			hash.Add("Picture1", PlayerPrefs.GetInt("picId1", 0));
			hash.Add("Picture2", PlayerPrefs.GetInt("picId2", 0));
			hash.Add("Picture3", PlayerPrefs.GetInt("picId3", 0));
			hash.Add("Picture4", PlayerPrefs.GetInt("picId4", 0));
			hash.Add("id", Player1.instance.id);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			PhotonNetwork.JoinRandomRoom();
			startController.instance.switchPanels(2);
		}
		else
		{
			LogFeedback(Languages.instence.GetText("connecting"));
			PhotonNetwork.GameVersion = this.gameVersion;
			PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
			PhotonNetwork.ConnectUsingSettings();
		}
	}
	public void ConnectToCustomRoom()
	{
		if (NameroomCustom.text != "")
		{

			feedbackText.text = "";

			persistantmanager.instence.customRoom = true;
			isConnecting = true;
			if (PhotonNetwork.IsConnected)
			{
				LogFeedback(Languages.instence.GetText("joiningRoom"));
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
				RoomOptions roomOptions = new RoomOptions();
				roomOptions.IsVisible = true;
				roomOptions.PlayerTtl = 60000;
				roomOptions.MaxPlayers = 4;
				roomOptions.EmptyRoomTtl = 0;

				roomOptions.CleanupCacheOnLeave = false;
				PhotonNetwork.JoinOrCreateRoom(NameroomCustom.text, roomOptions, TypedLobby.Default, null);

				startController.instance.switchPanels(2);

			}
			else
			{
				LogFeedback("Connecting...");
				PhotonNetwork.GameVersion = this.gameVersion;
				PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
				PhotonNetwork.ConnectUsingSettings();
			}
		}
		else
		{
			persistantmanager.instence.PopUpWakeUp(Languages.instence.GetText("enterRoomCode"), null , 0 );
		}
	}
	public void CreateARoom()
	{
		PhotonNetwork.NickName = Player1.instance.user_name;
		Hashtable hash = new Hashtable();
		hash.Add("Flag", 0);
		hash.Add("Picture", Player1.instance.photon_id);
		hash.Add("id", Player1.instance.id);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.PlayerTtl = 60000;
		roomOptions.MaxPlayers = 4;
		roomOptions.EmptyRoomTtl = 0;

		roomOptions.CleanupCacheOnLeave = false;

		PhotonNetwork.CreateRoom(Random.Range(10000f, 99999f).ToString(), roomOptions);
	}

	//-------------------------------Helper -------------------------------------------

	public IEnumerator TurnPlayerOn(int count , int playerImageCount)
	{

		yield return new WaitForSeconds(0.5f);

		PlayAddedSounds();
		int y = 0;
		foreach (GameObject player in Player)
		{
			if (y == count-1)
			{
				LeanTween.move(player, temp[y], 0.8f).setEaseInBounce();
				if (!persistantmanager.instence.ArtificalPlayers[y])
				{
					persistantmanager.instence.players[y].picId0 = (int)PhotonNetwork.CurrentRoom.GetPlayer(playerImageCount).CustomProperties["Picture0"];
					persistantmanager.instence.players[y].picId1 = (int)PhotonNetwork.CurrentRoom.GetPlayer(playerImageCount).CustomProperties["Picture1"];
					persistantmanager.instence.players[y].picId2 = (int)PhotonNetwork.CurrentRoom.GetPlayer(playerImageCount).CustomProperties["Picture2"];
					persistantmanager.instence.players[y].picId3 = (int)PhotonNetwork.CurrentRoom.GetPlayer(playerImageCount).CustomProperties["Picture3"];
					persistantmanager.instence.players[y].picId4 = (int)PhotonNetwork.CurrentRoom.GetPlayer(playerImageCount).CustomProperties["Picture4"];

				}
				else
				{
					persistantmanager.instence.players[y].picId0 = y;
					persistantmanager.instence.players[y].picId1 = y;
					persistantmanager.instence.players[y].picId2 = y;
					persistantmanager.instence.players[y].picId3 = y;
					persistantmanager.instence.players[y].picId4 = y;
				}
			}
			y++;
		}
	}
	bool check = false;
	IEnumerator WaitForstart()
	{
		ddebug.text = Languages.instence.GetText("goingToStart");
		yield return new WaitForSeconds(5);

		PhotonNetwork.LoadLevel(1);
	}
	public void LeaveRoom()
	{
		if (PhotonNetwork.IsConnected)
		{
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.LeaveRoom();
				//PhotonNetwork.LeaveLobby();
			}
		}
		startController.instance.switchPanels(0);

	}
	public int StepUp(int temp)
	{
		if (temp == 3)
		{
			temp = 0;
		}
		else
		{
			temp++;
		}
		return temp;
	}
	void LogFeedback(string message)
	{
		if (feedbackText == null)
		{
			return;
		}
		ddebug.color = Color.white;
		feedbackText.text += System.Environment.NewLine + message;
		ddebug.text += System.Environment.NewLine + message;
	}
	private void OnApplicationQuit()
	{
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.LeaveRoom();
	}

	//------------------Photon Overrides -------------------------------------------------------

	public override void OnDisconnected(DisconnectCause cause)
	{
		LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
		Debug.LogError("/Launcher:Disconnected");
		isConnecting = false;
	}
	public override void OnJoinedRoom()
	{

		persistantmanager.instence.multiplayer = true;
		if (PhotonNetwork.IsMasterClient)
		{
			isStarted = true;
		}
		LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
		Debug.Log("/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
		startController.instance.switchPanels(3);
		
	}
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
		Debug.Log("/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
		//PhotonNetwork.CreateRoom(null, new RoomOptions());
		CreateARoom();
	}

	public override void OnJoinedLobby()
	{
		base.OnJoinedLobby();
		
		ddebug.color = Color.green;
		ddebug.text = Languages.instence.GetText("ReadyToPlay");


		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.KeepAliveInBackground = 300f;
		ddebug.text = Languages.instence.GetText("serverConnection");
		JoinLobyNow();
	}
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);

	}
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);

		
	}
}