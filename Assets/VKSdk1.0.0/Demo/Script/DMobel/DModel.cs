using System.Collections;
using System.Collections.Generic;
namespace VKSdkDemo.UIDemo
{
    public class DModel 
    {
        
    }
    [System.Serializable]
    public class SkillObject
    {
        public SkillType skillType;
        public int damage;
        public int min = 10;
    }
    [System.Serializable]
    public enum SkillType
    {
        Sword,
        Gun,
        ShotGun,
        Missile,
        Kick,
        Punch,
        Bomb,
        Archery
    }
    [System.Serializable]
    public class Armor
    {
        public string name;
        public int gold;
        public int level;
    }
  
}

