using System;
using CodeBase.PlayerScripts;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace CodeBase.Logic.WorldUI
{
    public class WorldHP : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private TMP_Text _hpText;

        private void OnEnable()
        {
            _playerHealth.OnHp += UpdateHPText;
        }

        private void UpdateHPText(float hp)
        {
            _photonView.RPC("UpdateHPTextRPC",RpcTarget.AllBuffered,hp);
        }
        [PunRPC]
        private void UpdateHPTextRPC(float hp)
        {
            
            if (!_photonView.IsMine)
            {
                _hpText.text = hp.ToString();
            }
        }
    }
}