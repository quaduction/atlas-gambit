using GRisk.Data;
using GRisk.Engine;
using GRisk.Interface;
using GRisk.Util;
using UnityEditor.SearchService;
using UnityEngine;

namespace GRisk
{
    public class GameMaster : MonoBehaviour
    {
        public GR engine;
        public GRFacade facade;
        public TileManager tileManager;

        private void Awake()
        {
            engine = new GR();
            facade.engine = engine;
            tileManager.engine = engine;
            tileManager.facade = facade;
            
            loadScenario("ownership");
        }

        private void loadScenario(string scenarioName)
        {
            ScenarioPlayerData[] scenarioPlayers = JLoader<ScenarioDataList>.load(scenarioName).scenario;

            foreach (ScenarioPlayerData scenarioPlayerData in scenarioPlayers)
            {
                uint playerId = scenarioPlayerData.playerId;
                uint startMp = scenarioPlayerData.startMp;
                
                engine.registerPlayer(playerId);

                foreach (string territoryId in scenarioPlayerData.territories)
                {
                    engine.setOwnerAt(territoryId, playerId);
                    engine.setManpowerAt(territoryId, startMp);
                }
            }
        }
    }
}