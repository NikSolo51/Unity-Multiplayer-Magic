using System;
using System.Collections.Generic;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Logic.Inidcators
{
    public class ReloadUIIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;
        private List<PlayerWeapon> _signed = new List<PlayerWeapon>();
        private float _startIndicatorFill;
        private IPlayerWeaponInventory _playerWeaponInventory;
      
        [Inject]
        public void Construct(IPlayerWeaponInventory playerWeaponInventory)
        {
            _playerWeaponInventory = playerWeaponInventory;
            _playerWeaponInventory.OnAddedNewWeapon += SubscribeToNewWeapon;
        }

        private void Start()
        {
            _startIndicatorFill = _indicator.fillAmount;
        }

        public void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }

        public void SubscribeToNewWeapon(PlayerWeapon playerWeapon)
        {
            if(_signed.Contains(playerWeapon))
                return;
            _indicator.fillAmount = _startIndicatorFill;
            playerWeapon.OnReloadPercent += AnimateIndicator;
            _signed.Add(playerWeapon);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _signed.Count; i++)
            {
                if (_signed[i])
                {
                    _signed[i].OnReloadPercent -= AnimateIndicator;
                }
            }
            
            _playerWeaponInventory.OnAddedNewWeapon -= SubscribeToNewWeapon;
        }
    }
}