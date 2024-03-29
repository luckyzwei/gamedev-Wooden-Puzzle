using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UISetting
{
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
                VKAudioController.Instance.StopMusic(()=> { });
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
            VKAudioController.Instance.isMusicOn = DataMono.Instance.SettingData.isMusic;
            VKAudioController.Instance.isSoundOn = DataMono.Instance.SettingData.isSound;   
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
