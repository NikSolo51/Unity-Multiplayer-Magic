using System;
using System.Collections.Generic;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class AmmoCountUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentAmmoText;
        [SerializeField] private TMP_Text _maxAmmoText;
        private List<PlayerWeapon> _signed = new List<PlayerWeapon>();
        private IPlayerWeaponInventory _playerWeaponInventory;

        [Inject]
        public void Construct(IPlayerWeaponInventory playerWeaponInventory)
        {
            _playerWeaponInventory = playerWeaponInventory;
            _playerWeaponInventory.OnAddedNewWeapon += SubscribeToNewWeapon;
        }
        
        public void InitializeMaxAmmo(int maxAmmo)
        {
            _maxAmmoText.text = maxAmmo.ToString();
        }

        public void UpdateAmmoText(int count,int maxAmmo)
        {
            InitializeMaxAmmo(maxAmmo);
            _currentAmmoText.text = count.ToString();
        }

        public void SubscribeToNewWeapon(PlayerWeapon playerWeapon)
        {
            if(_signed.Contains(playerWeapon))
                return;
            
            playerWeapon.OnAmmoCount += UpdateAmmoText;
            _signed.Add(playerWeapon);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _signed.Count; i++)
            {
                if (_signed[i])
                {
                    _signed[i].OnAmmoCount -= UpdateAmmoText;
                }
            }
            
            _playerWeaponInventory.OnAddedNewWeapon -= SubscribeToNewWeapon;
        }
    }
}