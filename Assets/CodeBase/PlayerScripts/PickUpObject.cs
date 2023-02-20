using System;
using CodeBase.Logic.Interactive;
using CodeBase.Logic.Raycast;
using CodeBase.Services.Input;
using CodeBase.Services.Update;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class PickUpObject : MonoBehaviour, IUpdatable
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private InteractiveSystem _interactiveSystem;
        [SerializeField] private Transform _attractionPoint;
        [SerializeField] private float _attractionSpeed = 2;
        private PickUp _pickUp;
        private IUpdateService _updateService;
        private IInputService _inputService;
        public event Action OnPickUp;
        public event Action<PickUp> OnUnPickUp;


        [Inject]
        public void Constructor(IUpdateService updateService, IInputService inputService)
        {
            _updateService = updateService;
            _inputService = inputService;
            if (_photonView.IsMine)
                _updateService.Register(this);
        }

        private void OnEnable()
        {
            _interactiveSystem.OnInteractive += PickUp;
            _interactiveSystem.OnUnInteractive += UnPickUp;
        }

        private void OnDestroy()
        {
            if (_photonView.IsMine)
                _updateService.Unregister(this);
            _interactiveSystem.OnInteractive -= PickUp;
            _interactiveSystem.OnUnInteractive -= UnPickUp;
        }

        private void PickUp(Collider other)
        {
            if (other)
            {
                _pickUp = other?.GetComponent<PickUp>();
                if (_pickUp)
                {
                    _pickUp.Interact();
                    OnPickUp?.Invoke();
                }
            }
        }

        private void UnPickUp()
        {
            if (_pickUp)
            {
                _pickUp.UnInteract();
                OnUnPickUp?.Invoke(_pickUp);
                _pickUp = null;
            }
        }

        public void UpdateTick()
        {
            if (_pickUp)
            {
                _pickUp.gameObject.transform.position = Vector3.Lerp(
                    _pickUp.transform.position,
                    _attractionPoint.transform.position, Time.deltaTime * _attractionSpeed);
            }
        }
    }
}