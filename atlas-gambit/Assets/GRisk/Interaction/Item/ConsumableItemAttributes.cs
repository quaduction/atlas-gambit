using UnityEngine;

namespace GRisk.Interaction.Item
{
    [System.Serializable]
    public struct ConsumableItemAttributes
    {
        public int manpowerMut;

        public ConsumableItemAttributes(int? mpmut)
        {
            manpowerMut = mpmut ?? 0;
        }
    }
}