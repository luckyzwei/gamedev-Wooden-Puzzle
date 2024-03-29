using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIFail
{
    public class UILevelFailed : VKLayer
    {
        public void Home()
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
            CLevelManager.Instance.Reset();
            VKLayerController.Instance.ShowLayer("UIMenu");
            Close();
        }
        public void AddTime()
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.AddTime(60);
            Close();
      
        }
        public void Retry()
        {
            VKAudioController.Instance.PlaySound("Button");
            CLevelManager.Instance.OnRetry();
            Close();
        }
    }
}
