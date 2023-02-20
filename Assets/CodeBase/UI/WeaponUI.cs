using CodeBase.Services.StaticData;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private TMP_Text _textTMP;
        private IPlayerWeaponInventory _playerWeaponInventory;

        [Inject]
        public void Constructor(IPlayerWeaponInventory playerWeaponInventory)
        {
            _playerWeaponInventory = playerWeaponInventory;
        }

        public void Initialize(WeaponStaticData weaponStaticData)
        {
            weaponType = weaponStaticData.WeaponType;
            _textTMP.text = weaponType.ToString();
        }

        public void SelectWeapon()
        {
            _playerWeaponInventory.WeaponType = weaponType;
        }
    }
}