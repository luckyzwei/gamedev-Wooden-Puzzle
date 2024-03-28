using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

public class UISetting : VKLayer
{
    [SerializeField] Transform soundBtn;
    [SerializeField] Transform musicBtn;
    [SerializeField] Transform shakeBtn;
 
    public override void EnableLayer()
    {
        base.EnableLayer();
        Refresh();
    }
    
    public override void ShowLayer()
    {
        base.ShowLayer();
        Refresh();
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
            VKAudioController.Instance.StopMusic(()=> { });
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
        VKAudioController.Instance.isMusicOn = UserData.Instance.CSettingData.isMusic;
        VKAudioController.Instance.isSoundOn = UserData.Instance.CSettingData.isSound;   
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
