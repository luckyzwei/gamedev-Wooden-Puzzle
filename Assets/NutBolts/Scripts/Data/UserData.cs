using UnityEngine;

namespace NutBolts.Scripts.Data
{
    public class UserData : MonoBehaviour
    {
        private static UserData instance;
        public static UserData Instance
        {
            get 
            {
                if (instance != null) return instance;
                
                GameObject go = new GameObject("UserData");
                instance = go.AddComponent<UserData>();
                instance.LoadLocalData();
                return instance; 
            }
        }
        public  GameData CGameData { get; private set; }
        public  SettingData CSettingData { get; private set; }
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }


        public void AddBooster(BoosterObj boosterObj)
        {
            CGameData.AddBooster(boosterObj);
            SaveLocalData();
        }
        public void SubBooster(BoosterType boostertype)
        {
            CGameData.UseBooster(boostertype);
            SaveLocalData();
        }
        public BoosterObj GetBoosterObj(BoosterType type)
        {
            for (int i = 0; i < CGameData.Boosters.Count; i++) {
                if (CGameData.Boosters[i].boosterType == type)
                {
                    return CGameData.Boosters[i];
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
                CSettingData = new SettingData();
            }
            else
            {
                CSettingData = JsonUtility.FromJson<SettingData>(jsonSettingString);
           
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
