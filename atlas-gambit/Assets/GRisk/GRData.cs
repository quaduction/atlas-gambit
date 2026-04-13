using GRisk.Util;

namespace GRisk
{
    public static class GRData
    {
        public static readonly TerritoryDataList territoryData = JLoader<TerritoryDataList>.load("territories");
    }
}