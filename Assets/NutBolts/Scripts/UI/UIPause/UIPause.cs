using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

public class UIPause : VKLayer
{
    [SerializeField] Transform soundBtn;
    [SerializeField] Transform musicBtn;
    [SerializeField] Transform shakeBtn;
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
        Refresh();
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
        Refresh();
    }

    public override void StartLayer()
    {
        base.StartLayer();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick_Sound()
    {
        VKAudioController.Instance.PlaySound("Button");
        UserData.Instance.CSettingData.isSound = !UserData.Instance.CSettingData.isSound;
        Refresh();
        UserData.Instance.SaveLocalData();
    }
    public void OnClick_Music()
    {
        VKAudioController.Instance.PlaySound("Button");
        UserData.Instance.CSettingData.isMusic = !UserData.Instance.CSettingData.isMusic;
        Refresh();
        UserData.Instance.SaveLocalData();
        if (UserData.Instance.CSettingData.isMusic)
        {
            VKAudioController.Instance.PlayMusic("game_music");
        }
        else
        {
            VKAudioController.Instance.StopMusic(() => { });
        }
    }
    public void OnClick_Shake()
    {
        VKAudioController.Instance.PlaySound("Button");
        UserData.Instance.CSettingData.isShake = !UserData.Instance.CSettingData.isShake;
        Refresh();
        UserData.Instance.SaveLocalData();
    }
    public void OnClick_Close()
    {
        VKAudioController.Instance.PlaySound("Button");
        Close();
    }
    public void OnClickHome()
    {
        VKAudioController.Instance.PlaySound("Button");
        var uiGame = (UIGame)VKLayerController.Instance.GetLayer("UIGame");
        uiGame.Close();
        CLevelManager.Instance.Reset();
        VKLayerController.Instance.ShowLayer("UIMenu");
        Close();
    }
    private void Refresh()
    {
        soundBtn.GetChild(0).gameObject.SetActive(UserData.Instance.CSettingData.isSound);
        soundBtn.GetChild(1).gameObject.SetActive(!UserData.Instance.CSettingData.isSound);

        musicBtn.GetChild(0).gameObject.SetActive(UserData.Instance.CSettingData.isMusic);
        musicBtn.GetChild(1).gameObject.SetActive(!UserData.Instance.CSettingData.isMusic);

        shakeBtn.GetChild(0).gameObject.SetActive(UserData.Instance.CSettingData.isShake);
        shakeBtn.GetChild(1).gameObject.SetActive(!UserData.Instance.CSettingData.isShake);
    }
}
