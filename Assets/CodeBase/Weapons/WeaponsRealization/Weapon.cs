using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Projectiles;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using Photon.Pun;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Weapons.WeaponsRealization
{
    public class Weapon : PlayerWeapon
    {
        [SerializeField] private Transform _shootOrigin;
        [SerializeField] private PhotonView _photonView;
        private float _lastShootTime;
        private bool _reloading;
        private float _reloadingTime;
        public override event Action<float> OnReloadPercent;
        public override event Action<int,int> OnAmmoCount;

        private void Start()
        {
            OnAmmoCount?.Invoke(CurrentAmmo,MaxAmmo);
        }

        private void OnDisable()
        {
            _reloading = false;
        }

        private void OnDestroy()
        {
            if (_updateService != null)
            {
                _updateService.Unregister(this);
            }
        }

        public override void Shoot()
        {
            if (!_photonView)
                return;

            
            if (!_reloading)
            {
                if (_lastShootTime + ShootDelay < Time.time)
                {
                    ProjectileStaticData projectileStaticData = _staticDataService.ForProjectile(ProjectileType);

                    GameObject projectileGo = SpawnProjectile();

                    InitializeProjectile(projectileGo, projectileStaticData);


                    CurrentAmmo--;

                    OnAmmoCount?.Invoke(CurrentAmmo,MaxAmmo);

                    _lastShootTime = Time.time;
                }
            }
        }

        private void InitializeProjectile(GameObject projectileGo, ProjectileStaticData projectileStaticData)
        {
            ProjectileAbstract projectileAbstract = projectileGo.GetComponent<ProjectileAbstract>();
            projectileAbstract.Damage = projectileStaticData.Damage;
            projectileAbstract.EffectType = projectileStaticData.EffectType;
        }

        private GameObject SpawnProjectile()
        {
            GameObject projectileGo =
                _gameFactory.CreateProjectile(ProjectileType, _shootOrigin.position, _shootOrigin.rotation);
            Rigidbody projectileRigidbody = projectileGo.GetComponent<Rigidbody>();
            projectileRigidbody.AddForce(_shootOrigin.forward * projectileRigidbody.mass * ProjectileSpeed,
                ForceMode.Impulse);
            return projectileGo;
        }

        public override void UpdateTick()
        {
            if (!gameObject.activeSelf)
                return;
            OnAmmoCount?.Invoke(CurrentAmmo,MaxAmmo);
            if (CurrentAmmo <= 0)
            {
                if (!_reloading)
                {
                    _reloadingTime = 0;
                    _reloading = true;
                }

                while (_reloadingTime < ReloadDelay)
                {
                    _reloadingTime += Time.deltaTime;
                    OnReloadPercent?.Invoke(_reloadingTime/ReloadDelay);
                    return;
                }

                CurrentAmmo = MaxAmmo;
                _reloading = false;
            }
        }
    }
}