using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace NutBolts.Scripts.Data
{
    public class GameData
    {
        public int Coins { get; set; } = 200;
        public int Level { get; set; }
        public List<AbilityObj> Abilities {get;} = new() 
        {
            new() {count=100,Type=AbilityType.CReset},
            new() {count=100,Type=AbilityType.CTool},
            new() {count=100,Type=AbilityType.CTip},
            new() {count=100,Type=AbilityType.CPrevious}
        };

        public void UseAbility(AbilityType type)
        {
            foreach (var t in Abilities.Where(t => t.Type == type))
            {
                t.count--;
                if (t.count < 0) t.count = 0;
                break;
            }
        }
        public void AddAbility(AbilityObj booster)
        {
            foreach (var t in Abilities)
            {
                if (t.Type == booster.Type)
                {
                    t.count+=booster.count;               
                    break;
                }
            }
        }
    }
    
    public class SettingInfo {
        public bool isSound = true;
        public bool isMusic = true;
        public bool isShake = true;
    }
    
    [Serializable]
    public class AbilityObj
    {
        [FormerlySerializedAs("boosterType")] public AbilityType Type;
        [FormerlySerializedAs("number")] public int count;
    }
    
    
    public enum AbilityType
    {
        CReset=0,
        CTool=1,
        CPrevious=2,
        CTip=3,

    }
}