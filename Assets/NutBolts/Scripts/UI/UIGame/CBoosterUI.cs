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
        public BoosterType boosterType;
        private BoosterObj boosterObj;
        public void Initialized()
        {
            boosterObj = UserData.Instance.GetBoosterObj(boosterType);
            RefreshBooster();
        }
        public void OnCick()
        {
            VKAudioController.Instance.PlaySound("Button");
            if (boosterObj == null)
            {
                boosterObj = UserData.Instance.GetBoosterObj(boosterType);
            }
            if (boosterObj.number == 0)
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
            UserData.Instance.SubBooster(boosterType);
            OnComplete();
            RefreshBooster();       
        }
        private void OnComplete()
        {
            switch (boosterObj.boosterType)
            {
                case BoosterType.CReset: OnReset(); break;
                case BoosterType.CTip: OnTips(); break;
                case BoosterType.CTool: OnTool(); break;
                case BoosterType.CPrevious: OnPrevious(); break;
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
            if (boosterObj.number == 0)
            {

                videoObj.SetActive(true);
                normalObj.SetActive(false);
            }
            else
            {
                videoObj.SetActive(false);
                normalObj.SetActive(true);
                normalObj.transform.GetChild(0).GetComponent<Text>().text = boosterObj.number.ToString();
            }
        }
    }
}
