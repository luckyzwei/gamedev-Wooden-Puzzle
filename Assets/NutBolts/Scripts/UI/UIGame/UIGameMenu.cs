using TMPro;
using UnityEngine;

using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIGame
{
    public class UIGameMenu : VKLayer
    {
        [SerializeField] private VKCountDownLite _countDown;
        [SerializeField] private CBoosterUI[] _abilities;
        [SerializeField] private TextMeshProUGUI _levelText;
        private void OnEnable()
        {
            CLevelManager.OnWin += Wictory;
        }
        private void OnDisable()
        {
            CLevelManager.OnWin -= Wictory;
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
            _levelText.text = $"LEVEL {CLevelManager.LEVEL}";
        

        }
        #region Listenner
        public void OpenSettings()
        {
            VKLayerController.Instance.ShowLayer("UIPause");
            VKAudioController.Instance.PlaySound("Button");
        }

        #endregion
        #region method

        private void Wictory()
        {
            Clear();
            var uiWin = (UIWin.UIWin)VKLayerController.Instance.ShowLayer("UIWin");
            uiWin.Construct();
        }

        private void GameLose()
        {
            VKAudioController.Instance.PlaySound($"Game_over_{Random.Range(1, 3)}");
            VKLayerController.Instance.ShowLayer("UIFail");
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
