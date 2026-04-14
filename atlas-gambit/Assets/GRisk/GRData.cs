using GRisk.Util;
using UnityEngine;

namespace GRisk
{
    public static class GRData
    {
        public static TerritoryDataList territoryData;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void init()
        {
            territoryData = JLoader<TerritoryDataList>.load("territories");
        }
    }
}