using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;

public class UIWin : VKLayer
{
    public Text textTittle;
    private LevelObject levelObj;
    public VKInfiniteScroll vkInfiniteScroll;
    public override void BeforeHideLayer()
    {
        base.BeforeHideLayer();
    }

    public override void Close()
    {
        base.Close();
        vkInfiniteScroll.OnFill -= FillItem;
        vkInfiniteScroll.OnWidth -= OnWidth;
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

    public void OnClickNextLevel()
    {
        VKAudioController.Instance.PlaySound("Button");
        CLevelManager.Instance.NextLevel();
        Close();
    }
    public void OnClickGoHome()
    {
        VKAudioController.Instance.PlaySound("Button");
        var uiGame = (UIGame)VKLayerController.Instance.GetLayer("UIGame");
        uiGame.Close();
        CLevelManager.Instance.Reset();
        VKLayerController.Instance.ShowLayer("UIMenu");
        Close();
    }
    public void Init()
    {
        this.levelObj = CLevelManager.Instance.levelObject;
        
        textTittle.text = string.Format("Level {0}", CLevelManager.LEVEL);
        vkInfiniteScroll.RecycleAll();
        vkInfiniteScroll.OnWidth += OnWidth;
        vkInfiniteScroll.OnFill += FillItem;
        vkInfiniteScroll.InitData(levelObj.rewards.Count);
        for(int i=0; i<levelObj.rewards.Count; i++)
        {
            BoosterObj b = levelObj.rewards[i].ConvertRewardToBooster();
            if (b == null)
            {
                UserData.Instance.CGameData.TotalCoin += levelObj.rewards[i].amount;
            }
            else
            {
                UserData.Instance.AddBooster(b);
            }
        }
        UserData.Instance.SaveLocalData();
        VKAudioController.Instance.PlaySound("Cheers");
    }

    private void FillItem(int Index, GameObject go)
    {
        go.GetComponent<RewardUI>().InitData(levelObj.rewards[Index]);
    }

    private int OnWidth(int index)
    {
        return 400;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
