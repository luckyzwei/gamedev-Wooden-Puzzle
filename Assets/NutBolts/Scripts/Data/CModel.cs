using System;
using System.Collections.Generic;
using UnityEngine;

namespace NutBolts.Scripts.Data
{
    [Serializable]
    public class LevelObject
    {
        public int maxCol;
        public int maxRow;
        public List<Side> sides;
        public List<int> tipPaths;
        public int[] nuts;
        public List<RewardObj> rewards;
        public LevelObject()
        {
            sides = new List<Side>();
            rewards = new List<RewardObj> { new() { rewardType = RewardType.Coin, amount = 50 } };
            tipPaths = new List<int>();
        }
    }
    [Serializable]
    public class Side
    {
        public int id;
        public string prefabName;
        public int rotBlockDegree;
        public List<int> dots;
        public float Tail;
        public float Head;
    }
    [Serializable]
    public class ObstacleSide
    {  
        public int id;
        public Vector3 position;
        public Vector3 eulerAngle;
        public List<int> dots;
        public ObstacleSide()
        {
            dots = new List<int>();
        }
    }
    [Serializable]
    public class RewardObj{
        public RewardType rewardType;
        public int amount;
        public AbilityObj ConvertRewardToBooster()
        {
            AbilityObj b = new AbilityObj();
            switch (rewardType)
            {
                case RewardType.Coin: return null;
                case RewardType.Tool: b.Type = AbilityType.CTool; break;
                default: return null;
            }
            return b;
        }
    }

    [Serializable]
    public enum CSquare
    {
        None = 0,
        Nut = 1,
        Hole = 2,   
        HoleVideo = 3,
        HoleCoin = 4
    
    }
    public enum RewardType
    {
        Coin=0,Tool=1,Reset=2,Tip=3
    }
    public enum EditState
    {
        None = 0,
        EditSideAndBlock=1,
        AvailableAddSide = 2,
        BlockAddSide = 3,
        AvailableAddHole = 4,
    }
}