using GRisk.Engine;
using UnityEngine;

namespace GRisk.Interaction.Item
{
    public class ConsumableItem : MonoBehaviour
    {
        public bool destroyOnConsume = true;
        public bool consumed = false;
        
        public bool checkOwnership = true;
        public GRTypes.Player owner;
        
        public ConsumableItemAttributes attributes = new();
        
        public virtual void territoryEffect(string territoryId, GR engine){}
    }
}