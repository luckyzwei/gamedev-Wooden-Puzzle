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
            DataMono.Instance.CSettingData.isSound = !DataMono.Instance.CSettingData.isSound;
            Refresh();
            DataMono.Instance.SaveLocalData();
        }
        public void OnClick_Music()
        {
            VKAudioController.Instance.PlaySound("Button");
            DataMono.Instance.CSettingData.isMusic = !DataMono.Instance.CSettingData.isMusic;
            Refresh();
            DataMono.Instance.SaveLocalData();
            if (DataMono.Instance.CSettingData.isMusic)
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
            DataMono.Instance.CSettingData.isShake = !DataMono.Instance.CSettingData.isShake;
            Refresh();
            DataMono.Instance.SaveLocalData();
        }
        public void OnClick_Close()
        {
            VKAudioController.Instance.PlaySound("Button");
            VKAudioController.Instance.isMusicOn = DataMono.Instance.CSettingData.isMusic;
            VKAudioController.Instance.isSoundOn = DataMono.Instance.CSettingData.isSound;   
            Close();
        }
        private void Refresh()
        {
            soundBtn.GetChild(0).gameObject.SetActive(DataMono.Instance.CSettingData.isSound);
            soundBtn.GetChild(1).gameObject.SetActive(!DataMono.Instance.CSettingData.isSound);

            musicBtn.GetChild(0).gameObject.SetActive(DataMono.Instance.CSettingData.isMusic);
            musicBtn.GetChild(1).gameObject.SetActive(!DataMono.Instance.CSettingData.isMusic);

            shakeBtn.GetChild(0).gameObject.SetActive(DataMono.Instance.CSettingData.isShake);
            shakeBtn.GetChild(1).gameObject.SetActive(!DataMono.Instance.CSettingData.isShake);
        }
    }
}
