using NutBolts.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NutBolts.Scripts.UI.CoinScript
{
    internal class Bank : MonoBehaviour
    {
        [Inject] private DataMono _dataMono;
        private int _coinAmount;
        private TMP_Text _coinText;

        private void Start()
        {
            _coinText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _coinAmount = _dataMono.Data.Coins;
            _coinText.text = _coinAmount.ToString("N0");
        }
    }
}
