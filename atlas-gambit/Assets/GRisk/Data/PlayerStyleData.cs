using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRisk.Data
{
    [Serializable]
    public class PlayerStyleData : ISerializationCallbackReceiver
    {
        public uint id;
        public string hex;

        [NonSerialized] public Color color = Color.white;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            ColorUtility.TryParseHtmlString(hex, out color);
        }
    }

    [Serializable]
    public class PlayerStyleDataList
    {
        public PlayerStyleData[] playerStyleData;

        public Dictionary<uint, PlayerStyleData> asDict()
        {
            Dictionary<uint, PlayerStyleData> styleDict = new();
            foreach (PlayerStyleData style in playerStyleData)
            {
                styleDict[style.id] = style;
            }

            return styleDict;
        }
    }
}