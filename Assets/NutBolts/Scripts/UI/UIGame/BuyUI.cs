using System;
using DG.Tweening;
using NutBolts.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NutBolts.Scripts.UI.UIGame
{
    public class BuyUI : MonoBehaviour
    {
        [Inject] private DataMono _dataMono;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private CanvasGroup _textWarning;
        [SerializeField] private Image _iconImage;
        private CBoosterUI _boosterUI;
        private Sequence _warningSequence;
        public void Open(CBoosterUI boosterUI)
        {
            _boosterUI = boosterUI;
            _description.text = _boosterUI.Description;
            gameObject.SetActive(true);
            _buyButton.onClick.AddListener(Buy);
            _closeButton.onClick.AddListener(Close);
            _iconImage.sprite = _boosterUI.IconSprite;
        }

        private void Close()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.RemoveListener(Close);
            _buyButton.onClick.RemoveListener(Buy);
        }

        private void Buy()
        {
            if (_dataMono.Data.Coins >= _boosterUI.Price)
            {
                _dataMono.Data.Coins -= _boosterUI.Price;
                _boosterUI.AddBuster();
            }
            else
            {
                _warningSequence = DOTween.Sequence();
                _warningSequence.Append(_textWarning.DOFade(1, 0.3f));
                _warningSequence.AppendInterval(2f);
                _warningSequence.Append(_textWarning.DOFade(0, 2f));
            }
        }
        
    }
}