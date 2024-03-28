
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.Notify;
using VKSdk.UI;


namespace VKSdkDemo
{
    namespace UIDemo
    {
        public class LSignin : VKLayer
        {
            private LVertical lVertical;
            private LHorizontal lHorizontal;
            private LCountDown lCountDownTime;
            private LCountDownLite lCountDownLite;
            private LMainMenu lMainMenu;
            public void BtOpenPopupCallbackYes()
            {
                LPopup.OpenPopup("Demo", "This is demo. A popup which has only yes callback", (isOK) =>
                  {
                      VKNotifyController.Instance.AddNotify("Hello World, Here is yes callback!", VKNotifyController.TypeNotify.Normal);
                  },false);
            }
            public void BtOpenMultiplePopup()
            {
                VKLayerController.Instance.ShowLayer("LExample");
            }
            public void BtOpenPopupClickListener()
            {
                LPopup.OpenPopup("Demo", "This is demo. A popup which is yes/no and its callback.", "Yes", "No", (isOk) =>
                {
                    if (isOk)
                    {
                        // callback yes
                        VKNotifyController.Instance.AddNotify("Hello World, Here is yes callback!", VKNotifyController.TypeNotify.Normal);
                    }
                    else
                    {
                        //callback no
                        VKNotifyController.Instance.AddNotify("Hello World, Here is no callback!", VKNotifyController.TypeNotify.Normal);
                    }
                }, true);
            }
            public void BtOpenPopupTimeCountdown()
            {
                lCountDownTime = (LCountDown)VKLayerController.Instance.ShowLayer("LCountDown");
            }
            public void BtOpenPopupTimeCountdownLite()
            {
                lCountDownLite = (LCountDownLite)VKLayerController.Instance.ShowLayer("LCountDownLite");
            }
            public void BtOpenPopupLVertical()
            {
                lVertical = (LVertical)VKLayerController.Instance.ShowLayer("LVertical");
            }
            public void BtOpenPopupLHorizontal()
            {
                lHorizontal = (LHorizontal)VKLayerController.Instance.ShowLayer("LHorizontal");
            }
            public void BtShowMessage()
            {
                VKNotifyController.Instance.AddNotify("Hello World, Show message demo!", VKNotifyController.TypeNotify.Error);
            }
            public void BtOpenBannerSlide()
            {
                lMainMenu = (LMainMenu)VKLayerController.Instance.ShowLayer("LMainMenu");
            }
            public void BtOpenSupport()
            {
                VKLayerController.Instance.ShowLayer("LSupport");
            }
        }
    }
}
