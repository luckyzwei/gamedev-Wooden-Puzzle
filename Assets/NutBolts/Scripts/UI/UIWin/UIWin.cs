using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIWin
{
    public class UIWin : VKLayer
    {
        
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
            VKAudioController.Instance.PlaySound("Button");
            GameManager.instance.LoadNextLevel();
            Close();
        }
        public void HomeButton() //TODO fix
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGameMenu)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
            GameManager.instance.Reset();
            VKLayerController.Instance.ShowLayer("UIMenu");
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
                    DataMono.Instance.Data.Coins += _levelObj.rewards[i].amount;
                }
                else
                {
                    DataMono.Instance.AddAbility(b);
                }
            }
            DataMono.Instance.SaveAll();
            VKAudioController.Instance.PlaySound("Cheers");
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
