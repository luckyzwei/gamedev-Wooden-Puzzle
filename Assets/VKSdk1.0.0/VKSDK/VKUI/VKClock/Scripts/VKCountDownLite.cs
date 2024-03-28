using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VKSdk.UI
{
    public class VKCountDownLite : MonoBehaviour
    {
        public Text txtCountDown;

        public float countdown; // seconds
        public float countdownTest; // seconds

        public VKCountDownType typeCountDown;

        public Action OnCountDownComplete;
        public Action OnShowSpecial;
        public Action<int> OnChangeNumber;

        public Color colorNormal;
        public Color colorSpecial;

        public int timeShowSpecial;
        public bool showSecondText;
        public bool autoChangeType;

        [HideInInspector]
        public bool isCountDone;
        private bool isShowSpecial;

        private DateTime timePause;

        //public void OnDisable()
        //{
        //    StopCountDown();
        //}

        [ContextMenu("Test")]
        public void Test()
        {
            StartCountDown(countdownTest);
        }

        #region Unity Method
        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                timePause = DateTime.Now;
            }
            else if (!isCountDone)
            {
                var range = DateTime.Now - timePause;
                countdown -= (float)range.TotalSeconds;
                if (countdown < 0)
                {
                    countdown = 0;
                }
            }
        }
        #endregion

        public void StartCountDown(double seconds)
        {
            StartCountDown((float)seconds);
        }

        public void StartCountDown(float seconds)
        {
            countdown = seconds;
            StartCountDown();
        }

        public void StartCountDown()
        {
            if (countdown < 0)
            {
                countdown = 0;
            }

            if (isShowSpecial)
            {
                txtCountDown.color = colorNormal;
                isShowSpecial = false;
            }

            ShowTime();

            StopAllCoroutines();
            StartCoroutine(ChangeTime());
        }

        public void SetSeconds(float seconds)
        {
            StopAllCoroutines();
            isCountDone = true;

            if (isShowSpecial)
            {
                txtCountDown.color = colorNormal;
                isShowSpecial = false;
            }

            countdown = seconds;
            ShowTime();
        }

        public int GetSeconds()
        {
            return (int)countdown;
        }

        private void ShowTime()
        {
            TimeSpan t = TimeSpan.FromSeconds(countdown);

            string str = countdown.ToString("F0");
            if (countdown < 0)
            {
                str = "0";
            }

            VKCountDownType typeTemp = typeCountDown;

            if(autoChangeType)
            {
                if(t.TotalDays >= 1) typeTemp = VKCountDownType.DAYS;
                else if(t.TotalHours >= 1) typeTemp = VKCountDownType.HOURS;
                else if(t.TotalMinutes >= 1) typeTemp = VKCountDownType.MINUTES;
                else typeTemp = VKCountDownType.SECONDS;

                if(typeTemp < typeCountDown) typeTemp = typeCountDown;
            }

            if (typeTemp == VKCountDownType.HOURS)
            {
                str = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    (int)t.TotalHours,
                    t.Minutes,
                    t.Seconds);
            }
            else if (typeTemp == VKCountDownType.MINUTES)
            {
                str = string.Format("{0:D2}:{1:D2}",
                    (int)t.TotalMinutes,
                    t.Seconds);
            }
            else if (typeTemp == VKCountDownType.DAYS)
            {
                str = string.Format("{0:D2} {4}, {1:D2}:{2:D2}:{3:D2}",
                    t.Days,
                    t.Hours,
                    t.Minutes,
                    t.Seconds,
                    (t.Days > 1 ? "DAYS" : "DAY").ToLower());
            }
            else
            {
                str = string.Format("{0:D2}",
                    (int)t.TotalSeconds);
            }

            if (timeShowSpecial > 0 && !isShowSpecial)
            {
                if (countdown <= timeShowSpecial)
                {
                    isShowSpecial = true;
                    txtCountDown.color = colorSpecial;

                    if (OnShowSpecial != null)
                    {
                        OnShowSpecial.Invoke();
                    }
                }
            }

            if (showSecondText)
            {
                str += "s";
            }

            txtCountDown.text = str;
        }

        public void ShowTextOnly(string str, bool isSpecial = false)
        {
            StopAllCoroutines();
            isCountDone = true;
            isShowSpecial = isSpecial;
            timeShowSpecial = 0;

            countdown = 0;
            txtCountDown.color = isShowSpecial ? colorSpecial : colorNormal;
            txtCountDown.text = str;
        }

        public void StopCountDown()
        {
            StopAllCoroutines();
            isCountDone = true;
        }

        IEnumerator ChangeTime()
        {
            isCountDone = false;
            while (true)
            {
                yield return new WaitForSeconds(1f);
                countdown -= 1f;

                if (countdown < 0)
                    countdown = 0;

                if (OnChangeNumber != null)
                {
                    OnChangeNumber.Invoke((int)countdown);
                }

                ShowTime();
                if (countdown <= 0)
                {
                    isCountDone = true;
                    if (OnCountDownComplete != null)
                    {
                        OnCountDownComplete.Invoke();
                    }
                    break;
                }
            }
        }
    }
}