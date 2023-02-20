using System;
using CodeBase.Services.StaticData;

namespace CodeBase.PlayerScripts
{
    public interface IHealth
    {
        void TakeDamage(float damage, int effectTypeId);
        public event Action<float> OnHpPercent;
    }
}