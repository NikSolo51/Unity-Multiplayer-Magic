using System.Collections;
using CodeBase.PlayerScripts;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.StaticData
{
    public class EffectsSystem : MonoBehaviour
    {
        [SerializeField] private HeroMove _heroMove;
        [SerializeField] private PhotonView _photonView;
        private IStaticDataService _staticDataService;
        
        [Inject]
        public void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void SetEffect(EffectType effectType)
        {
            EffectStaticData effectStaticData = _staticDataService.ForEffect(effectType);
            switch (effectType)
            {
                case EffectType.Nothing: return;
                case EffectType.Burning:
                {
                    PereodicDamage(effectStaticData);
                    return;
                }

                case EffectType.Frezze:
                {
                    Freeze(effectStaticData);
                    return;
                }
                case EffectType.Slowdown:
                {
                    SlowDown(effectStaticData);
                    return;
                }
                case EffectType.FrezzeBurning:
                {
                    PereodicDamage(effectStaticData);
                    Freeze(effectStaticData);
                    return;
                }
                   
            }
        }

        public void ResetEffectSystem()
        {
            StopAllCoroutines();
        }

        private void SlowDown(EffectStaticData effectStaticData)
        {
            StartCoroutine(SlowDownCoroutine(effectStaticData));
        }

        private IEnumerator SlowDownCoroutine(EffectStaticData effectStaticData)
        {
            float time = 0;
            _heroMove.SlowDown(effectStaticData.SlowdownModification);
            while (time < effectStaticData.DurationSlowdownEffect)
            {
                time += Time.deltaTime;
                yield return null;
            }
            _heroMove.UnSlowDown();
        }

        public void PereodicDamage(EffectStaticData effectStaticData)
        {
            StartCoroutine(PereodicDamageCoroutine(effectStaticData));
        }

        public void Freeze(EffectStaticData effectStaticData)
        {
            StartCoroutine(FreezeCoroutine(effectStaticData));
        }

        IEnumerator FreezeCoroutine(EffectStaticData effectStaticData)
        {
            float time = 0;
            _heroMove.Freeze();
            while (time < effectStaticData.DurationFreezeEffect)
            {
                time += Time.deltaTime;
                yield return null;
            }
            _heroMove.UnFreeze();
        }
        
        IEnumerator PereodicDamageCoroutine(EffectStaticData effectStaticData)
        {
            float appliedTimes = 0;
            float tickTime = effectStaticData.DurationPeriodicDamageEffect /effectStaticData.Frequency;
            
            while (appliedTimes < effectStaticData.DurationPeriodicDamageEffect)
            {
                appliedTimes += tickTime;
                yield return new WaitForSeconds(tickTime);
                _photonView.RPC("TakeDamage",RpcTarget.All,effectStaticData.Damage);
            }
        }
    }
}