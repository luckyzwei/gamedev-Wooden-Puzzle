using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIFail
{
    public class UIFail : VKLayer
    {
        public void OnClick_Home()
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGame)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
            CLevelManager.Instance.Reset();
            VKLayerController.Instance.ShowLayer("UIMenu");
            Close();
        }
        public void OnClick_AddTime()
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGame)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.AddTime(60);
            Close();
      
        }
        public void OnClick_Retry()
        {
            VKAudioController.Instance.PlaySound("Button");
            CLevelManager.Instance.OnRetry();
            Close();
        }
    }
}
