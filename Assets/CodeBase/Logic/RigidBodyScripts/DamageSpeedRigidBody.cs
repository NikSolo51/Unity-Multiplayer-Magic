using System;
using System.Collections;
using CodeBase.Projectiles;
using CodeBase.Services.StaticData;
using CodeBase.Weapons;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.RigidBodyScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class DamageSpeedRigidBody : ProjectileAbstract
    {
        [SerializeField] private ProjectileType _projectileType;
        [SerializeField] private LayerMask _damagableLayer;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _velocityMagnitudeBarrier = 1;
        [SerializeField] private float _delay = 1;
        private bool _canDamage = true;
        private IStaticDataService _staticDataService;
    
        [Inject]
        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
             ProjectileStaticData projectileStaticData =_staticDataService.ForProjectile(_projectileType);
             Damage = projectileStaticData.Damage;
             EffectType = projectileStaticData.EffectType;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!_canDamage)
                return;
            
            if (_damagableLayer.value == (_damagableLayer | (1 << other.gameObject.layer)))
            {
                if(_rigidBody.velocity.magnitude < _velocityMagnitudeBarrier)
                    return;
                        
                PhotonView otherPhotonView = other?.GetComponent<PhotonView>();
                if (otherPhotonView && !otherPhotonView.IsMine)
                {
                    otherPhotonView.RPC("TakeDamage", RpcTarget.All, Damage);
                    StartCoroutine(DelayCoroutine());
                }
            }
        }

        IEnumerator DelayCoroutine()
        {
            float time = 0;
            _canDamage = false;
            while (time < _delay)
            {
                time += Time.deltaTime;
                yield return null;
            }

            _canDamage = true;
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}