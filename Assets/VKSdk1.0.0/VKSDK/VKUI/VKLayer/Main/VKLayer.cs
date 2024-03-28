using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
using VKSdk;
using System;

namespace VKSdk.UI
{
    public class VKLayer : MonoBehaviour
    {
        public enum AnimKey
        {
            OpenPopup,
            Open,
            ReOpen,
            Close,
            ClosePopup,
            Hide
        };

        public enum Position
        {
            Bootom = 0,
            Middle,
            Top
        }

        public enum AnimType
        {
            None = 0,
            Slide,
            Popup,
            Normal,
        }

        [Space(10)]
        public VKDragLayerEvent dragMini;

        [Space(10)]
        public GameObject gContentAll;

        [Space(10)]
        public AnimType layerAnimType;

        [Space(10)]
        public bool allowDestroy;
        public bool isGameLayer;
        public bool lockCanvasScale;

        [Space(10)]
        public Position position = Position.Bootom;

        [Space(10)]
        public List<VKLayerChildOrder> childOrders;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Canvas canvas;
        [HideInInspector]
        public GraphicRaycaster graphicRaycaster;
        [HideInInspector]
        public int layerIndex;
        [HideInInspector]
        public string layerKey;
        [HideInInspector]
        public bool isLayerAnimOpenDone;
        [HideInInspector]
        public Action onActionClose;
      

        public void InitLayer(string layerKey, float screenRatio, float screenScale)
        {
            isLayerAnimOpenDone = false;

            this.layerKey = layerKey;
            canvas = GetComponent<Canvas>();
            anim = GetComponent<Animator>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();

            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
            if (!lockCanvasScale)
                canvasScaler.matchWidthOrHeight = screenScale;

#if UNITY_EDITOR
        if (canvas == null)
            VKDebug.LogError("Layer need a Canvas");
        if (graphicRaycaster == null)
            VKDebug.LogError("Layer need a GraphicRaycaster");
#endif

          
        }

        public void SetLayerIndex(int index)
        {
            layerIndex = index;
        }

        public void SetActionClose(Action onClose)
        {
            onActionClose = onClose;
        }

        public virtual void ReloadCanvasScale(float screenRatio, float screenScale)
        {
            if (!lockCanvasScale)
            {
                CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = screenScale;
            }          
        }

        /**
         * Khoi chay 1 lan khi layer duoc tao
         */
        public virtual void StartLayer()
        {
            if (layerAnimType == AnimType.None)
            {
                isLayerAnimOpenDone = true;
            }

           
            
        }

        /**
         * Khoi chay 1 lan tren layer dau tien duoc tao tren scene
         */
        public virtual void FirstLoadLayer()
        {
           
        }

        /**
         * Khoi chay khi layer duoc add vao list active
         */
        public virtual void ShowLayer()
        {
            
        }

        /**
         * Khoi chay khi layer la layer dau tien
         */
        public virtual void EnableLayer()
        {
            graphicRaycaster.enabled = true;

           
        }

        /**
         * Khoi chay 1 lan khi layer duoc goi lai
         */
        public virtual void ReloadLayer()
        {
           
        }

        public virtual void BeforeHideLayer()
        {
           
        }

        public virtual void DisableLayer()
        {
            if (position != Position.Middle)
                graphicRaycaster.enabled = false;

           
        }

        public virtual void HideLayer()
        {
            
        }

        public virtual void DestroyLayer()
        {
            
        }

        // func
        public void SetSortOrder(int order)
        {
            canvas.sortingOrder = order;

            if (childOrders != null)
                childOrders.ForEach(a => a.ResetOrder(canvas.sortingOrder));
        }

        public void ResetPosition()
        {
            if (gContentAll != null)
            {
                RectTransform rect = gContentAll.GetComponent<RectTransform>();
                if (layerAnimType == AnimType.Slide)
                {
                    rect.offsetMin = new Vector2(1334, 0);
                    rect.offsetMax = new Vector2(1334, 0);
                }
                else
                {
                    rect.localPosition = new Vector2(0, 0);
                    rect.localPosition = new Vector2(0, 0);
                }
            }

            
        }

