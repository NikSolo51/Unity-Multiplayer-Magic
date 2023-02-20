using System;
using CodeBase.Logic.Raycast;
using CodeBase.Services.Input;
using CodeBase.Services.Update;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Interactive
{
    public class InteractiveSystem : MonoBehaviour,IUpdatable
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private CameraRayCast _cameraRayCast;
        private IUpdateService _updateService;
        private IInputService _inputService;
        public event Action<Collider> OnInteractive; 
        public event Action OnUnInteractive; 

        [Inject]
        public void Construct(IUpdateService updateService,IInputService inputService)
        {
            _updateService = updateService;
            _inputService = inputService;
            if (_photonView.IsMine)
            {
                updateService.Register(this);
            }
        }

        private void OnDestroy()
        {
            if (_updateService != null)
            {
                _updateService.Unregister(this);
            }
        }

        public void UpdateTick()
        {
            if (_inputService.IsRightClickButtonDown())
            {
                Collider other = _cameraRayCast.GetCollider();
                IInteractive interactive = other?.GetComponent<IInteractive>();
                if (interactive != null)
                {
                    interactive.Interact();
                    OnInteractive?.Invoke(other);
                }
            }
            if(_inputService.IsRightClickButtonUp())
            {
                OnUnInteractive?.Invoke();
            }
        }
    }
}