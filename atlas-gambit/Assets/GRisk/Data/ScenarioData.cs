using System;

namespace GRisk.Data
{
    [Serializable]
    public class ScenarioPlayerData
    {
        public uint playerId;
        public uint startMp;
        public string[] territories;
    }

    [Serializable]
    public class ScenarioDataList
    {
        public ScenarioPlayerData[] scenario;
    }
}