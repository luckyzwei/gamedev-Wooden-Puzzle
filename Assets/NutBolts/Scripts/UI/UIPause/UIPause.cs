using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIPause
{
    public class UIPause : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKLayerController _vkLayerController;
        [Inject] private DataMono _dataMono;
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
            _vkAudioController.PlaySound("Button");
            _dataMono.SettingData.isSound = !_dataMono.SettingData.isSound;
            Reset();
            _dataMono.SaveAll();
        }
        public void MusicChange()
        {
            _vkAudioController.PlaySound("Button");
            _dataMono.SettingData.isMusic = !_dataMono.SettingData.isMusic;
            Reset();
            _dataMono.SaveAll();
            if (_dataMono.SettingData.isMusic)
            {
                _vkAudioController.PlayMusic("game_music");
            }
            else
            {
                _vkAudioController.StopMusic(() => { });
            }
        }
        public void ShakeChange()
        {
            _vkAudioController.PlaySound("Button");
            _dataMono.SettingData.isShake = !_dataMono.SettingData.isShake;
            Reset();
            _dataMono.SaveAll();
        }
        public void ClosePause()
        {
            _vkAudioController.PlaySound("Button");
            Close();
        }
        public void ReturnHome()
        {
            _vkAudioController.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)_vkLayerController.GetLayer("UIGame");
            uiGame.Close();
            GameManager.instance.Reset();
            _vkLayerController.ShowLayer("UIMenu");
            Close();
        }
        private void Reset()
        {
            _soundToggle.GetChild(0).gameObject.SetActive(_dataMono.SettingData.isSound);
            _soundToggle.GetChild(1).gameObject.SetActive(!_dataMono.SettingData.isSound);

            _musicToggle.GetChild(0).gameObject.SetActive(_dataMono.SettingData.isMusic);
            _musicToggle.GetChild(1).gameObject.SetActive(!_dataMono.SettingData.isMusic);

            _shakeToggle.GetChild(0).gameObject.SetActive(_dataMono.SettingData.isShake);
            _shakeToggle.GetChild(1).gameObject.SetActive(!_dataMono.SettingData.isShake);
        }
    }
}
