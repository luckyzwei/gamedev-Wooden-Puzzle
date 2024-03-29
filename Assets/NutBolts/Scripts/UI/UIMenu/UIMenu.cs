using System.Collections.Generic;
using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIMenu
{
    public class UIMenu : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKLayerController _vkLayerController;
        [Inject] private DataMono _dataMono;
        [SerializeField] private List<LevelButton> _levelButtons;
        private void Start()
        {
            for (var i = 0; i < _levelButtons.Count; i++)
            {
                if(_levelButtons[i] == null) return;
                int levelIndex = i;
                _levelButtons[i].Assign(i+1);
                _levelButtons[i].Button.onClick.AddListener((() =>
                {
                    LoadLevel(levelIndex);
                }));
            }
        }

  
        public override void ActivateLayer()
        {
            base.ActivateLayer();
            if (_dataMono.SettingData.isMusic)
            {
                _vkAudioController.PlayMusic("game_music");
            }
        }
    
        public void Play()
        {
            LoadLevel(_dataMono.Data.Level);
        }
    
        private void LoadLevel(int levelIndex)
        {
            _vkAudioController.PlaySound("Button");
            _vkLayerController.ShowLoading();
            PlayerPrefs.SetInt("OpenLevel", levelIndex);
            GameManager.instance.ConstructLevel();
            GameManager.@this.Status = GameState.PrepareGame;
            Close();
        }
        public void OpenSettings()
        {
            _vkAudioController.PlaySound("Button");
            _vkLayerController.ShowLayer("UISetting");
        }

    }
}
