using CodeBase.Weapons;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
    public class WeaponStaticData : ScriptableObject
    {
        public WeaponType WeaponType;
        public ProjectileType ProjectileType;
        public int MagazineCount;
        public float ShootDelay = 0.2f;
        public float ReloadDelay = 1;
        public float ProjectileSpeed = 1;
    }
}