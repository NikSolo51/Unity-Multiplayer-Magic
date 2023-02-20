using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Projectiles
{
    public abstract class ProjectileAbstract : MonoBehaviour
    {
        
        [HideInInspector]public float Damage;
        [HideInInspector]public EffectType EffectType;
        public abstract void Destroy();
    }
}