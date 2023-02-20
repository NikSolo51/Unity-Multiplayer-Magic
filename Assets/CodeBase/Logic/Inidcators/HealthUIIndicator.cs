using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic.Inidcators
{
    public class HealthUIIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;
        public virtual void AnimateIndicator(float percent)
        {
            _indicator.fillAmount = percent;
        }
    }
}