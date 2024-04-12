using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UISetting
{
    public class UISetting : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private DataMono _dataMono;
        [SerializeField] private Image _soundImage;
        [SerializeField] private Sprite _soundOn, _soundOff;
 
        public override void Activate()
        {
            base.Activate();
            _soundImage.sprite = _dataMono.SettingData.isSound ? _soundOn : _soundOff;
        }
    
        public override void ActivateLayer()
        {
            base.ActivateLayer();
            _soundImage.sprite = _dataMono.SettingData.isSound ? _soundOn : _soundOff;
        }
        
        public void SoundChange()
        {
            _vkAudioController.PlaySound("Button");
            _dataMono.SettingData.isSound = !_dataMono.SettingData.isSound;
            _dataMono.SettingData.isMusic = !_dataMono.SettingData.isMusic;
            _soundImage.sprite = _dataMono.SettingData.isSound ? _soundOn : _soundOff;
            _dataMono.SaveAll();
            if (_dataMono.SettingData.isMusic)
            {
                _vkAudioController.PlayMusic("game_music");
            }
            else
            {
                _vkAudioController.StopMusic(()=> { });
            }
        }
    
        public void ShakeChange()
        {
            _vkAudioController.PlaySound("Button");
            _dataMono.SettingData.isShake = !_dataMono.SettingData.isShake;
            _dataMono.SaveAll();
        }
        public void CloseSettings()
        {
            _vkAudioController.PlaySound("Button");
            _vkAudioController.isMusicOn = _dataMono.SettingData.isMusic;
            _vkAudioController.isSoundOn = _dataMono.SettingData.isSound;   
            Close();
        }
      
    }
}
