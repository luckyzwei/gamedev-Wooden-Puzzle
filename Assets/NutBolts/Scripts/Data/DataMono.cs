using UnityEngine;

namespace NutBolts.Scripts.Data
{
    public class DataMono : MonoBehaviour
    {
        private static DataMono instance;
        public static DataMono Instance
        {
            get 
            {
                if (instance != null) return instance;
                
                GameObject go = new GameObject("UserData");
                instance = go.AddComponent<DataMono>();
                instance.LoadLocalData();
                return instance; 
            }
        }
        public  GameData CGameData { get; private set; }
        public  SettingInfo CSettingData { get; private set; }
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }


        public void AddBooster(AbilityObj boosterObj)
        {
            CGameData.AddAbility(boosterObj);
            SaveLocalData();
        }
        public void SubBooster(AbilityType boostertype)
        {
            CGameData.UseAbility(boostertype);
            SaveLocalData();
        }
        public AbilityObj GetBoosterObj(AbilityType type)
        {
            foreach (var t in CGameData.Abilities)
            {
                if (t.Type == type)
                {
                    return t;
                }
            }

            return null;
        }
        private void LoadLocalData()
        {
            string jsonString = PlayerPrefs.GetString("GameData", "");
            if (jsonString == string.Empty)
            {
                CGameData = new GameData();
            }
            else
            {
                CGameData = JsonUtility.FromJson<GameData>(jsonString);
            }
            string jsonSettingString = PlayerPrefs.GetString("SettingData", "");
            if (jsonSettingString == string.Empty)
            {
                CSettingData = new SettingInfo();
            }
            else
            {
                CSettingData = JsonUtility.FromJson<SettingInfo>(jsonSettingString);
           
            }
            SaveLocalData();     
        }
        public void SaveLocalData()
        {
            string jsonString = JsonUtility.ToJson(CGameData);
            string jsonSettingStr = JsonUtility.ToJson(CSettingData);
            PlayerPrefs.SetString("GameData",jsonString);
            PlayerPrefs.SetString("SettingData", jsonSettingStr);
        }
    }
}
