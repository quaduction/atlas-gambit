using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRisk.Engine
{
    public class GRFacade : MonoBehaviour
    {
        public GR engine;

        public UnityEvent phaseChange = new();
        public UnityEvent turnChange  = new();

        public void check(string id)
        {
            uint[] state = engine.stateAt(id);
            TerritoryData territory = GRData.territoryDict[id];

            Debug.Log($"{territory.name} ({territory.id}) held by player {state[1]} with {state[0]} mp");
        }
        
        public void nextPhase()
        {
            bool endPhase = engine.nextPhase();
            if (endPhase)
            {
                Debug.Log($"End phase! Player {engine.currentPlayer()}'s turn is over.");
                nextTurn();
            }
            else
            {
                Debug.Log($"{engine.currentPhase().ToString()} phase");
            }
            
            phaseChange.Invoke();
        }

        public void nextTurn()
        {
            engine.nextTurn();
            turnChange.Invoke();
            
            Debug.Log($"It is now player {engine.currentPlayer()}'s turn");
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
                        Debug.Log("Invalid REINFORCE move (position, ownership, or manpower)");
                        break;
                    }
                    engine.reinforce(fromId, toId, manpower, player);
                    break;

                case GRTypes.Phase.ATTACK:
                    if (!engine.canAttack(fromId, toId, manpower, player))
                    {
                        Debug.Log("Invalid ATTACK move (position, ownership, or manpower)");
                        break;
                    }
                    engine.attack(fromId, toId, manpower, player);
                    break;

                default:
                    Debug.Log("Can't move outside of REINFORCE or ATTACK");
                    break;
            }
        }
    }
}