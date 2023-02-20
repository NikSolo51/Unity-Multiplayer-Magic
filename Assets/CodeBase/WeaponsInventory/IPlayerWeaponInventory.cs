using System;
using System.Collections.Generic;
using CodeBase.Services.SaveLoad;
using CodeBase.Weapons;
using UnityEngine;

namespace CodeBase.WeaponsInventory
{
    public interface IPlayerWeaponInventory
    {
        WeaponType WeaponType { get; set; }
        PlayerWeapon PlayerWeapon { get; set; }
        WeaponSpawner WeaponSpawner { get; set; }
        event Action<PlayerWeapon> OnAddedNewWeapon; 
        void AddWeapon(WeaponType weaponType);
        List<WeaponType> GetKeyList();
        void SetCurrentWeapon(int index);
        GameObject GetCurrentWeapon();
        void CleanUp();
    
    }
}