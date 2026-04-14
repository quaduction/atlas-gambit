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
    }
}