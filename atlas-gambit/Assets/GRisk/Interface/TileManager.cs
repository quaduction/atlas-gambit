using System.Collections.Generic;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Interface
{
    public class TileManager : MonoBehaviour
    {
        public GR engine;
        public GRFacade facade;
        public List<TerritoryTile> tiles = new();

        private ConsumableItem focuser;
        private bool focusLocked = false;

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

        public void unfocusTiles()
        {
            focusLocked = false;

            foreach (TerritoryTile tile in tiles) tile.setFocus(false);

            updateTiles();
        }

        public void focusTile(TerritoryTile tile)
        {
            if (focusLocked) return;

            unfocusTiles();
            tile.setFocus(true);
        }

        public void onConsumable(TerritoryTile tile, ConsumableItem consumable)
        {
            if (consumable.consumed) return;

            if (focusLocked && !(tile.getFocus() /*|| consumable == focuser*/)) return;

            if (consumable.applyFocus)
            {
                focusTile(tile);
                consumable.focus = tile;

                if (consumable.focusLock) focusLocked = true;
            }

            bool consumed = facade.handleConsumable(consumable, tile.territoryId);

            if (consumed)
            {
                if (consumable.destroyOnConsume)
                {
                    consumable.consumed = true;
                    Destroy(consumable.gameObject);
                }
            }

            updateTiles();
        }
    }
}