        private void ResetAfterAnim()
        {
            if (gContentAll != null)
            {
                if (layerAnimType != AnimType.Slide)
                {
                    gContentAll.transform.localScale = Vector3.one;

                    RectTransform rect = gContentAll.GetComponent<RectTransform>();
                    rect.localPosition = new Vector2(0, 0);
                    rect.localPosition = new Vector2(0, 0);

                    CanvasGroup cvGroup = gContentAll.GetComponent<CanvasGroup>();
                    cvGroup.alpha = 1;
                }
                
            }

          
        }

        public void PlayAnimation(AnimKey key)
        {
            if((key == AnimKey.ReOpen || key == AnimKey.Hide) && (layerAnimType == AnimType.Popup))
            {
                isLayerAnimOpenDone = true;
                return;
            }

            if (anim != null)
            {
                isLayerAnimOpenDone = false;
                anim.enabled = true;

                graphicRaycaster.enabled = false;
                switch(layerAnimType)
                {
                    case AnimType.Popup:
                        anim.SetTrigger(key.ToString());
                        if(key == AnimKey.OpenPopup) StartCoroutine(DelayToResetAfterAnim());
                        break;
                    case AnimType.None:
                    case AnimType.Normal:
                        if(key == AnimKey.ReOpen || key == AnimKey.Hide)
                        {
                            StartCoroutine(DelaytoRunAnim(key));
                        }
                        else
                        {
                            anim.SetTrigger(key.ToString());
                        }
                        break;
                    case AnimType.Slide:
                        StartCoroutine(DelaytoRunAnim(key));
                        break;
                }
               
            }
            else
            {
                isLayerAnimOpenDone = true;
            }
        }

        IEnumerator DelaytoRunAnim(AnimKey key)
        {
            yield return new WaitForSeconds(0.2f);
            anim.SetTrigger(key.ToString());
        }

        IEnumerator DelayToResetAfterAnim()
        {
            yield return new WaitForSeconds(0.5f);

            if (gContentAll != null)
            {
                if (layerAnimType != AnimType.Slide)
                {
                    CanvasGroup cvGroup = gContentAll.GetComponent<CanvasGroup>();
                    if (cvGroup.alpha < 1)
                    {
                        gContentAll.transform.localScale = Vector3.one;

                        RectTransform r = gContentAll.GetComponent<RectTransform>();
                        r.localPosition = new Vector2(0, 0);
                        r.localPosition = new Vector2(0, 0);
                        cvGroup.alpha = 1;
                    }              
                }

            }
        }

        public virtual void Close()
        {          
            if (onActionClose != null)
            {
                onActionClose.Invoke();
            }

            graphicRaycaster.enabled = false;
            VKLayerController.Instance.HideLayer(this);
        }

        #region Anim Action Done
        public virtual void OnLayerOpenDone()
        {
            graphicRaycaster.enabled = true;
            isLayerAnimOpenDone = true;
        }

        public virtual void OnLayerCloseDone()
        {
            HideLayer();
            VKLayerController.Instance.CacheLayer(this);
            isLayerAnimOpenDone = false;       
        }

        public virtual void OnLayerOpenPopupDone()
        {
            graphicRaycaster.enabled = true;
            anim.enabled = false;
            ResetAfterAnim();
            isLayerAnimOpenDone = true;        
        }

        public virtual void OnLayerPopupCloseDone()
        {
            anim.enabled = false;
            HideLayer();
            VKLayerController.Instance.CacheLayer(this);
            isLayerAnimOpenDone = false;
        }

        // khong phai la hidelayer ma chi dich sang ben trai roi an di
        public virtual void OnLayerSlideHideDone()
        {
            anim.enabled = false;

            isLayerAnimOpenDone = false;
            if (!isGameLayer)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void OnLayerReOpenDone()
        {
            anim.enabled = false;

            graphicRaycaster.enabled = true;
            isLayerAnimOpenDone = true;
        }
#endregion
    }
}