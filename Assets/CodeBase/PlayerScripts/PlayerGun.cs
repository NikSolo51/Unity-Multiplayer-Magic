using CodeBase.Services.Input;
using CodeBase.WeaponsInventory;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class PlayerGun : MonoBehaviour
    {
        private IInputService _inputService;
        private IPlayerWeaponInventory _playerWeaponInventory;

        [Inject]
        public void Construct(IInputService inputService, IPlayerWeaponInventory heroWeaponInventory)
        {
            _inputService = inputService;
            _playerWeaponInventory = heroWeaponInventory;
        }

        private void Update()
        {
            if (_inputService != null)
            {
                if (_inputService.IsClickButtonPress())
                {
                    Shoot();
                }
            }
        }

        public void Shoot()
        {
            if (_playerWeaponInventory.PlayerWeapon != null)
            {
                _playerWeaponInventory.PlayerWeapon.Shoot();
            }
        }
    }
}