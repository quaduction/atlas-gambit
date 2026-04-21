using System;
using System.Collections.Generic;
using System.Linq;
using GRisk.Util;

namespace GRisk.Engine
{
    public class GR
    {
        private Dictionary<string, uint[]> boardState = new();

        List<uint> players;
        int currentPlayerIndex;
        int currentPhaseIndex;

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

        public bool nextPhase()
        {
            if (currentPhaseIndex < (int)GRTypes.Phase.END) currentPhaseIndex++;

            // if true, "end of the roll" -- nothing more can be done in this turn
            return currentPhaseIndex == (int)GRTypes.Phase.END;
        }

        public void nextTurn()
        {
            currentPhaseIndex = 0;

            if (players.Count == 0)
                return;

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        bool tilesAdjacent(string fromId, string toId)
        {
            TerritoryData data = GRData.territoryDict[fromId];

            if (data.adjacencies.Contains("*"))
                return true;

            return data.adjacencies.Contains(toId);
        }

        bool tilesAdjacentBidirectional(string fromId, string toId)
        {
            return tilesAdjacent(fromId, toId) && tilesAdjacent(toId, fromId);
        }

        public uint[] stateAt(string id)
        {
            return boardState[id];
        }

        public uint manpowerAt(string id)
        {
            return stateAt(id)[0];
        }

        public uint ownerAt(string id)
        {
            return stateAt(id)[1];
        }

        uint setOwnerAt(string id, uint owner)
        {
            return stateAt(id)[1] = owner;
        }

        void conformOwnerAt(string id)
        {
            if (manpowerAt(id) == 0)
                setOwnerAt(id, 0);
        }

        uint setManpowerAt(string id, uint manpower)
        {
            stateAt(id)[0] = manpower;

            conformOwnerAt(id);

            return manpower;
        }

        uint addManpowerAt(string id, uint manpower)
        {
            uint current = manpowerAt(id);
            uint maxAdd = uint.MaxValue - current;
            uint added = Math.Min(manpower, maxAdd);

            setManpowerAt(id, current + added);

            return added;
        }

        uint subManpowerAt(string id, uint manpower)
        {
            uint current = manpowerAt(id);
            uint removed = Math.Min(manpower, current);

            setManpowerAt(id, current - removed);

            return removed;
        }

        uint moveManpower(string fromId, string toId, uint manpower)
        {
            return addManpowerAt(toId, subManpowerAt(fromId, manpower));
        }

        public uint draft(string id, uint manpower, uint player)
        {
            return addManpowerAt(id, manpower);
        }

        public bool canReinforce(string fromId, string toId, uint manpower, uint player)
        {
            return currentPhaseIndex == (int)GRTypes.Phase.REINFORCE
                   && tilesAdjacent(fromId, toId)
                   && ownerAt(fromId) == ownerAt(toId)
                   && ownerAt(fromId) == player
                   && manpowerAt(fromId) >= manpower;
        }

        public uint reinforce(string fromId, string toId, uint manpower, uint player)
        {
            if (!canReinforce(fromId, toId, manpower, player))
                return 0;

            return moveManpower(fromId, toId, manpower);
        }

        public bool canAttack(string fromId, string toId, uint manpower, uint player)
        {
            return currentPhaseIndex == (int)GRTypes.Phase.ATTACK
                   && tilesAdjacent(fromId, toId)
                   && ownerAt(fromId) == player
                   && ownerAt(toId) != player
                   && manpowerAt(fromId) >= manpower;
        }

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