using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Inidcators;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.UI;
using CodeBase.Weapons;
using UnityEngine;
using Zenject;

namespace CodeBase.WeaponsInventory
{
    public class PlayerWeaponInventory : IPlayerWeaponInventory
    {
        public WeaponType WeaponType
        {
            get => _currentWeaponType;
            set => _currentWeaponType = value;
        }

        public PlayerWeapon PlayerWeapon { get; set; }
        public WeaponSpawner WeaponSpawner { get; set; }

        private Dictionary<WeaponType, PlayerWeapon> _weaponsDictionary = new Dictionary<WeaponType, PlayerWeapon>();
        private List<WeaponType> _keyList = new List<WeaponType>();

        private WeaponType _currentWeaponType = WeaponType.FireStaff;
        private IStaticDataService _staticDataService;
        private IGameFactory _gameFactory;
        private IUpdateService _updateService;

        public event Action<PlayerWeapon> OnAddedNewWeapon;


        [Inject]
        public void Construct(IStaticDataService staticDataService, IGameFactory gameFactory,
            IUpdateService updateService)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _updateService = updateService;
        }

        public async void AddWeapon(WeaponType weaponType)
        {
            if (!_weaponsDictionary.ContainsKey(weaponType))
            {
                GameObject weaponGO = await WeaponSpawner.CreateWeapon(weaponType);
                PlayerWeapon playerWeapon = weaponGO.GetComponent<PlayerWeapon>();
                _weaponsDictionary.Add(weaponType, playerWeapon);
                playerWeapon.Construct(_gameFactory, _staticDataService, _updateService);
                _keyList.Add(weaponType);
                SetCurrentWeapon(weaponType);
                OnAddedNewWeapon?.Invoke(playerWeapon);
            }
        }

        public List<WeaponType> GetKeyList()
        {
            return _keyList;
        }

        public void CleanUp()
        {
            _weaponsDictionary.Clear();
        }

        public void SetCurrentWeapon(int index)
        {
            WeaponType key = _keyList[index];

            if (_currentWeaponType == key)
                return;

            if (PlayerWeapon.gameObject)
                PlayerWeapon.gameObject.SetActive(false);
            
            _currentWeaponType = key;
            PlayerWeapon = _weaponsDictionary[key];
            PlayerWeapon.gameObject.SetActive(true);
        }

        private void SetCurrentWeapon(WeaponType key)
        {
            if (PlayerWeapon)
            {
                PlayerWeapon.gameObject.SetActive(false);
            }

            _currentWeaponType = key;
            PlayerWeapon = _weaponsDictionary[key];
            PlayerWeapon.gameObject.SetActive(true);
        }

        public GameObject GetCurrentWeapon()
        {
            return PlayerWeapon.gameObject;
        }
    }
}