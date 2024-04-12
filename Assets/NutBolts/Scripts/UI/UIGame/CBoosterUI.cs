using NutBolts.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VKSdk;
using Zenject;

namespace NutBolts.Scripts.UI.UIGame
{
    public class CBoosterUI : MonoBehaviour
    {
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private DataMono _dataMono;
        [Inject] private GameManager _gameManager;
        [SerializeField] private TMP_Text _countText;
        [FormerlySerializedAs("boosterType")] [SerializeField] private AbilityType _type;
        private AbilityObj _booster;
        public void Construct()
        {
            _booster = _dataMono.GetAbilityObj(_type);
            _countText.text = _booster.count.ToString();
        }
        public void UseAbility()
        {
            _vkAudioController.PlaySound("Button");
            if (_booster.count != 0)
            {
                SimpleUse();
            }
           
        }
        private void SimpleUse()
        {
            _dataMono.SubAbility(_type);
            _countText.text = _booster.count.ToString();
            OnComplete();
              
        }
        private void OnComplete()
        {
            switch (_booster.Type)
            {
                case AbilityType.CReset: OnReset(); break;
                case AbilityType.CTip: OnTips(); break;
                case AbilityType.CTool: OnTool(); break;
                case AbilityType.CPrevious: OnPrevious(); break;
            }
        }
        private void OnReset()
        {
            _gameManager.ResetAllBusters();
        }
        private void OnTips()
        {
            _gameManager.PlayTip();
        }
        private void OnTool()
        {
            _gameManager.OnTool();
        }
        private void OnPrevious()
        {
            _gameManager.OnMove();
        }
    }
}
