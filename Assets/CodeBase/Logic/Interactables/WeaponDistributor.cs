using System;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Interactive
{
    public class WeaponDistributor : MonoBehaviour, IInteractive
    {
        [SerializeField] private WeaponType _weaponType;
        private IPlayerWeaponInventory _playerWeaponInventory;

        public event Action OnInteract;

        public event Action OnUnInteract;
    
        [Inject]
        public void Constructor(IPlayerWeaponInventory playerWeaponInventory)
        {
            _playerWeaponInventory = playerWeaponInventory;
        }
        public void Interact()
        {
            _playerWeaponInventory.AddWeapon(_weaponType);
            OnInteract?.Invoke();
        }

        public void UnInteract()
        {
            OnUnInteract?.Invoke();
        }
    }
}