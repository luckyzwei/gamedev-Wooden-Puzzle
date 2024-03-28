using System.Collections;
using UnityEngine;
using VKSdk;

namespace VKSdk.UI
{
    public class VKLoading : MonoBehaviour
    {
        [SerializeField] GameObject objConnect;
        [SerializeField] float speedRotate;
        Coroutine action;
        public void ShowLoading(bool autoHide)
        {
            gameObject.SetActive(true);
            if (autoHide)
                StartCoroutine(WaitToHideLoading());
        }

        public void HideLoading()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        public static IEnumerator WaitToHideLoading()
        {
            yield return new WaitForSeconds(30f);
            VKLayerController.Instance.HideLoading();
        }
        IEnumerator RotateConnecting()

        {
            action = null;
            objConnect.transform.Rotate(0, 0, -speedRotate);
            yield return new WaitForSeconds(0.02f);
            action = StartCoroutine(RotateConnecting());
        }
        private void OnEnable()
        {
            action = StartCoroutine(RotateConnecting());
        }
        private void OnDisable()
        {
            if (action != null)
            {
                StopCoroutine(action);
                action = null;
            }
        }
    }
}