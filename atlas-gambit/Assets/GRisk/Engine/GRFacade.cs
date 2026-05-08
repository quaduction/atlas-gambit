using System;
using GRisk.Data;
using GRisk.Interaction.Item;
using GRisk.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace GRisk.Engine
{
    public class GRFacade : MonoBehaviour
    {
        public GR engine;
        public Notifier notifier;

        public UnityEvent phaseChange = new();
        public UnityEvent turnChange = new();

        public bool handleConsumable(ConsumableItem consumable, string territoryId)
        {
            consumable.territoryEffect(this, territoryId);

            if (consumable.checkOwnership && (uint)consumable.owner != engine.ownerAt(territoryId)) return false;

            if (consumable.attributes.manpowerMut != 0)
                engine.mutManpowerAt(territoryId, consumable.attributes.manpowerMut);

            return true;
        }

        public void check(string id)
        {
            uint[] state = engine.stateAt(id);
            TerritoryData territory = GRData.territoryDict[id];

            notifier.log($"{territory.name} ({territory.id}) held by player {state[1]} with {state[0]} mp");
        }

        public void nextPhase()
        {
            bool endPhase = engine.nextPhase();
            if (endPhase)
            {
                notifier.log($"End phase! Player {engine.currentPlayer()}'s turn is over.");
                nextTurn();
            }
            else
            {
                notifier.log($"{engine.currentPhase().ToString()} phase");
            }

            phaseChange.Invoke();
        }

        public void nextTurn()
        {
            engine.nextTurn();
            turnChange.Invoke();

            notifier.log($"It is now player {engine.currentPlayer()}'s turn");
        }

        public void draft(string id, uint manpower)
        {
            engine.draft(id, manpower, engine.currentPlayer());
        }

        public void move(string fromId, string toId, uint manpower)
        {
            uint player = engine.currentPlayer();
            switch (engine.currentPhase())
            {
                case GRTypes.Phase.REINFORCE:
                    if (!engine.canReinforce(fromId, toId, manpower, player))
                    {
                        notifier.log("Invalid REINFORCE move (position, ownership, or manpower)");
                        break;
                    }

                    engine.reinforce(fromId, toId, manpower, player);
                    break;

                case GRTypes.Phase.ATTACK:
                    if (!engine.canAttack(fromId, toId, manpower, player))
                    {
                        notifier.log("Invalid ATTACK move (position, ownership, or manpower)");
                        break;
                    }

                    engine.attack(fromId, toId, manpower, player);
                    break;

                default:
                    notifier.log("Can't move outside of REINFORCE or ATTACK");
                    break;
            }
        }
    }
}