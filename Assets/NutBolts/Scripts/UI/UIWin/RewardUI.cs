using NutBolts.Scripts.Data;
using TMPro;
using UnityEngine;

namespace NutBolts.Scripts.UI.UIWin
{
    public class RewardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public void InitData(RewardObj reward)
        {
            _text.text = reward.amount.ToString();
        }
    }
}
