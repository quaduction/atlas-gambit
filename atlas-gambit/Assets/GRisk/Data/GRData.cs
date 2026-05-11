using System.Collections.Generic;
using GRisk.Util;
using UnityEngine;

namespace GRisk.Data
{
    public static class GRData
    {
        public static TerritoryDataList territoryData;
        public static Dictionary<string, TerritoryData> territoryDict;
        
        public static PlayerStyleDataList playerStyleData;
        public static Dictionary<uint, PlayerStyleData> playerStyleDict;
        
        public static Dictionary<string, AudioClip> soundLibrary = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void init()
        {
            territoryData = JLoader<TerritoryDataList>.load("territories");
            territoryDict = territoryData.asDict();
            
            playerStyleData = JLoader<PlayerStyleDataList>.load("playerstyles");
            playerStyleDict = playerStyleData.asDict();
            
            loadSoundAssets();
        }
        
        private static void loadSoundAssets()
        {
            // Load all sounds from the specified "Sounds" resource folder
            AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Sounds");

            foreach (AudioClip clip in audioClips)
            {
                // Use the clip's name as the key
                soundLibrary.Add(clip.name, clip);
                Debug.Log("Loaded sound: " + clip.name);
            }
        }
    }
}