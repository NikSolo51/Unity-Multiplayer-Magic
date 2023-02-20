using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "EffectData", menuName = "StaticData/Effect")]
    public class EffectStaticData : ScriptableObject
    {
        public EffectType EffectType;

        public bool PeriodicDamageEffect;
        [ShowIf("PeriodicDamageEffect")] public float Damage;
        [ShowIf("PeriodicDamageEffect")] public float DurationPeriodicDamageEffect;
        [ShowIf("PeriodicDamageEffect")] public float Frequency;

        public bool FreezeEffect;
        [ShowIf("FreezeEffect")] public float DurationFreezeEffect;

        public bool SlowdownEffect;
        [ShowIf("SlowdownEffect")] public float DurationSlowdownEffect;
        [ShowIf("SlowdownEffect")] public float SlowdownModification;
    }
}