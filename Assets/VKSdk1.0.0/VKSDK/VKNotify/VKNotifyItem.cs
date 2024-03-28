using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VKSdk.Notify
{
    public class VKNotifyItem : MonoBehaviour
    {
        public Text txtNoti;
        public Image imgBackground;
        public CanvasGroup group;

        public Color[] cBackgrounds;
        public Color[] cTexts;

        public float timeDelay;

        public bool isActive;
        private List<NotifyItemData> contents;
        private NotifyItemData currentItem;
        private Shadow shadowText;

        public void Init()
        {
            contents = new List<NotifyItemData>();
            isActive = false;
            shadowText = txtNoti.GetComponent<Shadow>();
        }

        public void Show(NotifyItemData item)
        {
            if (gameObject.activeSelf)
            {
                if (!currentItem.content.Equals(item.content))
                    contents.Add(item);
            }
            else
            {
                contents.Add(item);
                gameObject.SetActive(true);
                isActive = true;

                StartCoroutine(Move());
            }
        }

        IEnumerator Move()
        {
            while (true)
            {
                currentItem = contents[0];
                contents.RemoveAt(0);

                @group.alpha = 0;
                transform.localPosition = new Vector3(0, -100, 0);

                shadowText.enabled = currentItem.type != VKNotifyController.TypeNotify.Normal;
                txtNoti.text = currentItem.content;
                txtNoti.color = cTexts[(int)currentItem.type];
                imgBackground.color = cBackgrounds[(int)currentItem.type];

                LeanTween.value(this.gameObject, 0, 1, 0.2f).setOnUpdate(delegate (float f)
                {
                    @group.alpha = f;
                });
                yield return new WaitForSeconds(timeDelay + (currentItem.content.Length / 15));

                if (contents.Count <= 0)
                {
                    isActive = false;
                    LeanTween.moveLocalY(this.gameObject, 150, 0.5f).setEase(LeanTweenType.easeInOutBack);
                    yield return new WaitForSeconds(0.6f);
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}