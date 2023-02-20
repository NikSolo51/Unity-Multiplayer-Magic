using System.Collections.Generic;
using System.Linq;
using CodeBase.SoundManager;
using CodeBase.Weapons;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<SoundManagerType, SoundManagerStaticData> _soundManagers;
        private Dictionary<WeaponType, WeaponStaticData> _weapons;
        private Dictionary<ProjectileType, ProjectileStaticData> _projectiles;
        private Dictionary<EffectType, EffectStaticData> _effects;

        public void Initialize()
        {
            IList<LevelStaticData> resource =
                LoadResources<LevelStaticData>("Level");
            
            _levels = resource.ToDictionary(x => x.LevelKey, x => x);
            
            
            IList<SoundManagerStaticData> soundSystems =
                LoadResources<SoundManagerStaticData>("SoundManager");
            
            _soundManagers = soundSystems.ToDictionary(x => x.SoundManagerType, x => x);
            
            
            IList<WeaponStaticData> weapons =
                LoadResources<WeaponStaticData>("WeaponData");
            
            _weapons = weapons.ToDictionary(x => x.WeaponType, x => x);
            
            
            IList<ProjectileStaticData> projectiles =
                LoadResources<ProjectileStaticData>("ProjectileData");
            
            _projectiles = projectiles.ToDictionary(x => x.ProjectileType, x => x);
            
            IList<EffectStaticData> effects =
                LoadResources<EffectStaticData>("EffectData");
            
            _effects = effects.ToDictionary(x => x.EffectType, x => x);
        }

        private IList<T> LoadResources<T>(string dataName)
        {
            IList<IResourceLocation> resourceLocations =
                Addressables.LoadResourceLocationsAsync(dataName, typeof(T))
                    .WaitForCompletion();
            IList<T> resource =
                Addressables.LoadAssets<T>(resourceLocations, null)
                    .WaitForCompletion();
            return resource;
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            return _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
        }

        public SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType)
        {
            return _soundManagers.TryGetValue(soundManagerType, out SoundManagerStaticData staticData)
                ? staticData
                : null;
        }

        public WeaponStaticData ForWeapon(WeaponType weaponType)
        {
            return _weapons.TryGetValue(weaponType, out WeaponStaticData staticData)
                ? staticData
                : null;
        }

        public ProjectileStaticData ForProjectile(ProjectileType projectileType)
        {
            return _projectiles.TryGetValue(projectileType, out ProjectileStaticData staticData)
                ? staticData
                : null;
        }
        
        public EffectStaticData ForEffect(EffectType effectType)
        {
            return _effects.TryGetValue(effectType, out EffectStaticData staticData)
                ? staticData
                : null;
        }

        public WeaponStaticData[] AllWeapons()
        {
            return _weapons.Values.ToArray();
        }
    }
}