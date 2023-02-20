using System;
using System.Collections.Generic;
using CodeBase.Services.Input;
using CodeBase.Services.Update;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class PlayerWeaponScroller : MonoBehaviour, IUpdatable
    {
        private int _id;
        private IInputService _inputService;
        private IPlayerWeaponInventory _playerWeaponInventory;
        private IUpdateService _updateService;

        [Inject]
        public void Construct(IInputService inputService, IPlayerWeaponInventory playerWeaponInventory,
            IUpdateService updateService)
        {
            _inputService = inputService;
            _playerWeaponInventory = playerWeaponInventory;
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDestroy()
        {
            if (_updateService != null)
            {
                _updateService.Unregister(this);
            }
        }

        public void UpdateTick()
        {
            if (_inputService != null)
            {
                float scrollAxis = _inputService.ScrollAxis;

                if (scrollAxis > 0)
                {
                    _id += 1;
                    ChangeWeaponAt(_id);
                }
                else if (scrollAxis < 0)
                {
                    _id -= 1;
                    ChangeWeaponAt(_id);
                }
            }
        }

        private void ChangeWeaponAt(int at)
        {
            List<WeaponType> keys = _playerWeaponInventory.GetKeyList();
            
            if (keys.Count == 0)
                return;
            
            if (at < 0)
            {
                at = keys.Count - 1;
            }
            if (at >= keys.Count)
            {
                at = 0;
            }

            _id = at;
         
            _playerWeaponInventory.SetCurrentWeapon(at);
        }
    }
}