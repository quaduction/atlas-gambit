using UnityEngine;
using UnityEngine.Events;

namespace GRisk.Interface
{
    public class TerritoryTile : MonoBehaviour
    {
        public string territoryId;
        public TileManager manager;
        
        public UnityEvent propPlaced;
        
        private uint manpower;
        private uint owner;

        public void setState(uint[] state)
        {
            manpower = state[0];
            owner = state[1];
        }
    }
}