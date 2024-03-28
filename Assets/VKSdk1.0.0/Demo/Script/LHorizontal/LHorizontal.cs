using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk.UI;

namespace VKSdkDemo.UIDemo
{
    public class LHorizontal : VKLayer
    {
        [SerializeField] private VKInfiniteScroll vkInfiniteScroll;
        
        private List<SkillObject> SkillObjData;
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
            vkInfiniteScroll.OnFill -= OnFillItem;
            vkInfiniteScroll.OnHeight -= OnWidthItem;
            vkInfiniteScroll.RecycleAll();
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
          
            SkillObjData = SkillData.Instance.GetSkills();
            vkInfiniteScroll.OnFill += OnFillItem;
            vkInfiniteScroll.OnWidth += OnWidthItem;
            Init();

        }

        public override void StartLayer()
        {
            base.StartLayer();
        }
        #region method
        void OnFillItem(int index, GameObject item)
        {

            item.GetComponent<HItem>().InitData(SkillObjData[index], index);
        }

        int OnWidthItem(int index)
        {
            return 300;
        }
        #endregion
        #region Method Listener
        public void OnClickClose()
        {
            Close();
        }
      
        public void Init()
        {
            vkInfiniteScroll.RecycleAll();
            vkInfiniteScroll.InitData(this.SkillObjData.Count);
        }
        #endregion
    }

}
