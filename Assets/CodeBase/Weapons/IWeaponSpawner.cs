using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Weapons
{
    public interface IWeaponSpawner
    {
        Task<GameObject> CreateWeapon(WeaponType weaponType);
    }
}