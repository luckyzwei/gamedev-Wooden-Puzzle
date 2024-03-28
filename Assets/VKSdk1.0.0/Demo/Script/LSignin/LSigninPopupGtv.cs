
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VKSdk;
using VKSdk.Notify;
using VKSdk.UI;

namespace VKSdkDemo
{
    namespace UIDemo
    {        
        public class LSigninPopupGtv : VKLayer
        {

            #region Properties
          
            [Header("SIGNIN")]
            public InputField inpSigninUsername;
            public InputField inpSigninPassword;

            [SerializeField] private GameObject goSavePassTick;          
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
            }
            #endregion 
        }
    }
}
