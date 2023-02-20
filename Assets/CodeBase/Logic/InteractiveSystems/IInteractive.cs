using System;
using UnityEngine;

namespace CodeBase.Logic.Interactive
{
    public interface IInteractive
    {
        void Interact();
        void UnInteract();
        public event Action OnInteract;
        public event Action OnUnInteract;
        
    }
}