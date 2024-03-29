using NutBolts.Scripts.Data;
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
        [FormerlySerializedAs("videoObj")] [SerializeField] private GameObject _forVideo;
        [FormerlySerializedAs("normalObj")] [SerializeField] private GameObject _normal;
        [FormerlySerializedAs("boosterType")] [SerializeField] private AbilityType _type;
        private AbilityObj _booster;
        public void Construct()
        {
            _booster = _dataMono.GetAbilityObj(_type);
            ResetBooster();
        }
        public void UseAbility()
        {
            _vkAudioController.PlaySound("Button");
            _booster ??= _dataMono.GetAbilityObj(_type);
            if (_booster.count == 0)
            {
                Video();
            }
            else
            {
                SimpleUse();
            }
        }
        private void Video()
        {
            OnComplete();
        }
        private void SimpleUse()
        {
            _dataMono.SubAbility(_type);
            OnComplete();
            ResetBooster();       
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
            GameManager.instance.ResetAllBusters();
        }
        private void OnTips()
        {
            GameManager.instance.PlayTip();
        }
        private void OnTool()
        {
            GameManager.instance.OnTool();
        }
        private void OnPrevious()
        {
            GameManager.instance.OnMove();
        }

        private void ResetBooster()
        {
            if (_booster.count == 0)
            {
                _forVideo.SetActive(true);
                _normal.SetActive(false);
            }
            else
            {
                _forVideo.SetActive(false);
                _normal.SetActive(true);
                _normal.transform.GetChild(0).GetComponent<Text>().text = _booster.count.ToString();
            }
        }
    }
}
