using System.Collections.Generic;
using GRisk.Util;
using UnityEngine;

namespace GRisk
{
    public static class GRData
    {
        public static TerritoryDataList territoryData;
        public static Dictionary<string, TerritoryData> territoryDict;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void init()
        {
            territoryData = JLoader<TerritoryDataList>.load("territories");
            territoryDict = territoryData.asDict();
        }
    }
}