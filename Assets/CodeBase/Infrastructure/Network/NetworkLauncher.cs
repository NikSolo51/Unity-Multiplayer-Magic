using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Services.SaveLoad;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure.Network
{
    public class NetworkLauncher : MonoBehaviourPunCallbacks, ISavedProgress
    {
        [SerializeField] private ServerSettings _serverSettings;
        [Required] [SerializeField] private TMP_InputField _serverIPnputField;
        [Required] [SerializeField] private TMP_InputField _portInputField;
        [Required] [SerializeField] private TMP_InputField _roomNameInputField;
        [Required] [SerializeField] private GameObject _menu;
        [SerializeField] private TMP_InputField _nickNameInputField;
        [Required] [SerializeField] private Transform _parentOfPlayerRoomList;
        [Required] [SerializeField] private Transform _parentOfFindRoomList;
        [Required] [SerializeField] private GameObject _startGameButton;

        [SerializeField] private UnityEvent OnJoinedRoomEvent;
        private IGameFactory _gameFactory;
        private ISaveLoadService _saveLoadService;
        private GameStateMachine _gameStateMachine;
        private const string LevelName = "Level1";

        public UnityEvent OnConnected;

        [Inject]
        public void Construct(IGameFactory gameFactory, ISaveLoadService saveLoadService,
            GameStateMachine gameStateMachine)
        {
            _gameFactory = gameFactory;
            _saveLoadService = saveLoadService;
            _gameStateMachine = gameStateMachine;
            _saveLoadService.Register(this);
        }

        public void SetServerIP()
        {
            _serverSettings.AppSettings.Server = _serverIPnputField.text;
        }

        public void SetServerPort()
        {
            _serverSettings.AppSettings.Port = Int32.Parse(_portInputField.text);
        }
        
        public void ConnectToServer()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void UseCloudConnect()
        {
            _serverSettings.AppSettings.Server = "";
            _serverSettings.AppSettings.Port = 0;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected Master");
            OnConnected?.Invoke();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
                PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
        }

        public void SetUpNickName()
        {
            PhotonNetwork.NickName = _nickNameInputField.text;
        }

        public void CreateRoom()
        {
            if (string.IsNullOrEmpty(_roomNameInputField.text))
                return;

            PhotonNetwork.CreateRoom(_roomNameInputField.text);
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(LevelName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnJoinedRoom()
        {
            OnJoinedRoomEvent?.Invoke();
            Debug.Log("Joined Room");

            for (int i = 0; i < _parentOfPlayerRoomList.childCount; i++)
            {
                Destroy(_parentOfPlayerRoomList.GetChild(i));
            }

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                _gameFactory.CreatePlayerRoomButton(PhotonNetwork.PlayerList[i], this, _parentOfPlayerRoomList);
            }

            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Create Room Failed " + returnCode);
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left Room");
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            for (int i = 0; i < _parentOfFindRoomList.childCount; i++)
            {
                Destroy(_parentOfFindRoomList.GetChild(i).gameObject);
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].RemovedFromList)
                    continue;
                _gameFactory.CreateRoomButton(roomList[i], this, _parentOfFindRoomList);
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _gameFactory.CreatePlayerRoomButton(newPlayer, this, _parentOfPlayerRoomList);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _nickNameInputField.text = progress._nickName;
            SetUpNickName();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress._nickName = PhotonNetwork.NickName;
        }

        private void OnApplicationQuit()
        {
            _saveLoadService.SaveProgress();
        }
    }
}