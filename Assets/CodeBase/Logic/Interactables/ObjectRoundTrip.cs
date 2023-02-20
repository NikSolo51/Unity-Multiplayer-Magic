using System;
using CodeBase.Services.Update;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Interactive
{
    public class ObjectRoundTrip : MonoBehaviour, IInteractive, IUpdatable
    {
        [SerializeField] private GameObject _object;
        [SerializeField] private Transform _firstPoint;
        [SerializeField] private Transform _secondPoint;
        [SerializeField] private float _speed;
        [TextAreaAttribute]
        [SerializeField] private string _Description;
        [SerializeField] private bool _switch;
        private IUpdateService _updateService;
        public event Action OnInteract;
        public event Action OnUnInteract;

        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDestroy()
        {
            _updateService.Unregister(this);
        }

        public void Interact()
        {
            _switch = !_switch;
            OnInteract?.Invoke();
        }

        public void UnInteract()
        {
            OnUnInteract?.Invoke();
        }

        public void UpdateTick()
        {
            if (_switch)
            {
                float magnitude = (_secondPoint.transform.position - _object.transform.position).magnitude;
                if (magnitude <= 0)
                    return;
                _object.transform.position =
                    Vector3.Lerp(_object.transform.position, _secondPoint.transform.position, Time.deltaTime * _speed);
            }
            else
            {
                float magnitude = (_firstPoint.transform.position - _object.transform.position).magnitude;
                if (magnitude <= 0)
                    return;
                _object.transform.position =
                    Vector3.Lerp(_object.transform.position, _firstPoint.transform.position, Time.deltaTime * _speed);
            }
        }
    }
}