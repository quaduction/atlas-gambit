using GRisk.Engine;
using UnityEngine;

namespace GRisk
{
    public class GameMaster : MonoBehaviour
    {
        public GR engine;
        public GRFacade facade;

        void Start()
        {
            engine = new GR();
            facade.engine = engine;
        }
    }
}