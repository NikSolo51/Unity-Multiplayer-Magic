using System;
using UnityEngine;

namespace CodeBase.Logic.Raycast
{
    public class CameraRayCast : MonoBehaviour
    {
        [SerializeField] private LayerMask _layermask;
        [SerializeField] private float _raycastLength = 1;
        private bool _enableRaycast = true;

        public Collider GetCollider()
        {
            if (!_enableRaycast)
                return null;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _raycastLength, _layermask))
            {
                return hit.collider;
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.forward * _raycastLength, Color.green);
        }

        public void EnableRayCast()
        {
            _enableRaycast = true;
        }

        public void DisableRayCast()
        {
            _enableRaycast = false;
        }
    }
}