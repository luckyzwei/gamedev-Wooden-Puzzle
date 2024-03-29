using NutBolts.Scripts.Data;
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
        [SerializeField] private Text _levelText;
        private LevelObject _levelObj;
        [SerializeField] private VKInfiniteScroll _scroll;
    
        public override void Close()
        {
            base.Close();
            _scroll.OnFill -= AddReward;
            _scroll.OnWidth -= OnWidthChange;
        }
        public void OpenNextLevel()
        {
            _vkAudioController.PlaySound("Button");
            GameManager.instance.LoadNextLevel();
            Close();
        }
        public void HomeButton() //TODO fix
        {
            _vkAudioController.PlaySound("Button");
            //var uiGame = (UIGame.UIGameMenu)_vkLayerController.GetLayer("UIGame");
            //uiGame.Close();
            GameManager.instance.Reset();
            _vkLayerController.ShowLayer("UIMenu");
            Close();
        }
        public void Construct()
        {
            _levelObj = GameManager.instance.LevelObject;
        
            _levelText.text = string.Format("Level {0}", GameManager.level);
            _scroll.RecycleAll();
            _scroll.OnWidth += OnWidthChange;
            _scroll.OnFill += AddReward;
            _scroll.InitData(_levelObj.rewards.Count);
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

        private void AddReward(int index, GameObject go)
        {
            go.GetComponent<RewardUI>().InitData(_levelObj.rewards[index]);
        }

        private static int OnWidthChange(int index)
        {
            return 400;
        }
    }
}
