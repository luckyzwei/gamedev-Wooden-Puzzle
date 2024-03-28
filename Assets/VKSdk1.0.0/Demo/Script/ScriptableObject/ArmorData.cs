using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VKSdkDemo.UIDemo
{
    [CreateAssetMenu(fileName = "ArmorData", menuName = "GameData/Init ArmorData")]
    public class ArmorData : ScriptableObject
    {
        private static ArmorData instance;
        public static ArmorData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<ArmorData>("ArmorData");
                }
                return instance;
            }
        }
        [SerializeField] private List<Armor> ARMORS = new List<Armor>();
        public List<Armor> GetArmors()
        {
            return ARMORS;
        }

    }
}