using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VKSdk.UI;

namespace VKSdkDemo.UIDemo
{
    public class LVertical : VKLayer
    {
        [SerializeField] private VKInfiniteScroll vkInfiniteScroll;
        [SerializeField] private VKButtonSlide vkTabSlide;
        private List<Armor> ArmorObjData;
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
            vkInfiniteScroll.OnHeight -= OnHeightItem;

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
            ArmorObjData = ArmorData.Instance.GetArmors();
            SkillObjData = SkillData.Instance.GetSkills();
            vkInfiniteScroll.OnFill += OnFillItem;
            vkInfiniteScroll.OnHeight += OnHeightItem;
            Init();
           
        }

        public override void StartLayer()
        {
            base.StartLayer();
        }
        #region method
        void OnFillItem(int index, GameObject item)
        {

            if (vkTabSlide.currentIndex == 0)
            {
                item.GetComponent<LItem>().InitDataSKill(SkillObjData[index],index);
            }
            else
            {
                item.GetComponent<LItem>().InitDataArmor(ArmorObjData[index],index);
            }
        }

        int OnHeightItem(int index)
        {
            return 120;
        }
        #endregion
        #region Method Listener
        public void OnClickClose()
        {
            Close();
        }
        public void OnClickTab()
        {
            int tab = vkTabSlide.currentIndex;
            vkTabSlide.Init(tab);
            vkInfiniteScroll.RecycleAll();
            if (tab==0)
            {
                vkInfiniteScroll.InitData(this.SkillObjData.Count);
            }
            else
            {
                vkInfiniteScroll.InitData(this.ArmorObjData.Count);
            }
        }
        public void Init()
        {          
            vkTabSlide.Init(0);
            vkInfiniteScroll.RecycleAll();
            vkInfiniteScroll.InitData(this.SkillObjData.Count);
        }
        #endregion
    }

}
