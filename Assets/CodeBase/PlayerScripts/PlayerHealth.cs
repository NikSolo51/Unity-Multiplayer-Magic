using System;
using CodeBase.Services.StaticData;
using Photon.Pun;
using UnityEngine;

namespace CodeBase.PlayerScripts
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float MaxHp = 100;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private PlayerDeath _playerDeath;
        [SerializeField] private EffectsSystem _effectsSystem;
        public event Action<float> OnHpPercent;
        public event Action<float> OnHp;
        private float CurrentHP;

        public void ResetHP() => CurrentHP = MaxHp;

        private void Start()
        {
            CurrentHP = MaxHp;
        }

        public float Current
        {
            get => CurrentHP;
            set
            {
                if (CurrentHP != value)
                {
                    CurrentHP = value;
                }
            }
        }

        [PunRPC]
        public void TakeDamage(float damage, int effectTypeId)
        {
            if (!_photonView.IsMine)
                return;
            Current -= damage;
            OnHpPercent?.Invoke(Current / MaxHp);
            OnHp?.Invoke(Current);
           
            if (Current <= 0)
            {
                PlayerDeath();
                return;
            }
            
            EffectType effectType = (EffectType) effectTypeId;

            _effectsSystem.SetEffect(effectType);
        }
        
        [PunRPC]
        public void TakeDamage(float damage)
        {
            if (!_photonView.IsMine)
                return;
            
            Current -= damage;
            OnHpPercent?.Invoke(Current / MaxHp);
            OnHp?.Invoke(Current);
            
            if (Current <= 0)
            {
                PlayerDeath();
                return;
            }
        }

        private void PlayerDeath()
        {
            _effectsSystem.ResetEffectSystem();
            _playerDeath.Die();
            ResetHP();
            OnHpPercent?.Invoke(Current / MaxHp);
            OnHp?.Invoke(Current);
        }
    }
}