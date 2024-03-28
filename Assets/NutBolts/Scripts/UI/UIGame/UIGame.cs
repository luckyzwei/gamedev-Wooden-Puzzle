using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

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
    public override void BeforeHideLayer()
    {
        base.BeforeHideLayer();
    }

    public override void Close()
    {
        base.Close();
        Clear();
    }

    public override void DestroyLayer()
    {
        base.DestroyLayer();
    }

    public override void DisableLayer()
    {
        base.DisableLayer();
    }

    public override void EnableLayer()
    {
        base.EnableLayer();
    }

    public override void FirstLoadLayer()
    {
        base.FirstLoadLayer();
    }

    public override void HideLayer()
    {
        base.HideLayer();
    }

    public override void OnLayerCloseDone()
    {
        base.OnLayerCloseDone();
    }

    public override void OnLayerOpenDone()
    {
        base.OnLayerOpenDone();
    }

    public override void OnLayerOpenPopupDone()
    {
        base.OnLayerOpenPopupDone();
    }

    public override void OnLayerPopupCloseDone()
    {
        base.OnLayerPopupCloseDone();
    }

    public override void OnLayerReOpenDone()
    {
        base.OnLayerReOpenDone();
    }

    public override void OnLayerSlideHideDone()
    {
        base.OnLayerSlideHideDone();
    }

    public override void ReloadCanvasScale(float screenRatio, float screenScale)
    {
        base.ReloadCanvasScale(screenRatio, screenScale);
    }

    public override void ReloadLayer()
    {
        base.ReloadLayer();
    }

    public override void ShowLayer()
    {
        base.ShowLayer();
    }

    public override void StartLayer()
    {
        base.StartLayer();
    }

    public void Init(int seconds=60)
    {
        for(int i=0; i<cBoosters.Length; i++)
        {
            cBoosters[i].Initialized();
        }
        vkCountDown.SetSeconds(seconds);
        vkCountDown.StartCountDown();
        vkCountDown.OnCountDownComplete = FailGame;
        levelText.text = string.Format("LEVEL {0}", CLevelManager.LEVEL);
        

    }
    #region Listenner
    public void OnClickSetting()
    {

        VKLayerController.Instance.ShowLayer("UIPause");
        VKAudioController.Instance.PlaySound("Button");
    }

    #endregion
    #region method
    void WinGame()
    {
        Clear();
        var uiWin = (UIWin)VKLayerController.Instance.ShowLayer("UIWin");
        uiWin.Init();
    }
    void FailGame()
    {
        VKAudioController.Instance.PlaySound(string.Format("Game_over_{0}", Random.Range(1, 3)));
        VKLayerController.Instance.ShowLayer("UIFail");
    }
    public void Clear()
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
