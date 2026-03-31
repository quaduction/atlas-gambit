using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GRisk
{
	public Dictionary<string, uint[]> boardState = new Dictionary<string, uint[]>();

	Dictionary<string, TerritoryData> territoryData = GRData.territoryData.asDict();
	
	List<uint> players = new List<uint>();
	int currentPlayerIndex = 0;

	public GRisk()
	{
		foreach (string id in territoryData.Keys)
		{
			boardState[id] = new uint[]
			{
				0, // manpower (0 is "nothing")
				0 // owner (0 is unclaimed)
			};
		}
	}

	public void registerPlayer(uint playerId)
	{
		if (playerId == 0) return; // reserved
		if (!players.Contains(playerId))
		{
			players.Add(playerId);
		}
	}

	public void nextTurn()
	{
		if (players.Count == 0) return;
		currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
	}

	bool tilesAdjacent(string fromId, string toId)
	{
		TerritoryData data = territoryData[fromId];
		return data.adjacencies.Contains(toId);
	}

	bool tilesAdjacentBidirectional(string fromId, string toId)
	{
		return tilesAdjacent(fromId, toId) && tilesAdjacent(toId, fromId);
	}

	uint manpowerAt(string id)
	{
		return boardState[id][0];
	}
	
	uint ownerAt(string id)
	{
		return boardState[id][1];
	}

	uint setOwnerAt(string id, uint owner)
	{
		return boardState[id][1] = owner;
	}

	void conformOwnerAt(string id)
	{
		if (manpowerAt(id) == 0) setOwnerAt(id, 0);
	}

	uint setManpowerAt(string id, uint manpower)
	{
		boardState[id][0] = manpower;
		
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
	
}