using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System;
using System.Collections;

namespace VKSdk.UI
{
    public class VKWebGLKeyboardController : MonoBehaviour
    {
#if UNITY_WEBGL
    //[DllImport("__Internal")]
    //private static extern void FKeyboardCheckPlatform();

    //[DllImport("__Internal")]
    //private static extern void FOpenInputKeyboard(string str);
    //[DllImport("__Internal")]
    //private static extern void FCloseInputKeyboard();

    ////Just adds these functions references to avoid stripping
    //[DllImport("__Internal")]
    //private static extern void FFixInputOnBlur();
    //[DllImport("__Internal")]
    //private static extern void FFixInputUpdate();

    public InputField inputFieldCurrent;
    public bool webGLMobile;

        #region Sinleton
    private static VKWebGLKeyboardController instance;

    public static VKWebGLKeyboardController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<VKWebGLKeyboardController>();
            }
            return instance;
        }
    }

    public void Start()
    {
#if UNITY_EDITOR
        OnTargetPlatform("true|iOS|Safari");
#else
        StartCoroutine(WaitToCheckPlatform());
#endif
    }
        #endregion

        #region Method
    IEnumerator WaitToCheckPlatform()
    {
        yield return new WaitForEndOfFrame();
        Application.ExternalCall("KeyboardCheckPlatform");
    }

    public void OnInputFieldSelect(InputField inputField)
    {
        if(webGLMobile)
        {
            this.inputFieldCurrent = inputField;
            try
            {
                Application.ExternalCall("OpenInputKeyboard", inputField.text);
                UnityEngine.WebGLInput.captureAllKeyboardInput = false;
            }
            catch (Exception error) { }
        }
    }

    public void OnInputFieldUnSelect(InputField inputField)
    {
        if (webGLMobile)
        {
            if (inputFieldCurrent != null && inputFieldCurrent.Equals(inputField))
            {
                this.inputFieldCurrent = null;
                try
                {
                    Application.ExternalCall("CloseInputKeyboard");
                    UnityEngine.WebGLInput.captureAllKeyboardInput = true;
                }
                catch { }
            }
        }
    }
        #endregion

        #region Callback
    public void OnTargetPlatform(string data)
    {
        string[] datas = data.Split('|');
        if(datas != null && datas.Length == 3)
        {
            webGLMobile = datas[0].Equals("true");
            // GameConfig.Instance.webglPlatform = datas[1];
            // GameConfig.Instance.webglBrowser = datas[2];

            // // add to home screen
            // if (webGLMobile && SceneStart.Instance != null)
            // {
            //     if (datas[1].Equals("iOS") && !datas[2].Equals("Safari"))
            //     {
            //         return;
            //     }
            //     // SceneStart.Instance.ShowAddHomePopup(datas[2].Equals("Safari") ? 1 : 0);
            // }
        }
    }

    public void OnLoseFocus()
    {
        if (webGLMobile)
        {
            if(inputFieldCurrent != null)
            {
                inputFieldCurrent.DeactivateInputField();
                inputFieldCurrent = null;
            }
            UnityEngine.WebGLInput.captureAllKeyboardInput = true;
        }
    }

    public void OnReceiveInputChange(string value)
    {
        if (webGLMobile && inputFieldCurrent != null)
        {
            inputFieldCurrent.text = value;
            StartCoroutine(WaitToMoveInputFieldEnd());
        }
    }

    IEnumerator WaitToMoveInputFieldEnd()
    {
        yield return new WaitForEndOfFrame();
        if (inputFieldCurrent != null)
        {
            inputFieldCurrent.MoveTextEnd(false);
        }
    }
        #endregion
#endif
    }
}