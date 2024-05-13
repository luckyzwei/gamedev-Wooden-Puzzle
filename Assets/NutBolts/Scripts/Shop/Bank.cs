using System;
using UnityEngine;

namespace Game.Scripts.Shop
{
    public class Bank
    {
        public event Action OnValuesChange; 
        private const string KEY_COINS = "Coins", KEY_GEMS = "Gems";
        private const int START_COINS = 10000, START_GEMS = 1000;
        private int _coins;
        private int _gems;
        public int Coins => _coins;
        public int Gems => _gems;
        
        public Bank()
        {
            _coins = PlayerPrefs.GetInt(KEY_COINS, START_COINS);
            _gems = PlayerPrefs.GetInt(KEY_GEMS, START_GEMS);
        }

        public void ChangeCoins(int change)
        {
            _coins += change;
            _coins = Math.Max(_coins, 0);
            PlayerPrefs.SetInt(KEY_COINS, _coins);
            OnValuesChange?.Invoke();
        }

        public void GemsChange(int change)
        {
            _gems += change;
            _gems = Math.Max(_gems, 0);
            PlayerPrefs.SetInt(KEY_GEMS, _gems);
            Debug.Log(_gems);
            OnValuesChange?.Invoke();
        }
    }
}