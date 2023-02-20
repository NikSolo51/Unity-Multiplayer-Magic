using System;
using CodeBase.Services.Input;
using CodeBase.Services.Update;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class PlayerJump : MonoBehaviour, IUpdatable
    {
        [SerializeField] private float _jumpForce = 2;
        [SerializeField] private float _raycstLenght = 2;
        [SerializeField] private LayerMask _groundLayers;
        private PhotonView _photonView;
        private Rigidbody _rigidbody;
        private float groundDrag;
        private bool _grounded;
        private bool _jump;
        private IInputService _inputService;
        private IUpdateService _updateService;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        [Inject]
        public void Construct(IInputService inputService, IUpdateService updateService)
        {
            _inputService = inputService;
            _updateService = updateService;
            if (_photonView.IsMine)
            {
                _updateService.Register(this);
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
            if (_inputService == null)
                return;
            if (!_photonView || !_photonView.IsMine)
                return;
            
            GroundCheck();
          
            if (_inputService.IsJumpButtonDown() && _grounded)
            {
                Jump();
            }
        }

        private void Jump()
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            _jump = false;
        }


        private void GroundCheck()
        {
            _grounded = Physics.Raycast(transform.position, Vector3.down, _raycstLenght * 0.5f, _groundLayers);
        }
    }
}