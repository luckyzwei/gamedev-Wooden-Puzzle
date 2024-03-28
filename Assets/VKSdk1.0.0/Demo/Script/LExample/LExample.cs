using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk.UI;

namespace VKSdkDemo.UIDemo
{
    public class LExample : VKLayer
    {      
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
        #region method listener
        public virtual void OnClickNext()
        {
            VKLayerController.Instance.ShowLayer("LExample1");
        }
        public void OnClickClose()
        {
            Close();
        }
        #endregion
    }
}