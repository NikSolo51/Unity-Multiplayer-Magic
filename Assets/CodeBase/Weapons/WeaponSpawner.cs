using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Weapons
{
    public class WeaponSpawner : MonoBehaviour,  IWeaponSpawner
    {
        [SerializeField] private Transform _weaponInitPoint;
        private IGameFactory _gameFactory;
        
        [Inject]
        public void Constructor(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public async Task<GameObject> CreateWeapon(WeaponType weaponType)
        {
            GameObject weaponGO = await _gameFactory.CreateWeapon(weaponType, _weaponInitPoint);
            weaponGO.transform.SetParent(_weaponInitPoint);
            weaponGO.transform.rotation = _weaponInitPoint.rotation;
            return weaponGO;
        }
    }
}