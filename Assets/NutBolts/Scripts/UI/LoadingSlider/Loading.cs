using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NutBolts.Scripts.UI.LoadingSlider
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private void OnEnable()
        {
            _image.fillAmount = 0;
            _image.DOFillAmount(1, 0.5f);
        }
    }
}