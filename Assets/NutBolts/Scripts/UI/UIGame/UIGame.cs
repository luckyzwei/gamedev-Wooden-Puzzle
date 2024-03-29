using TMPro;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIGame
{
    public class UIGame : VKLayer
    {
        public VKCountDownLite vkCountDown;
        public CBoosterUI[] cBoosters;
        public TextMeshProUGUI levelText;
        private void OnEnable()
        {
            CLevelManager.OnWin += WinGame;
       
        }
        private void OnDisable()
        {
            CLevelManager.OnWin -= WinGame;
       
        }
        public override void Close()
        {
            base.Close();
            Clear();
        }
        
        public void Init(int seconds=60)
        {
            foreach (var booster in cBoosters)
            {
                booster.Initialized();
            }
            vkCountDown.SetSeconds(seconds);
            vkCountDown.StartCountDown();
            vkCountDown.OnCountDownComplete = FailGame;
            levelText.text = $"LEVEL {CLevelManager.LEVEL}";
        

        }
        #region Listenner
        public void OnClickSetting()
        {

            VKLayerController.Instance.ShowLayer("UIPause");
            VKAudioController.Instance.PlaySound("Button");
        }

        #endregion
        #region method

        private void WinGame()
        {
            Clear();
            var uiWin = (UIWin.UIWin)VKLayerController.Instance.ShowLayer("UIWin");
            uiWin.Init();
        }

        private void FailGame()
        {
            VKAudioController.Instance.PlaySound($"Game_over_{Random.Range(1, 3)}");
            VKLayerController.Instance.ShowLayer("UIFail");
        }

        private void Clear()
        {      
            vkCountDown.OnCountDownComplete = null;
            vkCountDown.StopCountDown();
       
        }
        public void AddTime(int addTime)
        {
        
            vkCountDown.SetSeconds( addTime);
            vkCountDown.OnCountDownComplete = FailGame;
            vkCountDown.StartCountDown();
        }
   
        #endregion

    }
}
