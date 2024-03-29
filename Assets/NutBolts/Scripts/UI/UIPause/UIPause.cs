using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIPause
{
    public class UIPause : VKLayer
    {
        [SerializeField] private Transform _soundToggle;
        [SerializeField] private Transform _musicToggle;
        [SerializeField] private Transform _shakeToggle;

        public override void ActivateLayer()
        {
            base.ActivateLayer();
            Reset();
        }
    
        public void ChangeSound()
        {
            VKAudioController.Instance.PlaySound("Button");
            DataMono.Instance.SettingData.isSound = !DataMono.Instance.SettingData.isSound;
            Reset();
            DataMono.Instance.SaveAll();
        }
        public void MusicChange()
        {
            VKAudioController.Instance.PlaySound("Button");
            DataMono.Instance.SettingData.isMusic = !DataMono.Instance.SettingData.isMusic;
            Reset();
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
        public void ShakeChange()
        {
            VKAudioController.Instance.PlaySound("Button");
            DataMono.Instance.SettingData.isShake = !DataMono.Instance.SettingData.isShake;
            Reset();
            DataMono.Instance.SaveAll();
        }
        public void ClosePause()
        {
            VKAudioController.Instance.PlaySound("Button");
            Close();
        }
        public void ReturnHome()
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
            GameManager.instance.Reset();
            VKLayerController.Instance.ShowLayer("UIMenu");
            Close();
        }
        private void Reset()
        {
            _soundToggle.GetChild(0).gameObject.SetActive(DataMono.Instance.SettingData.isSound);
            _soundToggle.GetChild(1).gameObject.SetActive(!DataMono.Instance.SettingData.isSound);

            _musicToggle.GetChild(0).gameObject.SetActive(DataMono.Instance.SettingData.isMusic);
            _musicToggle.GetChild(1).gameObject.SetActive(!DataMono.Instance.SettingData.isMusic);

            _shakeToggle.GetChild(0).gameObject.SetActive(DataMono.Instance.SettingData.isShake);
            _shakeToggle.GetChild(1).gameObject.SetActive(!DataMono.Instance.SettingData.isShake);
        }
    }
}
