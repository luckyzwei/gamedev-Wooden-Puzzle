using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

public class UIMenu : VKLayer
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void BeforeHideLayer()
    {
        base.BeforeHideLayer();
    }

    public override void Close()
    {
        base.Close();
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
        if (UserData.Instance.CSettingData.isMusic)
        {
            VKAudioController.Instance.PlayMusic("game_music");
        }
    }

    public override void StartLayer()
    {
        base.StartLayer();
    }
    public void OnClickPlay()
    {
        VKAudioController.Instance.PlaySound("Button");
        VKLayerController.Instance.ShowLoading();
        PlayerPrefs.SetInt("OpenLevel", UserData.Instance.CGameData.CurrentLevel);
        CLevelManager.Instance.LoadLevel();
        CLevelManager.THIS.GameStatus = GameState.PrepareGame;
        Close();
    }
    public void OnClickSetting()
    {
        VKAudioController.Instance.PlaySound("Button");
        VKLayerController.Instance.ShowLayer("UISetting");
    }

}
