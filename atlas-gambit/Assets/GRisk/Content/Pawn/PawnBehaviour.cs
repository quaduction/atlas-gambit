using GRisk.Data;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Content.Pawn
{
    [RequireComponent(typeof(Renderer))]
    public class PawnBehaviour : ItemBehaviour
    {
        public PawnConsumable consumable;

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