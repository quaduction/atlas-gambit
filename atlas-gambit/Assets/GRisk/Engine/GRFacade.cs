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
        public GRSound soundPlayer;
        public TileManager tileManager;

        public UnityEvent phaseChange = new();
        public UnityEvent turnChange = new();
        public UnityEvent winCondition = new();
        public UnityEvent lossCondition = new();

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

            // log($"{territory.name} ({territory.id}) held by player {state[1]} with {state[0]} mp", id);
            log($"{territory.name} ({territory.id})", id);
        }

        public void nextPhase()
        {
            bool endPhase = engine.nextPhase();
            if (endPhase)
            {
                notifier.log($"Fin de phases! Le tour de joueur #{engine.currentPlayer()} est fini");
                nextTurn();
            }
            else
            {
                notifier.log($"Phase de {engine.currentPhase().ToString()}");
            }

            phaseChange.Invoke();
            soundPlayer.playsound("ping-squelch", gameObject.transform);
        }

        public void nextTurn()
        {
            engine.nextTurn();
            turnChange.Invoke();
            
            tileManager.updateTiles();

            notifier.log($"Tour de joueur #{engine.currentPlayer()}");
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
                        if (engine.currentPlayer() == (uint)GRTypes.Player.ENTITY) return;
                        
                        log("Mouvement REINFORCE invalide", toId);
                        deny(fromId);
                        return;
                    }

                    engine.reinforce(fromId, toId, manpower, player);
                    soundPlayer.playsound("marching", tileManager.firstTileFor(toId).transform);
                    
                    break;

                case GRTypes.Phase.ATTACK:
                    if (!engine.canAttack(fromId, toId, manpower, player))
                    {
                        if (engine.currentPlayer() == (uint)GRTypes.Player.ENTITY) return;
                        
                        log("Mouvement ATTACK invalide", toId);
                        deny(fromId);
                        return;
                    }

                    engine.attack(fromId, toId, manpower, player);
                    soundPlayer.playsound("ping-attack", tileManager.firstTileFor(toId).transform);

                    if (engine.winCondition((uint)GRTypes.Player.PLAYER))
                    {
                        log("WOO! Vous avez eradiqué l'entité!", toId);
                        winCondition.Invoke();
                        return;
                    }

                    if (engine.winCondition((uint)GRTypes.Player.ENTITY))
                    {
                        log("Vous êtes morts :(", toId);
                        lossCondition.Invoke();
                        return;
                    }

                    break;

                default:
                    log("Impossible de bouger hors de REINFORCE ou ATTACK", toId);
                    deny(fromId);
                    return;
            }
            
            // check(toId);
        }

        // feedback

        private void log(string msg, string sourceId)
        {
            notifier.extraLog(msg, tileManager.firstTileFor(sourceId).transform);
        }

        private void deny(string sourceId)
        {
            if (engine.currentPlayer() == (uint)GRTypes.Player.ENTITY) return;
            soundPlayer.playsound("ping-nope", tileManager.firstTileFor(sourceId).transform);
        }
    }
}