using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIPause
{
    public class UIPause : VKLayer
    {
        [SerializeField] Transform soundBtn;
        [SerializeField] Transform musicBtn;
        [SerializeField] Transform shakeBtn;

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
            var uiGame = (UIGame.UIGame)VKLayerController.Instance.GetLayer("UIGame");
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
}
