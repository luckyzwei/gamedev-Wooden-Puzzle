using NutBolts.Scripts.Data;
using TMPro;
using UnityEngine;

using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIGame
{
    public class UIGameMenu : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKLayerController _vkLayerController;
        [Inject] private DataMono _dataMono;
        [SerializeField] private VKCountDownLite _countDown;
        [SerializeField] private CBoosterUI[] _abilities;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private VKCountDownLite _countdown;

        public VKCountDownLite Countdown => _countdown;
        private void OnEnable()
        {
            GameManager.OnWin += Wictory;
        }
        private void OnDisable()
        {
            GameManager.OnWin -= Wictory;
        }
        public override void Close()
        {
            base.Close();
            Clear();
        }
        
        public void Construct(int seconds=60)
        {
            foreach (var booster in _abilities)
            {
                booster.Construct();
            }
            _countDown.SetSeconds(seconds);
            _countDown.StartCountDown();
            _countDown.OnCountDownComplete = GameLose;
            _levelText.text = $"LEVEL {GameManager.level}";
        }
        #region Listenner
        public void OpenSettings()
        {
            _vkLayerController.ShowLayer("UIPause");
            _vkAudioController.PlaySound("Button");
            _countdown.IsPause = true;
        }

        #endregion
        #region method

        private void Wictory()
        {
            Clear();
            _dataMono.Data.LevelCompleted(GameManager.level);
            var uiWin = (UIWin.UIWin)_vkLayerController.ShowLayer("UIWin");
            uiWin.Construct();
        }

        private void GameLose()
        {
            _vkAudioController.PlaySound($"Game_over_{Random.Range(1, 3)}");
            _vkLayerController.ShowLayer("UIFail");
        }

        private void Clear()
        {      
            _countDown.OnCountDownComplete = null;
            _countDown.StopCountDown();
       
        }
        public void AddTime(int addTime)
        {
            _countDown.SetSeconds( addTime);
            _countDown.OnCountDownComplete = GameLose;
            _countDown.StartCountDown();
        }
   
        #endregion

    }
}
