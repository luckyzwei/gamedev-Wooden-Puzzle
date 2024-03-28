using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;
namespace VKSdkDemo.UIDemo
{
    public class LCountDown : VKLayer
    {
        [SerializeField] private VKCountDown vkCountDown;
        [SerializeField] private GameObject vkCountDownDAY;
        [SerializeField] private GameObject vkCountDownHOUR;
        [SerializeField] private GameObject vkCountDownMIN;
        [SerializeField] private GameObject vkCountDownSECOND;
        public void OnClickSelectType(Text textType)
        {
            Clear();
            string vkcountDownStr = textType.text;          
            switch (vkcountDownStr)
            {
                case "DAYS": 
                    vkCountDownDAY.SetActive(true);
                    vkCountDown = vkCountDownDAY.GetComponent<VKCountDown>();
                    break;
                case "HOURS": 
                    vkCountDownHOUR.SetActive(true);
                    vkCountDown = vkCountDownHOUR.GetComponent<VKCountDown>();
                    break;
                case "MINUTES": 
                    vkCountDownMIN.SetActive(true);
                    vkCountDown = vkCountDownMIN.GetComponent<VKCountDown>();
                    break;
                case "SECONDS":
                    vkCountDownSECOND.SetActive(true);
                    vkCountDown = vkCountDownSECOND.GetComponent<VKCountDown>();
                    break;
            }
            vkCountDown.SetSeconds(99);
        }
        public void OnClickListenerCountNumber()
        {
          //  vkCountDown.SetSeconds(100);
            vkCountDown.StartCountDown();
        }
        public void OnClickStopCountDown()
        {
            vkCountDown.StopCountDown();
        }
        public override void ShowLayer()
        {
            base.ShowLayer();
            Clear();
           // vkCountDown.AddListener(OnStartGame, OnSpecial);
        }
        void OnStartGame()
        {

        }
        void OnSpecial()
        {

        }
        void Clear()
        {
            vkCountDownDAY.SetActive(false);
            vkCountDownHOUR.SetActive(false);
            vkCountDownMIN.SetActive(false);
            vkCountDownSECOND.SetActive(false);
        }
        public override void BeforeHideLayer()
        {
            base.BeforeHideLayer();
        }

        public override void Close()
        {
            base.Close();
        }

        public override void DestroyLayer()
        {
            base.DestroyLayer();
        }

        public override void DisableLayer()
        {
            base.DisableLayer();
        }

        public override void EnableLayer()
        {
            base.EnableLayer();
        }

        public override void FirstLoadLayer()
        {
            base.FirstLoadLayer();
        }

        public override void HideLayer()
        {
            base.HideLayer();
          //  vkCountDown.RemoveListener(OnStartGame, OnSpecial);
        }

        public override void OnLayerCloseDone()
        {
            base.OnLayerCloseDone();
        }

        public override void OnLayerOpenDone()
        {
            base.OnLayerOpenDone();
        }

        public override void OnLayerOpenPopupDone()
        {
            base.OnLayerOpenPopupDone();
        }

        public override void OnLayerPopupCloseDone()
        {
            base.OnLayerPopupCloseDone();
        }

        public override void OnLayerReOpenDone()
        {
            base.OnLayerReOpenDone();
        }

        public override void OnLayerSlideHideDone()
        {
            base.OnLayerSlideHideDone();
        }

        public override void ReloadCanvasScale(float screenRatio, float screenScale)
        {
            base.ReloadCanvasScale(screenRatio, screenScale);
        }

        public override void ReloadLayer()
        {
            base.ReloadLayer();
        }

       

        public override void StartLayer()
        {
            base.StartLayer();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

