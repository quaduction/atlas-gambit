using GRisk.Data;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Content.DebugPawn
{
    [RequireComponent(typeof(Renderer))]
    public class DebugPawnBehaviour : ItemBehaviour
    {
        public DebugPawnConsumable consumable;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            
            updateColor();
        }

        public override void onSecondary()
        {
            consumable.owner = consumable.owner == GRTypes.Player.PLAYER ? GRTypes.Player.ENTITY : GRTypes.Player.PLAYER;
            
            updateColor();
        }

        private void updateColor()
        {
            renderer.material.color = GRData.playerStyleDict[(uint)consumable.owner].color;
        }
    }
}