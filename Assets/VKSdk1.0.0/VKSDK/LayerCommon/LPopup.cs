using System;
using UnityEngine;
using UnityEngine.UI;
using VKSdk.UI;
using VKSdk;

namespace VKSdkDemo
{
    public class LPopup : VKLayer
    {
        #region Properties
        [Space(20)]
        public Text txtInfo;
        public Text txtTitle;
        public Text txtBtOk;
        public Text txtBtCancel;

        public Button btOk;
        public Button btActionCancel;
        public Button btCancel;

        public GameObject gButtonGroup;

        public string content;

        private Action<bool> ResultAction;
        #endregion

        #region Implement
        public override void StartLayer()
        {
            base.StartLayer();
        }

        public override void ActivateLayer()
        {
            base.ActivateLayer();
        }

        public override void ReloadLayer()
        {
            base.ReloadLayer();
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void DisableLayer()
        {
            base.DisableLayer();
        }

        public override void HideLayer()
        {
            base.HideLayer();

            gButtonGroup.SetActive(false);

            btOk.gameObject.SetActive(false);
            btActionCancel.gameObject.SetActive(false);
            btCancel.gameObject.SetActive(false);
        }
        #endregion

        #region Listener
        public void BtOkClickListener()
        {
            Close();
            if (ResultAction != null)
                ResultAction.Invoke(true);


        }

        public void BtCancelClickListener()
        {
            Close();
            if (!btActionCancel.gameObject.activeSelf && ResultAction != null)
                ResultAction.Invoke(false);


        }

        public void BtActionCancelClickListener()
        {
            Close();
            if (ResultAction != null)
                ResultAction.Invoke(false);


        }

        #endregion

        #region Method
        public void ShowPopup(string title = "NOTIFICATION", string strInfo = "", string strBtOK = "", string strBtClose = "", Action<bool> action = null, bool isClose = false)
        {
            ResultAction = action;
            content = strInfo;

            txtInfo.text = strInfo;
            txtTitle.text = title;
            // txtInfo.text = strInfo;
            // txtTitle.text = title;

            if (!string.IsNullOrEmpty(strBtOK) || !string.IsNullOrEmpty(strBtClose))
            {
                gButtonGroup.SetActive(true);

                if (!string.IsNullOrEmpty(strBtOK))
                {
                    btOk.gameObject.SetActive(true);
                    // txtBtOk.text = strBtOK;
                    txtBtOk.text = strBtOK;
                }

                if (!string.IsNullOrEmpty(strBtClose))
                {
                    btActionCancel.gameObject.SetActive(true);
                    txtBtCancel.text = strBtClose;
                    // txtBtCancel.text = strBtClose;
                }
                // txtInfo.transform.localPosition = new Vector3(0, 15f, 0);
            }
            else
            {
                gButtonGroup.SetActive(false);
                // txtInfo.transform.localPosition = new Vector3(0, -10f, 0);
            }

            btCancel.gameObject.SetActive(isClose);
        }
        #endregion

        #region Open Popup
        public static void OpenPopup(string title, string content, bool isClose = true)
        {
            if (VKLayerController.Instance == null) return;

            LPopup lPopup = VKLayerController.Instance.GetLayer<LPopup>();
            if (lPopup != null && lPopup.content.Equals(content))
                return;
            ((LPopup)VKLayerController.Instance.ShowLayer("LPopup")).ShowPopup(title: title, strInfo: content, isClose: isClose);
        }

        public static void OpenPopup(string title, string content, Action<bool> action, bool isClose)
        {
            if (VKLayerController.Instance == null) return;

            LPopup lPopup = VKLayerController.Instance.GetLayer<LPopup>();
            if (lPopup != null && lPopup.content.Equals(content))
                return;
            ((LPopup)VKLayerController.Instance.ShowLayer("LPopup")).ShowPopup(title: title, strInfo: content, strBtOK: "OK", action: action, isClose: isClose);
        }

        public static void OpenPopup(string title, string content, string strBtOk, string strBtCancel, Action<bool> action, bool isClose)
        {
            if (VKLayerController.Instance == null) return;

            LPopup lPopup = VKLayerController.Instance.GetLayer<LPopup>();
            if (lPopup != null && lPopup.content.Equals(content))
                return;

            ((LPopup)VKLayerController.Instance.ShowLayer("LPopup")).ShowPopup(title: title, strInfo: content, strBtOK: strBtOk, strBtClose: strBtCancel, action: action, isClose: isClose);
        }
        #endregion
    }
}