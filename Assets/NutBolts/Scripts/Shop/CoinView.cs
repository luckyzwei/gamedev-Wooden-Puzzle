using Game.Scripts.Shop;
using TMPro;
using UnityEngine;
using Zenject;

namespace NutBolts.Scripts.Shop
{
    public class CoinView : MonoBehaviour
    {
        [Inject] private Bank _bank;
        [SerializeField] private bool _isGem;
        [SerializeField] private TMP_Text _coinText;

        private void OnEnable()
        {
            ChangeValues();
            _bank.OnValuesChange += ChangeValues;
        }

        private void OnDisable()
        {
            _bank.OnValuesChange -= ChangeValues;
        }

        private void ChangeValues()
        {
            Debug.Log(_isGem);
            _coinText.text = _isGem ? _bank.Gems.ToString() : _bank.Coins.ToString();
        }
    }
}