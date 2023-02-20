using System;
using CodeBase.PlayerScripts;
using Photon.Pun;
using UnityEngine;

namespace CodeBase.Projectiles
{
    public class Projectile : ProjectileAbstract
    {
        [SerializeField] private LayerMask _damagableLayer;

        private void OnTriggerEnter(Collider other)
        {

            if (_damagableLayer.value  == (_damagableLayer | (1 << other.gameObject.layer))) 
            {
                PhotonView otherPhotonView = other?.GetComponent<PhotonView>();
                
                if (otherPhotonView && !otherPhotonView.IsMine)
                {
                    int effectTypeId = (int)EffectType;
                    otherPhotonView.RPC("TakeDamage", RpcTarget.All, Damage,effectTypeId);
                }
            }

            Destroy();
        }

        public override void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}