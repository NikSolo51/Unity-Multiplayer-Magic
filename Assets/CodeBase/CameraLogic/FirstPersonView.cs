using System;
using CodeBase.Services.Input;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.CameraLogic
{
    public class FirstPersonView : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private Transform _body;
        [SerializeField] private float SensitivityX;
        [SerializeField] private float SensitivityY;

        private Vector3 Angles = Vector3.zero;
        private Vector2 ViewAxis = Vector2.zero;
        private IInputService _viewInputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _viewInputService = inputService;
        }

        private void Start()
        {
            if (!_photonView.IsMine)
                gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (!_photonView.IsMine)
                return;

            if (_body == null || _viewInputService == null)
                return;

            ViewAxis = CalculateViewSensitivity();

            _body.Rotate(0, ViewAxis.x, 0);

            CameraFollowingView();
        }

        private Vector2 CalculateViewSensitivity()
        {
            Vector2 ViewAxis = new Vector2(_viewInputService.ViewAxis.x * SensitivityX,
                _viewInputService.ViewAxis.y * SensitivityY);
            return ViewAxis;
        }

        private void CameraFollowingView()
        {
            if (ViewAxis.y > 0)
                Angles = new Vector3(Mathf.MoveTowards(Angles.x, -40, ViewAxis.y), Angles.y, 0);
            else
                Angles = new Vector3(Mathf.MoveTowards(Angles.x, 40, -ViewAxis.y), Angles.y, 0);

            transform.localEulerAngles = Angles;
        }
    }
}