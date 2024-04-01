using UnityEngine;

namespace NutBolts.Scripts.Data
{
    public class DataMono : MonoBehaviour
    {
        public  GameData Data { get; private set; }
        public  SettingInfo SettingData { get; private set; }
        private void Awake()
        {
            LoadSaved();
        }
        
        public void AddAbility(AbilityObj ani)
        {
            Data.AddAbility(ani);
            SaveAll();
        }
        public void SubAbility(AbilityType type)
        {
            Data.UseAbility(type);
            SaveAll();
        }
        public AbilityObj GetAbilityObj(AbilityType type)
        {
            foreach (var t in Data.Abilities)
            {
                if (t.Type == type)
                {
                    return t;
                }
            }

            return null;
        }
        private void LoadSaved()
        {
            string jsonString = PlayerPrefs.GetString("GameData", "");
            if (jsonString == string.Empty)
            {
                Data = new GameData();
            }
            else
            {
                Data = JsonUtility.FromJson<GameData>(jsonString);
            }
            string jsonSettingString = PlayerPrefs.GetString("SettingData", "");
            if (jsonSettingString == string.Empty)
            {
                SettingData = new SettingInfo();
            }
            else
            {
                SettingData = JsonUtility.FromJson<SettingInfo>(jsonSettingString);
           
            }
            SaveAll();     
        }
        public void SaveAll()
        {
            string jsonString = JsonUtility.ToJson(Data);
            string jsonSettingStr = JsonUtility.ToJson(SettingData);
            PlayerPrefs.SetString("GameData",jsonString);
            PlayerPrefs.SetString("SettingData", jsonSettingStr);
        }
    }
}
