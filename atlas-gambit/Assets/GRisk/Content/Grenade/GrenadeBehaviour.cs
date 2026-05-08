using GRisk.Data;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Content.Grenade
{
    [RequireComponent(typeof(Renderer))]
    public class GrenadeBehaviour : ItemBehaviour
    {
        public GrenadeConsumable consumable;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }
    }
}