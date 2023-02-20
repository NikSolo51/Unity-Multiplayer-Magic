using CodeBase.Weapons;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "StaticData/Projectile")]
    public class ProjectileStaticData : ScriptableObject
    {
        public ProjectileType ProjectileType;
        public EffectType EffectType;
        public float Damage;
    }
}