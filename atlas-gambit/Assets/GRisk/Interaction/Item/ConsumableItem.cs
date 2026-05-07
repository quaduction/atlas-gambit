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
        [CanBeNull] public TerritoryTile focusedTile;

        public void releaseFocus()
        {
            if (focusedTile == null) return;
            
            focusedTile.manager.releaseFocus();
            focusedTile = null;
        }
        
        // runs after TileManager focus checks but before GRFacade ownership checks
        public virtual void territoryEffect(GRFacade facade, string territoryId)
        {
        }

        // runs if the TileManager is holding focus from this same ConsumableItem when it touches a non-focused tile.
        // this happens regardless of focus lock.
        public virtual void secondaryFocusEffect(GRFacade facade, string territoryId)
        {
            
        }
    }
}