using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.Weapons.WeaponsRealization;
using UnityEngine;
using Zenject;

namespace CodeBase.Weapons
{
    public abstract class PlayerWeapon : MonoBehaviour, IWeapon,IUpdatable
    {
        public WeaponType WeaponType { get; set; }
        public ProjectileType ProjectileType { get; set; }
        public bool NotEmpty { get; set; }
        public int MaxAmmo { get; set; }
        public float ShootDelay { get; set; }
        public float ProjectileSpeed { get; set; }
        public float ReloadDelay { get; set; }
        public int CurrentAmmo { get; set; }

        protected IGameFactory _gameFactory;
        protected IStaticDataService _staticDataService;
        protected IUpdateService _updateService;
        public abstract event Action<float> OnReloadPercent;
        public abstract event Action<int,int> OnAmmoCount;

        [Inject]
        public  void Construct(IGameFactory gameFactory, IStaticDataService staticDataService,IUpdateService updateService)
        {
            _gameFactory = gameFactory;
            _staticDataService = staticDataService;
            _updateService = updateService;
            _updateService.Register(this);
            CurrentAmmo = MaxAmmo;
        }

        public abstract void Shoot();
        public abstract void UpdateTick();
    }
}