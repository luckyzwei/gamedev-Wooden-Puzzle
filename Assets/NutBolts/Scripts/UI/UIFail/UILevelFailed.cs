using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIFail
{
    public class UILevelFailed : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKLayerController _vkLayerController;
        public void Home()
        {
            _vkAudioController.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)_vkLayerController.GetLayer("UIGame");
            uiGame.Close();
            GameManager.instance.Reset();
            _vkLayerController.ShowLayer("UIMenu");
            Close();
        }
        public void AddTime()
        {
            _vkAudioController.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)_vkLayerController.GetLayer("UIGame");
            uiGame.AddTime(60);
            Close();
      
        }
        public void Retry()
        {
            _vkAudioController.PlaySound("Button");
            GameManager.instance.OnReplayLevel();
            Close();
        }
    }
}
