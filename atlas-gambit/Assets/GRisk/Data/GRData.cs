using System.Collections.Generic;
using GRisk.Util;
using UnityEngine;

namespace GRisk.Data
{
    public static class GRData
    {
        public static TerritoryDataList territoryData;
        public static Dictionary<string, TerritoryData> territoryDict;
        
        public static PlayerStyleDataList playerStyleData;
        public static Dictionary<uint, PlayerStyleData> playerStyleDict;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void init()
        {
            territoryData = JLoader<TerritoryDataList>.load("territories");
            territoryDict = territoryData.asDict();
            
            playerStyleData = JLoader<PlayerStyleDataList>.load("playerstyles");
            playerStyleDict = playerStyleData.asDict();
            
            Debug.Log(playerStyleDict);
        }
    }
}