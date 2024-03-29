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
            DataMono.Instance.SettingData.isSound = !DataMono.Instance.SettingData.isSound;
            Refresh();
            DataMono.Instance.SaveAll();
        }
        public void OnClick_Music()
        {
            VKAudioController.Instance.PlaySound("Button");
            DataMono.Instance.SettingData.isMusic = !DataMono.Instance.SettingData.isMusic;
            Refresh();
            DataMono.Instance.SaveAll();
            if (DataMono.Instance.SettingData.isMusic)
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
            DataMono.Instance.SettingData.isShake = !DataMono.Instance.SettingData.isShake;
            Refresh();
            DataMono.Instance.SaveAll();
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
            soundBtn.GetChild(0).gameObject.SetActive(DataMono.Instance.SettingData.isSound);
            soundBtn.GetChild(1).gameObject.SetActive(!DataMono.Instance.SettingData.isSound);

            musicBtn.GetChild(0).gameObject.SetActive(DataMono.Instance.SettingData.isMusic);
            musicBtn.GetChild(1).gameObject.SetActive(!DataMono.Instance.SettingData.isMusic);

            shakeBtn.GetChild(0).gameObject.SetActive(DataMono.Instance.SettingData.isShake);
            shakeBtn.GetChild(1).gameObject.SetActive(!DataMono.Instance.SettingData.isShake);
        }
    }
}
