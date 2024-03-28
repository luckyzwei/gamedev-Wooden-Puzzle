using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VKSdkDemo.UIDemo
{
    public class HItem : MonoBehaviour
    {
        [SerializeField] private Text nameTxt;      
        [SerializeField] private Text Text1;
        [SerializeField] private Text Text2;
        public void InitData(SkillObject skilObject, int Index)
        {
            nameTxt.text = skilObject.skillType.ToString();
            Text1.text = skilObject.damage.ToString();
            Text2.text = skilObject.min.ToString();
           
        }
    }
}

