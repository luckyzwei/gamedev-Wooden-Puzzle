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
        [SerializeField] private int _price = 100;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private string _description;
        [SerializeField] private BuyUI _buyUI;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _useButton;
        [FormerlySerializedAs("boosterType")] [SerializeField] private AbilityType _type;
        private AbilityObj _booster;
        public int Price => _price;
        public string Description => _description;
        public Sprite IconSprite => _iconImage.sprite;
        public void Construct()
        {
            _booster = _dataMono.GetAbilityObj(_type);
            _countText.text = _booster.count.ToString();
            _useButton.onClick.AddListener(UseAbility);
            if (_booster.count <= 0)
            {
                _useButton.interactable = false;
            }
        }
        private void UseAbility()
        {
            _vkAudioController.PlaySound("Button");
            _dataMono.SubAbility(_type);
            _countText.text = _booster.count.ToString();
            _useButton.interactable = false;
            OnComplete();
           
        }
      
        private void OnComplete()
        {
            switch (_booster.Type)
            {
                case AbilityType.CTool: OnTool(); break;
                case AbilityType.CPrevious: OnPrevious(); break;
            }
        }

        public void Buy()
        {
            _buyUI.Open(this);
        }

        public void AddBuster()
        {
            _dataMono.Data.AddAbility(_booster);
            _booster = _dataMono.GetAbilityObj(_type);
            _countText.text = _booster.count.ToString();
        }

        public void AddBusterBack()
        {
            AddBuster();
            _useButton.interactable = true;
        }
        private void OnTool()
        {
            _gameManager.OnTool();
        }
        private void OnPrevious()
        {
            if (!_gameManager.OnMove())
            {
                AddBusterBack();
            }
        }
    }
}
