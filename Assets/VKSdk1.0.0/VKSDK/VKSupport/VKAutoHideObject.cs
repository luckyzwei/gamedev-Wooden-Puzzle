using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace VKSdk.Support
{
    public class VKAutoHideObject : MonoBehaviour
    {
        public TextMeshProUGUI txtTarget;
        public string strCompare;

        void OnEnable()
        {
            StartCoroutine(WaitToHide());
        }

        IEnumerator WaitToHide()
        {
            yield return new WaitForEndOfFrame();
            if(txtTarget.text.Equals(strCompare))
            {
                gameObject.SetActive(false);
            }
        }
    }
}