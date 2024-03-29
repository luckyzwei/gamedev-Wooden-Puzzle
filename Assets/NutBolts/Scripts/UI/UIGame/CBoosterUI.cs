using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;

namespace NutBolts.Scripts.UI.UIGame
{
    public class CBoosterUI : MonoBehaviour
    {
        public GameObject videoObj;
        public GameObject normalObj;
        public AbilityType boosterType;
        private AbilityObj boosterObj;
        public void Initialized()
        {
            boosterObj = DataMono.Instance.GetAbilityObj(boosterType);
            RefreshBooster();
        }
        public void OnCick()
        {
            VKAudioController.Instance.PlaySound("Button");
            if (boosterObj == null)
            {
                boosterObj = DataMono.Instance.GetAbilityObj(boosterType);
            }
            if (boosterObj.count == 0)
            {
                OnClickVideo();
            }
            else
            {
                OnClickNormal();
            }
        }
        private void OnClickVideo()
        {
            OnComplete();
        }
        private void OnClickNormal()
        {
            DataMono.Instance.SubAbility(boosterType);
            OnComplete();
            RefreshBooster();       
        }
        private void OnComplete()
        {
            switch (boosterObj.Type)
            {
                case AbilityType.CReset: OnReset(); break;
                case AbilityType.CTip: OnTips(); break;
                case AbilityType.CTool: OnTool(); break;
                case AbilityType.CPrevious: OnPrevious(); break;
            }
        }
  
        private void OnReset()
        {
            CLevelManager.Instance.OnBoosterReset();
        }
        private void OnTips()
        {
            CLevelManager.Instance.OnPlayTip();
        }
        private void OnTool()
        {
            CLevelManager.Instance.OnTool();
        }
        private void OnPrevious()
        {
            CLevelManager.Instance.OnPreviouseMove();
        }
        public void RefreshBooster()
        {
            if (boosterObj.count == 0)
            {

                videoObj.SetActive(true);
                normalObj.SetActive(false);
            }
            else
            {
                videoObj.SetActive(false);
                normalObj.SetActive(true);
                normalObj.transform.GetChild(0).GetComponent<Text>().text = boosterObj.count.ToString();
            }
        }
    }
}
