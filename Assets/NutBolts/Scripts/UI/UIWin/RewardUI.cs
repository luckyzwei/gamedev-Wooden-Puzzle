using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace NutBolts.Scripts.UI.UIWin
{
    public class RewardUI : MonoBehaviour
    {
        public Image imgIcon;
        public Text textNumber;
        public void InitData(RewardObj reward)
        {
            imgIcon.sprite = Resources.Load<Sprite>("Sprite/" + reward.rewardType.ToString());
            textNumber.text = reward.amount.ToString();
        }
    }
}
