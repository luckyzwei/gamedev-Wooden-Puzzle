using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk.UI;

using VKSdk;
//using SFB;

namespace VKSdkDemo.UIDemo
{
    public class LMainMenu : VKLayer
    {
        #region Properties
        [Space(20)]
        [Header("===========================================================")]
        public VKBannerSlide bannerSlide;

        [Space(20)]
        [SerializeField] private GameObject goNewPrefab;
        [SerializeField] private GameObject goNewContent;

        [Space(20)]
        public List<GameObject> goBannerAlphas;
        #endregion

        #region Implement
        public override void StartLayer()
        {
            base.StartLayer();
            if (VKLayerController.Instance.screenRatio < 1.5f)
            {
                goBannerAlphas.ForEach(a => a.SetActive(true));
            }
            else
            {
                goBannerAlphas.ForEach(a => a.SetActive(false));
            }
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
            bannerSlide.StopAutoSlide();
        }
        #endregion

        #region Listener
        
        public void OnBannerClickListener()
        {
            bannerSlide.vkHandAction.gameObject.SetActive(bannerSlide.indexCurrent != 1);
        }
       
        #endregion

       

      

    }

}


