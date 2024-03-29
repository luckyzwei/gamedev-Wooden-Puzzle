using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace NutBolts.Scripts.UI.CoinScript
{
    internal class CointScript : MonoBehaviour
    {
        public int Coin;
        private Text textCoin;

        private void Start()
        {
            textCoin = GetComponent<Text>();
        }

        private void Update()
        {
            Coin = UserData.Instance.CGameData.TotalCoin;
            textCoin.text = Coin.ToString("N0");
        }
    }
}
