using System;
using UnityEngine;

namespace CodeBase.Logic.Interactive
{
    public class PickUp : MonoBehaviour, IInteractive
    {
        public Rigidbody _rigidbody;
        public event Action OnInteract;
        public event Action OnUnInteract;
        
        public void Interact()
        {
            _rigidbody.isKinematic = true;
            OnInteract?.Invoke();
        }

        public void UnInteract()
        {
            _rigidbody.isKinematic = false;
            OnUnInteract?.Invoke();
        }

        
    }
}