using System.Collections.Generic;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Interface
{
    public class TileManager : MonoBehaviour
    {
        public GR engine;
        public List<TerritoryTile> tiles = new();
        
        public void registerTile(TerritoryTile tile)
        {
            tiles.Add(tile);
            updateTile(tile);
        }

        public void updateTile(TerritoryTile tile)
        {
            
            tile.setState(engine.stateAt(tile.territoryId));
            tile.updateVisuals();
        }

        public void updateTiles()
        {
            foreach (TerritoryTile tile in tiles) updateTile(tile);
        }

        public void onConsumable(TerritoryTile tile, ConsumableItemAttributes consumable)
        {
            Debug.Log("[TileManager] Got consumable");
            Debug.Log(consumable);
        }
    }
}