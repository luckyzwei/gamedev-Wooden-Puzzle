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
        [Inject] private GameManager _gameManager;
        [SerializeField] private List<LevelButton> _levelButtons;
        [SerializeField] private GameObject _startMenu;
        [SerializeField] private GameObject _levelsMenu;
        [SerializeField] private GameObject _shopMenu;
        [SerializeField] private GameObject _coinsMenu;
        private void Start()
        {
            for (var i = 0; i < 45; i++) //TODO _dataMono.Data.LevelsCompleted
            {
                if(_levelButtons[i] == null) return;
                int levelIndex = i + 1;
                _levelButtons[i].Assign(levelIndex);
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
            _gameManager.ConstructLevel();
            _gameManager.Status = GameState.PrepareGame;
            Close();
        }
        public void OpenSettings()
        {
            _vkAudioController.PlaySound("Button");
            _vkLayerController.ShowLayer("UISetting");
        }

        public void OpenLevels(bool isLevels)
        {
            _startMenu.SetActive(!isLevels);
            _levelsMenu.SetActive(isLevels);
        }
        
        public void OpenShop(bool isLevels)
        {
            _startMenu.SetActive(!isLevels);
            _shopMenu.SetActive(isLevels);
        }
        
        public void OpenCoinsBuy(bool isLevels)
        {
            _shopMenu.SetActive(!isLevels);
            _coinsMenu.SetActive(isLevels);
        }
    }
}
