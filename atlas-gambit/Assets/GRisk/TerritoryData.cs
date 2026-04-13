using System;
using System.Collections.Generic;

namespace GRisk
{
    [Serializable]
    public class TerritoryData
    {
        public string id;
        public string name;
        public string continent;
        public string[] adjacencies;
    }

    [Serializable]
    public class TerritoryDataList
    {
        public TerritoryData[] territories;

        public Dictionary<string, TerritoryData> asDict()
        {
            Dictionary<string, TerritoryData> terrDict = new Dictionary<string, TerritoryData>();
            foreach (TerritoryData territory in territories)
            {
                terrDict[territory.id] = territory;
            }

            return terrDict;
        }
    }
}