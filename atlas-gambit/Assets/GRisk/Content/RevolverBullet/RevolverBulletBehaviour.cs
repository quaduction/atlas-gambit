using GRisk.Data;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Content.Bullet
{
    [RequireComponent(typeof(Renderer))]
    public class BulletBehaviour : ItemBehaviour
    {
        public BulletConsumable consumable;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }
    }
}