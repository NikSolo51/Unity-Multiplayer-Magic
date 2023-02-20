using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Weapons
{
    public interface IWeapon
    {
        public WeaponType WeaponType { get; }
        public ProjectileType ProjectileType { get; set; }
        public bool NotEmpty { get; set; }
        public int MaxAmmo { get; set; }
        public float ShootDelay { get; set; }
        public float ProjectileSpeed { get; set; }
        public void Shoot();
    }
}