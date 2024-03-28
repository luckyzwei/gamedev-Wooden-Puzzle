using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.UI;
namespace VKSdkDemo

{
    namespace UIDemo
    {
        public class LPopup : VKLayer
        {
            #region Properties
            [Space(20)]
            [SerializeField] private Text txtInfo;
            [SerializeField] private Text txtTitle;
            [SerializeField] private Text txtBtOk;
            [SerializeField] private Text txtBtCancel;

            [SerializeField] private Button btOk;
            [SerializeField] private Button btActionCancel;
            [SerializeField] private Button btCancel;

            [SerializeField] private GameObject gButtonGroup;
            [SerializeField] private Image imgIcon;
            [SerializeField] private Sprite[] sprIcons;

            public string content;

            private System.Action<bool> ResultAction;
            #endregion

            #region Implement
            public override void StartLayer()
            {
                base.StartLayer();
                
            }

            public override void ShowLayer()
            {
                base.ShowLayer();
            }

            public override void ReloadLayer()
            {
                base.ReloadLayer();
            }

            public override void EnableLayer()
            {
                base.EnableLayer();
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
            public void ShowPopup(string title = "Notification", string strInfo = "", string strBtOK = "", string strBtClose = "", System.Action<bool> action = null, bool isClose = false)
            {
                this.ResultAction = action;
                this.content = strInfo;

                if (imgIcon != null)
                {
                    SetIcon(title);
                }

                txtInfo.text = strInfo;
                txtTitle.text = title;

                if (!string.IsNullOrEmpty(strBtOK) || !string.IsNullOrEmpty(strBtClose))
                {
                    gButtonGroup.SetActive(true);

                    if (!string.IsNullOrEmpty(strBtOK))
                    {
                        btOk.gameObject.SetActive(true);
                        txtBtOk.text = strBtOK;
                    }

                    if (!string.IsNullOrEmpty(strBtClose))
                    {
                        btActionCancel.gameObject.SetActive(true);
                        txtBtCancel.text = strBtClose;
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

            private void SetIcon(string title)
            {
                if (string.IsNullOrEmpty(title))
                {
                    imgIcon.sprite = sprIcons[0];
                    return;
                }

                if (title.Equals("Notify"))
                    imgIcon.sprite = sprIcons[1];
                else if (title.Equals("Error"))
                    imgIcon.sprite = sprIcons[1];
                else
                    imgIcon.sprite = sprIcons[0];
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
                ((LPopup)VKLayerController.Instance.ShowLayer("LPopup")).ShowPopup(title: title, strInfo: content, strBtOK: "Đồng ý", action: action, isClose: isClose);
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


}
