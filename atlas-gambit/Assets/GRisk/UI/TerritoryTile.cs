using UnityEngine;
using UnityEngine.Events;

namespace GRisk.UI
{
    public class TerritoryTile : MonoBehaviour
    {
        public string territoryId;
        
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