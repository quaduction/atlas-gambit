using GRisk.Engine;
using GRisk.Interaction.Item;

namespace GRisk.Content.Grenade
{
    public class GrenadeConsumable : ConsumableItem
    {
        public override void territoryEffect(GRFacade facade, string territoryId)
        {
            if ((uint)GRTypes.Player.NONE == facade.engine.ownerAt(territoryId))
            {
                facade.engine.setOwnerAt(territoryId, (uint)owner);
            }
        }
    }
}