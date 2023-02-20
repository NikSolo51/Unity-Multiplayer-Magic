using System;
using CodeBase.Logic.Interactive;
using CodeBase.PlayerScripts;
using UnityEngine;

namespace CodeBase.Logic
{
    public class ThrowingObject : MonoBehaviour
    {
        [SerializeField] private Transform _directionTransform;
        [SerializeField] private PickUpObject _pickUpObject;
        [SerializeField] private float _force;
      
            
        private void OnEnable()
        {
            _pickUpObject.OnUnPickUp += Throwing;
        }

        private void OnDestroy()
        {
            _pickUpObject.OnUnPickUp -= Throwing;
        }

        private void Throwing(PickUp pickUp)
        {
            pickUp._rigidbody.AddForce(_directionTransform.forward * _force,ForceMode.Impulse);
        }
    }
}