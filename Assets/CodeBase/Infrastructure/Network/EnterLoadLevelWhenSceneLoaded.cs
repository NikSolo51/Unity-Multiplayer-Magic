﻿using CodeBase.Infrastructure.States;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Network
{
    public class EnterLoadLevelWhenSceneLoaded : MonoBehaviourPunCallbacks
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public override void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _gameStateMachine.Enter<LoadLevelState, string>(scene.name);
        }
    }
}