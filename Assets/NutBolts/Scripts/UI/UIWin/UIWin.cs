using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;

namespace NutBolts.Scripts.UI.UIWin
{
    public class UIWin : VKLayer
    {
        public Text textTittle;
        private LevelObject _levelObj;
        public VKInfiniteScroll vkInfiniteScroll;
    
        public override void Close()
        {
            base.Close();
            vkInfiniteScroll.OnFill -= FillItem;
            vkInfiniteScroll.OnWidth -= OnWidth;
        }
        public void OnClickNextLevel()
        {
            VKAudioController.Instance.PlaySound("Button");
            CLevelManager.Instance.NextLevel();
            Close();
        }
        public void OnClickGoHome()
        {
            VKAudioController.Instance.PlaySound("Button");
            var uiGame = (UIGame.UIGame)VKLayerController.Instance.GetLayer("UIGame");
            uiGame.Close();
            CLevelManager.Instance.Reset();
            VKLayerController.Instance.ShowLayer("UIMenu");
            Close();
        }
        public void Init()
        {
            this._levelObj = CLevelManager.Instance.levelObject;
        
            textTittle.text = string.Format("Level {0}", CLevelManager.LEVEL);
            vkInfiniteScroll.RecycleAll();
            vkInfiniteScroll.OnWidth += OnWidth;
            vkInfiniteScroll.OnFill += FillItem;
            vkInfiniteScroll.InitData(_levelObj.rewards.Count);
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

        private void FillItem(int index, GameObject go)
        {
            go.GetComponent<RewardUI>().InitData(_levelObj.rewards[index]);
        }

        private int OnWidth(int index)
        {
            return 400;
        }
    }
}
