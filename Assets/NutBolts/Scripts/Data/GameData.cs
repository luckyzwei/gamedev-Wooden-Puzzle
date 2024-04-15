using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace NutBolts.Scripts.Data
{
    public class GameData
    {
        private int _coins;

        public int Coins
        {
            get => _coins;
            set
            {
                _coins = value;
                PlayerPrefs.SetInt("Coins", _coins);
            }
        }

        public int Level { get; set; }
        
        public int LevelsCompleted { get; private set; }
        public List<AbilityObj> Abilities {get;} = new() 
        {
            new() {count=0,Type=AbilityType.CTool},
            new() {count=0,Type=AbilityType.CPrevious}
        };

        public GameData()
        {
            Coins = PlayerPrefs.GetInt("Coins", 200);
            LevelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 1);
            
            foreach (var ability in Abilities)
            {
                ability.count = PlayerPrefs.GetInt("Ability" + ability.Type, 0); 
            }
        }

        public void UseAbility(AbilityType type)
        {
            foreach (var ability in Abilities.Where(t => t.Type == type))
            {
                ability.count--;
                if (ability.count < 0) ability.count = 0;
                PlayerPrefs.SetInt("Ability" + ability.Type, ability.count);
                break;
            }
        }
        public void AddAbility(AbilityObj booster)
        {
            foreach (var ability in Abilities)
            {
                if (ability.Type == booster.Type)
                {
                    ability.count += 1;       
                    PlayerPrefs.SetInt("Ability" + ability.Type, ability.count);
                    break;
                }
            }
        }

        public void LevelCompleted(int currentLevel)
        {
            if (currentLevel >= LevelsCompleted)
            {
                LevelsCompleted = currentLevel + 1;
                PlayerPrefs.SetInt("LevelsCompleted", LevelsCompleted);
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
        CTool=1,
        CPrevious=2,
    }
}