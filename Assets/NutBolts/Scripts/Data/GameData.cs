using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
    public int TotalCoin;
    public int vip;
    public int CurrentLevel;
    public List<BoosterObj> Boosters;
    public GameData(){
        TotalCoin = 200;
        vip = 0;
        Boosters = new List<BoosterObj>
        {
            new BoosterObj{number=2,boosterType=BoosterType.CReset},
            new BoosterObj{number=2,boosterType=BoosterType.CTool},
            new BoosterObj{number=2,boosterType=BoosterType.CTip},
            new BoosterObj{number=2,boosterType=BoosterType.CPrevious}
        };
     }
    public void UseBooster(BoosterType type)
    {
        for(int i=0; i<Boosters.Count; i++)
        {
            if (Boosters[i].boosterType == type)
            {
                Boosters[i].number--;
                if (Boosters[i].number < 0) Boosters[i].number = 0;
                break;
            }
        }
    }
    public void AddBooster(BoosterObj booster)
    {
        for (int i = 0; i < Boosters.Count; i++)
        {
            if (Boosters[i].boosterType == booster.boosterType)
            {
                Boosters[i].number+=booster.number;               
                break;
            }
        }
    }
}
[Serializable]
public class SettingData {
    public bool isSound;
    public bool isMusic;
    public bool isShake;
    public SettingData()
    {
        isSound = true;
        isMusic = true;
        isShake = true;
    }
}
[Serializable]
public class BoosterObj
{
    public BoosterType boosterType;
    public int number;
}
public enum BoosterType
{
    CReset=0,
    CTool=1,
    CPrevious=2,
    CTip=3,

}


