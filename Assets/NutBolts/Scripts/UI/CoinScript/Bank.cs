using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace NutBolts.Scripts.UI.CoinScript
{
    internal class Bank : MonoBehaviour
    {
        private int _coinAmount;
        private Text _coinText;

        private void Start()
        {
            _coinText = GetComponent<Text>();
        }

        private void Update()
        {
            _coinAmount = DataMono.Instance.Data.Coins;
            _coinText.text = _coinAmount.ToString("N0");
        }
    }
}
