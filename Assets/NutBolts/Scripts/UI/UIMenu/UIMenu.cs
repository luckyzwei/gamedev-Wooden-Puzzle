using System.Collections.Generic;
using NutBolts.Scripts.Data;
using UnityEngine;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIMenu
{
    public class UIMenu : VKLayer
    {
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

  
        public override void ShowLayer()
        {
            base.ShowLayer();
            if (DataMono.Instance.SettingData.isMusic)
            {
                VKAudioController.Instance.PlayMusic("game_music");
            }
        }
    
        public void OnClickPlay()
        {
            LoadLevel(DataMono.Instance.Data.Level);
        }
    
        private void LoadLevel(int levelIndex)
        {
            VKAudioController.Instance.PlaySound("Button");
            VKLayerController.Instance.ShowLoading();
            PlayerPrefs.SetInt("OpenLevel", levelIndex);
            CLevelManager.Instance.LoadLevel();
            CLevelManager.THIS.GameStatus = GameState.PrepareGame;
            Close();
        }
        public void OnClickSetting()
        {
            VKAudioController.Instance.PlaySound("Button");
            VKLayerController.Instance.ShowLayer("UISetting");
        }

    }
}
