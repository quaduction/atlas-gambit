using System;
using GRisk.Engine;
using GRisk.Interaction.Item;

namespace GRisk.Content.Grabber
{
    public class GrabberConsumable : ConsumableItem
    {
        public uint moveAmount = 0u;
        
        private void Reset()
        {
            destroyOnConsume = false;
            focusLock = true;
        }

        public override void focusEffect(GRFacade facade, string territoryId)
        {
            facade.soundPlayer.playsound("ping-squelch", gameObject.transform);
        }

        public override void secondaryFocusEffect(GRFacade facade, string territoryId)
        {
            if (focusedTile == null) return;
            
            facade.move(focusedTile.territoryId, territoryId, moveAmount);
        }
    }
}