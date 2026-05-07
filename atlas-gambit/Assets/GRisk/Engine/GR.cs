using System;
using System.Collections.Generic;
using System.Linq;
using GRisk.Data;
using GRisk.Util;

namespace GRisk.Engine
{
    public class GR
    {
        private Dictionary<string, uint[]> boardState = new();

        List<uint> players = new();
        int currentPlayerIndex = 0;
        int currentPhaseIndex = 0;

        // Initialiser les provinces
        public GR()
        {
            foreach (string id in GRData.territoryDict.Keys)
            {
                boardState[id] = new uint[]
                {
                    0, // manpower (0 is "nothing")
                    (uint)GRTypes.Player.NONE, // owner (0 is unclaimed)
                };
            }
        }

        // Initialiser les joueurs
        public void registerPlayer(uint playerId)
        {
            if (playerId == (uint)GRTypes.Player.NONE)
                return; // reserved

            if (!players.Contains(playerId))
            {
                players.Add(playerId);
            }
        }

        public uint currentPlayer()
        {
            return players[currentPlayerIndex];
        }

        public GRTypes.Phase currentPhase()
        {
            return (GRTypes.Phase)currentPhaseIndex;
        }

        // Prochaine phase de tour
        // Les phases sont contenus dans un tour.
        public bool nextPhase()
        {
            if (currentPhaseIndex < (int)GRTypes.Phase.END) currentPhaseIndex++;

            // if true, "end of the roll" -- nothing more can be done in this turn
            return currentPhaseIndex == (int)GRTypes.Phase.END;
        }

        // Prochain tour
        // Les tours sont par joueur.
        public void nextTurn()
        {
            currentPhaseIndex = 0;

            if (players.Count == 0)
                return;

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        // Adjacences de provinces (de territories.json)
        bool tilesAdjacent(string fromId, string toId)
        {
            TerritoryData data = GRData.territoryDict[fromId];

            if (data.adjacencies.Contains("*"))
                return true;

            return data.adjacencies.Contains(toId);
        }

        // Provinces bidirectionnels
        bool tilesAdjacentBidirectional(string fromId, string toId)
        {
            return tilesAdjacent(fromId, toId) && tilesAdjacent(toId, fromId);
        }

        public uint[] stateAt(string id)
        {
            return boardState[id];
        }

        // [UTIL] Retourner le nombre de troupes sur une province
        public uint manpowerAt(string id)
        {
            return stateAt(id)[0];
        }

        // [UTIL] Retourner le propriétaire d'une province
        public uint ownerAt(string id)
        {
            return stateAt(id)[1];
        }

        // [UTIL] Définir le propriétaire d'une province
        public uint setOwnerAt(string id, uint owner)
        {
            return stateAt(id)[1] = owner;
        }

        void conformOwnerAt(string id)
        {
            if (manpowerAt(id) == 0)
                setOwnerAt(id, 0);
        }

        // [UTIL] Définir le nombre de troupes sur une province.
        uint setManpowerAt(string id, uint manpower)
        {
            stateAt(id)[0] = manpower;

            conformOwnerAt(id);

            return manpower;
        }

        // [UTIL] Ajouter nombre de troupes sur une province. 
        // ID de la province + nombre de troupes à ajouter
        public uint addManpowerAt(string id, uint manpower)
        {
            uint current = manpowerAt(id);
            uint maxAdd = uint.MaxValue - current;
            uint added = Math.Min(manpower, maxAdd);

            setManpowerAt(id, current + added);

            return added;
        }

        // [UTIL] Soustraire le nombre de troupes
        public uint subManpowerAt(string id, uint manpower)
        {
            uint current = manpowerAt(id);
            uint removed = Math.Min(manpower, current);

            setManpowerAt(id, current - removed);

            return removed;
        }

        public uint mutManpowerAt(string id, int manpower)
        {
            return (
                Math.Sign(manpower) >= 0
                    ? (Func<string, uint, uint>)addManpowerAt
                    : subManpowerAt
            )(id, (uint)Math.Abs(manpower));
        }

        // [UTIL] Bouger des troupes d'une province à une autre
        // ID de la province de départ, ID de la province de destination, nombre de troupes
        uint moveManpower(string fromId, string toId, uint manpower)
        {
            return addManpowerAt(toId, subManpowerAt(fromId, manpower));
        }

        // Ajouter des troupes pour un joueur
        // ID de la province + nombre de troupes à ajouter, joueur
        public uint draft(string id, uint manpower, uint player)
        {
            return addManpowerAt(id, manpower);
        }

        // Check pour mouvement de troupes
        // ID de la province de départ, ID de la province de destination, nombre de troupes, joueur
        // Si la phase est REINFORCE (mouvement) et les provinces sont adjacentes du même joueur.
        public bool canReinforce(string fromId, string toId, uint manpower, uint player)
        {
            return currentPhaseIndex == (int)GRTypes.Phase.REINFORCE
                   && tilesAdjacent(fromId, toId)
                   && ownerAt(fromId) == ownerAt(toId)
                   && ownerAt(fromId) == player
                   && manpowerAt(fromId) >= manpower;
        }

        // Mouvement de troupes
        // ID de la province de départ, ID de la province de destination, nombre de troupes, joueur
        public uint reinforce(string fromId, string toId, uint manpower, uint player)
        {
            if (!canReinforce(fromId, toId, manpower, player))
                return 0;

            return moveManpower(fromId, toId, manpower);
        }

        // Check pour attaque
        // ID de la province de départ, ID de la province de destination, nombre de troupes, joueur
        // Si la phase est ATTACK (attaque) et les provinces sont adjacentes de joueurs différents.
        public bool canAttack(string fromId, string toId, uint manpower, uint player)
        {
            return currentPhaseIndex == (int)GRTypes.Phase.ATTACK
                   && tilesAdjacent(fromId, toId)
                   && ownerAt(fromId) == player
                   && ownerAt(toId) != player
                   && manpowerAt(fromId) >= manpower;
        }

        // Attaque
        // Simule une escarmouche pour calculer les pertes de troupes
        // ID de la province de départ, ID de la province de destination, nombre de troupes, joueur
        public uint attack(string fromId, string toId, uint manpower, uint player)
        {
            if (!canAttack(fromId, toId, manpower, player))
                return 0;

            uint[] outcome = GRMath.skirmish(manpower, manpowerAt(toId));
            uint remainder = outcome[0];
            uint captureType = outcome[1];

            return 0;
        }
    }
}