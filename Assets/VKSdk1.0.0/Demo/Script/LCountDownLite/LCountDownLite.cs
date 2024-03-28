using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VKSdk.UI;
namespace VKSdkDemo.UIDemo
{
    public class LCountDownLite : VKLayer
    {
        [SerializeField] private VKCountDownLite vkCountDownLite;
        public void OnClickSelectType(Text textType)
        {         
            string vkcountDownStr = textType.text;
            switch (vkcountDownStr)
            {
                case "DAYS":
                    vkCountDownLite.typeCountDown = VKCountDownType.DAYS;
                    break;
                case "HOURS":
                    vkCountDownLite.typeCountDown = VKCountDownType.HOURS;
                    break;
                case "MINUTES":
                    vkCountDownLite.typeCountDown = VKCountDownType.MINUTES;
                    break;
                case "SECONDS":
                    vkCountDownLite.typeCountDown = VKCountDownType.SECONDS;
                    break;
            }
            vkCountDownLite.SetSeconds(100);
        }
        public void OnClickListenerCountNumber()
        {
            vkCountDownLite.SetSeconds(100);
            vkCountDownLite.StartCountDown();
        }
        public void OnClickStopCountDown()
        {
            vkCountDownLite.StopCountDown();
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

        public override void ShowLayer()
        {
            base.ShowLayer();
        }

        public override void StartLayer()
        {
            base.StartLayer();
        }
    }
}

