using CodeBase.SoundManager;
using CodeBase.Weapons;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        void Initialize();
        LevelStaticData ForLevel(string sceneKey);

        SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType);
        WeaponStaticData ForWeapon(WeaponType weaponType);
        ProjectileStaticData ForProjectile(ProjectileType projectileType);
        WeaponStaticData[] AllWeapons();
        EffectStaticData ForEffect(EffectType effectType);
    }
}