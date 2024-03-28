using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VKSdkDemo.UIDemo
{
    public class LItem : MonoBehaviour
    {
        [SerializeField] private Text nameTxt;
        [SerializeField] private Image bgImg;
        [SerializeField] private Text Text1;
        [SerializeField] private Text Text2;
        [SerializeField] private Color32[] colors;
        public void InitDataSKill(SkillObject skilObject, int Index)
        {
            nameTxt.text = skilObject.skillType.ToString();
            Text1.text = skilObject.damage.ToString();
            Text2.text = skilObject.min.ToString();
            bgImg.color = colors[Index % 2];
        }
        public void InitDataArmor(Armor armor, int Index)
        {
            nameTxt.text = armor.name;
            Text1.text = armor.gold.ToString();
            Text2.text = armor.level.ToString();
            bgImg.color = colors[Index % 2];
        }
    }
}

