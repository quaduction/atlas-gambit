using GRisk.Engine;
using GRisk.Interface;
using JetBrains.Annotations;
using UnityEngine;

namespace GRisk.Interaction.Item
{
    public class ConsumableItem : MonoBehaviour
    {
        public ConsumableItemAttributes attributes;

        public bool destroyOnConsume = true;
        public bool consumed = false;

        public bool checkOwnership = true;
        public GRTypes.Player owner;

        public bool applyFocus = false;
        public bool focusLock = false;
        [CanBeNull] public TerritoryTile focus;

        public void releaseFocus()
        {
            if (focus == null) return;
            
            focus.manager.unfocusTiles();
            focus = null;
        }
        
        // runs after TileManager focus checks but before GRFacade ownership checks
        public virtual void territoryEffect(GRFacade facade, string territoryId)
        {
        }
    }
}