using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

public class UIFail : VKLayer
{
    // Start is called before the first frame update
   
    #region OnClick Listener
    public void OnClick_Home()
    {
        VKAudioController.Instance.PlaySound("Button");
        var uiGame = (UIGame)VKLayerController.Instance.GetLayer("UIGame");
        uiGame.Close();
        CLevelManager.Instance.Reset();
        VKLayerController.Instance.ShowLayer("UIMenu");
        Close();
    }
    public void OnClick_AddTime()
    {
        VKAudioController.Instance.PlaySound("Button");
        var uiGame = (UIGame)VKLayerController.Instance.GetLayer("UIGame");
        uiGame.AddTime(60);
        Close();
      
    }
    public void OnClick_Retry()
    {
        VKAudioController.Instance.PlaySound("Button");
        CLevelManager.Instance.OnRetry();
        Close();
    }
    #endregion
}
