using GRisk.Engine;
using GRisk.Interface;
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
        }
    }
}