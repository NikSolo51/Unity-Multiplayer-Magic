using CodeBase.Services.Input;
using CodeBase.Services.Update;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class HeroMove : MonoBehaviour, IUpdatable
    {
        [SerializeField] private float _movementSpeed = 1;

        private IInputService _inputService;
        private IUpdateService _updateService;
        private PhotonView photonView;
        private Vector3 _startPos;
        private Vector3 _localRight;
        private Vector3 _localForward;
        private float _dotDirection;
        private bool _freeze;
        private float _slowDownModificator = 1;

        [Inject]
        public void Construct(IInputService inputService, IUpdateService updateService)
        {
            _inputService = inputService;
            _updateService = updateService;
            photonView = GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                _updateService.Register(this);
                _startPos = transform.position;
            }
        }

        private void OnDisable()
        {
            if (_updateService != null)
            {
                _updateService.Unregister(this);
            }
        }

        public void UpdateTick()
        {
            if (!photonView || !photonView.IsMine)
                return;
            
            if(_freeze)
                return;
            
            Vector3 movementAxis = Vector3.zero;
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementAxis = _inputService.Axis;
                movementAxis.z = movementAxis.y;
                movementAxis.y = 0;
                movementVector += transform.forward * movementAxis.z;
                movementVector += transform.right * movementAxis.x;
                movementVector.Normalize();
            }

            transform.position += _movementSpeed * _slowDownModificator * movementVector * Time.deltaTime;
        }

        public void Freeze()
        {
            _freeze = true;
        }

        public void UnFreeze()
        {
            _freeze = false;
        }

        public void SlowDown(float slowdownModificator)
        {
            _slowDownModificator = slowdownModificator;
        }

        public void UnSlowDown()
        {
            _slowDownModificator = 1;
        }

        public void ResetPosition()
        {
            transform.position = _startPos;
        }
    }
}