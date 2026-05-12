using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRisk.Data;
using GRisk.Engine;
using UnityEngine;
using Random = System.Random;

namespace GRisk
{
    public class EntityAI : MonoBehaviour
    {
        public GRFacade facade;

        public uint selfId = 666;
        public uint defenseMovement = 5;
        public int opWait = 100;

        public GR engine;

        private Random rand = new();

        public async void process()
        {
            if (engine.currentPlayer() != selfId) return;
            
            switch (engine.currentPhase())
            {
                case GRTypes.Phase.ITEMS:
                    await itemPhase();
                    break;

                case GRTypes.Phase.REINFORCE:
                    await defensePhase();
                    break;

                case GRTypes.Phase.ATTACK:
                    await attackPhase();
                    break;
            }

            facade.nextPhase();
            facade.tileManager.updateTiles();

            process();
        }

        private async Task visibilityPause()
        {
            facade.tileManager.updateTiles();
            await Task.Delay(opWait);
        }

        private string[] adjacents(string territoryId)
        {
            List<string> adjacentIds = new();

            foreach (string id in GRData.territoryDict.Keys)
                if (engine.tilesAdjacent(id, territoryId))
                    adjacentIds.Add(id);

            return adjacentIds.ToArray();
        }

        private string[] unownedAdjacents(string territoryId)
        {
            string[] adjacentIds = adjacents(territoryId);

            List<string> unownedIds = new();

            foreach (string id in adjacentIds)
                if (engine.ownerAt(id) != selfId)
                    unownedIds.Add(id);

            return unownedIds.ToArray();
        }

        private string[] ownedAdjacents(string territoryId)
        {
            string[] adjacentIds = adjacents(territoryId);

            List<string> ownedIds = new();

            foreach (string id in adjacentIds)
                if (engine.ownerAt(id) == selfId)
                    ownedIds.Add(id);

            return ownedIds.ToArray();
        }

        private string[] owned()
        {
            List<string> ownedIds = new();

            foreach (string id in GRData.territoryDict.Keys)
                if (engine.ownerAt(id) == selfId)
                    ownedIds.Add(id);

            return ownedIds.ToArray();
        }

        private async Task itemPhase()
        {
            string[] ownedIds = owned();
        
            engine.draft(ownedIds[rand.Next(ownedIds.Length)], 10u, selfId);
            await visibilityPause();
        }

        private async Task defensePhase()
        {
            for (int i = 0; i < 12; i++)
            {
                foreach (string id in owned()) await plagueDefense(id);
                await visibilityPause();
            }
        }

        private async Task plagueDefense(string territoryId)
        {
            string[] adjacentEnemy = unownedAdjacents(territoryId);
            
            if (adjacentEnemy.Length > 0) return;

            uint mp = engine.manpowerAt(territoryId);

            if (mp <= defenseMovement) return;

            string[] adjacentSelf = ownedAdjacents(territoryId);

            for (int i = 0; i < adjacentSelf.Length; i++)
            {
                facade.move(
                    territoryId,
                    adjacentSelf[rand.Next(adjacentSelf.Length)],
                    defenseMovement
                );
                await visibilityPause();
            }
        }

        private async Task attackPhase()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (string id in owned())
                {
                    plagueAttack(id);
                    await visibilityPause();
                }
                
            }
        }

        private void plagueAttack(string territoryId)
        {
            string[] adjacentEnemy = unownedAdjacents(territoryId);

            if (adjacentEnemy.Length == 0) return;

            string target = adjacentEnemy[rand.Next(adjacentEnemy.Length)];

            uint mp = engine.manpowerAt(territoryId);

            if (engine.manpowerAt(territoryId) > mp) return;

            facade.move(territoryId, target, (uint)(mp * 0.75));
        }
    }
}