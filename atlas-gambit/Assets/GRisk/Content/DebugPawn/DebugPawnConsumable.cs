using GRisk.Data;
using GRisk.Engine;
using GRisk.Interaction.Item;

namespace GRisk.Content.DebugPawn
{
    public class DebugPawnConsumable : ConsumableItem
    {
        public override void territoryEffect(string territoryId, GR engine)
        {
            if ((uint)GRTypes.Player.NONE == engine.ownerAt(territoryId))
            {
                engine.setOwnerAt(territoryId, (uint)owner);
            }
        }
    }
}