using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VKSdkDemo.UIDemo
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "GameData/Init SkillData")]
    public class SkillData : ScriptableObject
    {
        private static SkillData instance;
        public static SkillData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<SkillData>("SkillData");
                }
                return instance;
            }
        }
        [SerializeField] private List<SkillObject> SKILLS = new List<SkillObject>();
        public List<SkillObject> GetSkills()
        {
            return SKILLS;
        }

    }
}