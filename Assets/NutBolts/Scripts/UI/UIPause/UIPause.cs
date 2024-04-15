using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIPause
{
    public class UIPause : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKLayerController _vkLayerController;
        [Inject] private DataMono _dataMono;
        [Inject] private GameManager _gameManager;
        
        public void ClosePause()
        {
            _vkAudioController.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)_vkLayerController.GetLayer("UIGame");
            uiGame.Countdown.IsPause = false;
            Close();
        }
        public void ReturnHome()
        {
            _vkAudioController.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)_vkLayerController.GetLayer("UIGame");
            uiGame.Close();
            _gameManager.Reset();
            _vkLayerController.ShowLayer("UIMenu");
            Close();
        }
        
        public void Retry()
        {
            _vkAudioController.PlaySound("Button");
            _gameManager.OnReplayLevel();
            Close();
        }
   
    }
}
