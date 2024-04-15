using NutBolts.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIWin
{
    public class UIWin : VKLayer
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private VKLayerController _vkLayerController;
        [Inject] private DataMono _dataMono;
        [Inject] private GameManager _gameManager;
        [SerializeField] private TMP_Text _levelText;
        private LevelObject _levelObj;
        [SerializeField] private RewardUI _rewardUI;
    
        
        public void OpenNextLevel()
        {
            if (DataMono.LevelsTotal >= GameManager.level + 1)
            {
                _vkAudioController.PlaySound("Button");
                _gameManager.LoadNextLevel();
                Close();
            }
            else
            {
                HomeButton();
            }
            
        }
        public void HomeButton() 
        {
            _vkAudioController.PlaySound("Button");
            _gameManager.Reset();
            _vkLayerController.ShowLayer("UIMenu");
            Close();
        }
        public void Construct()
        {
            _levelObj = _gameManager.LevelObject;
        
            _levelText.text = string.Format("Level {0}", GameManager.level);
            _rewardUI.InitData(_levelObj.rewards[0]);
          
            for(int i=0; i<_levelObj.rewards.Count; i++)
            {
                AbilityObj b = _levelObj.rewards[i].ConvertRewardToBooster();
                if (b == null)
                {
                    _dataMono.Data.Coins += _levelObj.rewards[i].amount;
                }
                else
                {
                    _dataMono.AddAbility(b);
                }
            }
            _dataMono.SaveAll();
            _vkAudioController.PlaySound("Cheers");
        }
    }
}
